using AutoMapper;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Services;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Enums;
using Melobarbershop.Domain.Interfaces.Repositories;

namespace Melobarbershop.Application.Servicos.Implementacoes;

public class AgendamentoService : IAgendamentoService
{
    private readonly IAgendamentoRepository _agendamentoRepository;
    private readonly IServicoRepository _servicoRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IMapper _mapper;

    // Fuso horário fixo da barbearia (Brasil/Brasília = UTC-3).
    // Usar TimeZoneInfo explícito garante que o servidor sempre opere
    // em horário local brasileiro, independentemente do fuso configurado no SO.
    private static readonly TimeZoneInfo _fusoHorarioBrasilia =
        TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows()
                ? "E. South America Standard Time"   // ID no Windows
                : "America/Sao_Paulo");               // ID no Linux/Docker

    /// <summary>Retorna o DateTime atual no fuso horário de São Paulo.</summary>
    private static DateTime AgoraBrt() =>
        TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _fusoHorarioBrasilia);

    public AgendamentoService(
        IAgendamentoRepository agendamentoRepository,
        IServicoRepository servicoRepository,
        IUsuarioRepository usuarioRepository,
        IMapper mapper)
    {
        _agendamentoRepository = agendamentoRepository;
        _servicoRepository = servicoRepository;
        _usuarioRepository = usuarioRepository;
        _mapper = mapper;
    }

    public async Task<ApiResposta<AgendamentoDto>> ObterPorIdAsync(int id)
    {
        try
        {
            var agendamento = await _agendamentoRepository.ObterPorIdCompletoAsync(id);

            if (agendamento == null)
                return ApiResposta<AgendamentoDto>.Falha("Agendamento não encontrado.");

            var dto = _mapper.Map<AgendamentoDto>(agendamento);
            return ApiResposta<AgendamentoDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            return ApiResposta<AgendamentoDto>.Falha($"Erro ao obter o agendamento: {ex.Message}.");
        }
    }

    public async Task<ApiResposta<IEnumerable<AgendamentoDto>>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim, string? barbeiroId = null)
    {
        try
        {
            IEnumerable<Agendamento> agendamentos;

            if (!string.IsNullOrWhiteSpace(barbeiroId))
                agendamentos = await _agendamentoRepository.ObterPorBarbeiroEPeriodoAsync(barbeiroId, inicio, fim);
            else
                agendamentos = await _agendamentoRepository.ObterPorPeriodoAsync(inicio, fim);

            var dtos = _mapper.Map<IEnumerable<AgendamentoDto>>(agendamentos);
            return ApiResposta<IEnumerable<AgendamentoDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return ApiResposta<IEnumerable<AgendamentoDto>>.Falha($"Erro ao listar agendamentos por periodo: {ex.Message}");
        }
    }

    public async Task<ApiResposta<IEnumerable<AgendamentoDto>>> ListarPorClienteAsync(string clienteId)
    {
        try
        {
            var agendamentos = await _agendamentoRepository.ObterPorClienteAsync(clienteId);
            var dtos = _mapper.Map<IEnumerable<AgendamentoDto>>(agendamentos);
            return ApiResposta<IEnumerable<AgendamentoDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return ApiResposta<IEnumerable<AgendamentoDto>>.Falha($"Erro ao listar agendamentos do cliente '{clienteId}': {ex.Message}");
        }
    }

    public async Task<ApiResposta<AgendamentoDto>> CriarAsync(CriarAgendamentoDto dto)
    {
        try
        {
            if (dto.ServicoIds == null || !dto.ServicoIds.Any())
                return ApiResposta<AgendamentoDto>.Falha("Pelo menos um servico deve ser selecionado para o agendamento.");

            // Compara contra hora local BRT para evitar falsa rejeição quando o
            // servidor está em UTC e o slot foi gerado em hora local.
            if (dto.DataHoraInicio < AgoraBrt().AddMinutes(-5))
                return ApiResposta<AgendamentoDto>.Falha("A data e hora do agendamento nao pode ser no passado.");

            var cliente = await _usuarioRepository.ObterPorIdAsync(dto.ClienteId);
            if (cliente == null)
                return ApiResposta<AgendamentoDto>.Falha($"Cliente com ID '{dto.ClienteId}' nao encontrado.");
            if (!cliente.Ativo)
                return ApiResposta<AgendamentoDto>.Falha("O cliente informado esta desativado no sistema.");

            var barbeiro = await _usuarioRepository.ObterPorIdAsync(dto.BarbeiroId);
            if (barbeiro == null)
                return ApiResposta<AgendamentoDto>.Falha($"Barbeiro com ID '{dto.BarbeiroId}' nao encontrado.");
            if (!barbeiro.Ativo)
                return ApiResposta<AgendamentoDto>.Falha("O barbeiro informado esta desativado no sistema.");

            var servicos = (await _servicoRepository.ObterPorIdsAsync(dto.ServicoIds))
                .Where(s => s.Ativo)
                .ToList();

            if (servicos.Count != dto.ServicoIds.Distinct().Count())
                return ApiResposta<AgendamentoDto>.Falha("Um ou mais servicos selecionados nao foram encontrados ou estao inativos.");

            var duracaoTotalMinutos = servicos.Sum(s => s.DuracaoMinutos);
            var dataHoraFim = dto.DataHoraInicio.AddMinutes(duracaoTotalMinutos);

            var possuiBloqueio = await _usuarioRepository.ExisteBloqueioNoPeriodoAsync(dto.BarbeiroId, dto.DataHoraInicio, dataHoraFim);
            if (possuiBloqueio)
                return ApiResposta<AgendamentoDto>.Falha("O barbeiro selecionado possui um bloqueio de agenda no horario solicitado.");

            var possuiConflito = await _agendamentoRepository.ExisteConflitoDeHorarioAsync(dto.BarbeiroId, dto.DataHoraInicio, dataHoraFim, null);
            if (possuiConflito)
                return ApiResposta<AgendamentoDto>.Falha("Ja existe outro agendamento para este barbeiro no horario solicitado.");

            var agendamento = new Agendamento
            {
                ClienteId = dto.ClienteId,
                BarbeiroId = dto.BarbeiroId,
                DataHoraInicio = dto.DataHoraInicio,
                DataHoraFim = dataHoraFim,
                Origem = dto.Origem,
                Observacoes = dto.Observacoes,
                Status = StatusAgendamento.Confirmado,
                DataCriacao = DateTime.UtcNow,
                Itens = servicos.Select(s => new AgendamentoItem
                {
                    ServicoId = s.Id,
                    PrecoCobrado = s.Preco
                }).ToList()
            };

            await _agendamentoRepository.AdicionarAsync(agendamento);
            return await ObterPorIdAsync(agendamento.Id);
        }
        catch (Exception ex)
        {
            return ApiResposta<AgendamentoDto>.Falha($"Erro ao criar agendamento: {ex.Message}");
        }
    }
    public async Task<ApiResposta<AgendamentoDto>> ConfirmarAsync(int agendamentoId)
    {
        try
        {
            var agendamento = await _agendamentoRepository.ObterPorIdCompletoAsync(agendamentoId);
            if (agendamento == null)
                return ApiResposta<AgendamentoDto>.Falha($"Agendamento com ID {agendamentoId} nao encontrado.");

            if (agendamento.Status != StatusAgendamento.Pendente)
                return ApiResposta<AgendamentoDto>.Falha("Somente agendamentos pendentes podem ser confirmados.");

            agendamento.Status = StatusAgendamento.Confirmado;
            await _agendamentoRepository.AtualizarAsync(agendamento);

            return await ObterPorIdAsync(agendamento.Id);
        }
        catch (Exception ex)
        {
            return ApiResposta<AgendamentoDto>.Falha($"Erro ao confirmar agendamento com ID {agendamentoId}: {ex.Message}");
        }
    }

    public async Task<ApiResposta<AgendamentoDto>> IniciarAtendimentoAsync(int agendamentoId)
    {
        try
        {
            var agendamento = await _agendamentoRepository.ObterPorIdCompletoAsync(agendamentoId);
            if (agendamento == null)
                return ApiResposta<AgendamentoDto>.Falha($"Agendamento com ID {agendamentoId} nao encontrado.");

            if (agendamento.Status != StatusAgendamento.Confirmado)
                return ApiResposta<AgendamentoDto>.Falha("Somente agendamentos confirmados podem ter o atendimento iniciado.");

            agendamento.Status = StatusAgendamento.EmAtendimento;
            await _agendamentoRepository.AtualizarAsync(agendamento);

            return await ObterPorIdAsync(agendamento.Id);
        }
        catch (Exception ex)
        {
            return ApiResposta<AgendamentoDto>.Falha($"Erro ao iniciar atendimento do agendamento com ID {agendamentoId}: {ex.Message}");
        }
    }

    public async Task<ApiResposta<AgendamentoDto>> ConcluirAsync(int agendamentoId)
    {
        try
        {
            var agendamento = await _agendamentoRepository.ObterPorIdCompletoAsync(agendamentoId);
            if (agendamento == null)
                return ApiResposta<AgendamentoDto>.Falha($"Agendamento com ID {agendamentoId} nao encontrado.");

            if (agendamento.Status != StatusAgendamento.Confirmado && agendamento.Status != StatusAgendamento.EmAtendimento)
                return ApiResposta<AgendamentoDto>.Falha("Somente agendamentos confirmados podem ser concluidos.");

            agendamento.Status = StatusAgendamento.Concluido;
            await _agendamentoRepository.AtualizarAsync(agendamento);

            return await ObterPorIdAsync(agendamento.Id);
        }
        catch (Exception ex)
        {
            return ApiResposta<AgendamentoDto>.Falha($"Erro ao concluir agendamento com ID {agendamentoId}: {ex.Message}");
        }
    }

    public async Task<ApiResposta<AgendamentoDto>> CancelarAsync(int agendamentoId, string? motivo = null)
    {
        try
        {
            var agendamento = await _agendamentoRepository.ObterPorIdCompletoAsync(agendamentoId);
            if (agendamento == null)
                return ApiResposta<AgendamentoDto>.Falha($"Agendamento com ID {agendamentoId} nao encontrado.");

            if (agendamento.Status == StatusAgendamento.Concluido || agendamento.Status == StatusAgendamento.Cancelado)
                return ApiResposta<AgendamentoDto>.Falha("Nao e possivel cancelar um agendamento ja concluido ou ja cancelado.");

            agendamento.Status = StatusAgendamento.Cancelado;

            if (!string.IsNullOrWhiteSpace(motivo))
                agendamento.Observacoes = string.IsNullOrWhiteSpace(agendamento.Observacoes)
                    ? $"Cancelado: {motivo}"
                    : $"{agendamento.Observacoes} | Cancelado: {motivo}";

            await _agendamentoRepository.AtualizarAsync(agendamento);

            return await ObterPorIdAsync(agendamento.Id);
        }
        catch (Exception ex)
        {
            return ApiResposta<AgendamentoDto>.Falha($"Erro ao cancelar agendamento com ID {agendamentoId}: {ex.Message}");
        }
    }

    public async Task<ApiResposta<AgendamentoDto>> RegistrarNaoComparecimentoAsync(int agendamentoId)
    {
        try
        {
            var agendamento = await _agendamentoRepository.ObterPorIdCompletoAsync(agendamentoId);
            if (agendamento == null)
                return ApiResposta<AgendamentoDto>.Falha($"Agendamento com ID {agendamentoId} nao encontrado.");

            if (agendamento.Status != StatusAgendamento.Pendente && agendamento.Status != StatusAgendamento.Confirmado)
                return ApiResposta<AgendamentoDto>.Falha("Somente agendamentos pendentes ou confirmados podem ser marcados como nao comparecimento.");

            agendamento.Status = StatusAgendamento.NaoCompareceu;
            await _agendamentoRepository.AtualizarAsync(agendamento);

            return await ObterPorIdAsync(agendamento.Id);
        }
        catch (Exception ex)
        {
            return ApiResposta<AgendamentoDto>.Falha($"Erro ao registrar nao comparecimento do agendamento com ID {agendamentoId}: {ex.Message}");
        }
    }

    public async Task<ApiResposta<AgendamentoDto>> ReagendarAsync(int agendamentoId, ReagendarAgendamentoDto dto)
    {
        try
        {
            var agendamento = await _agendamentoRepository.ObterPorIdCompletoAsync(agendamentoId);
            if (agendamento == null)
                return ApiResposta<AgendamentoDto>.Falha($"Agendamento com ID {agendamentoId} nao encontrado.");

            if (agendamento.Status == StatusAgendamento.Concluido || agendamento.Status == StatusAgendamento.Cancelado)
                return ApiResposta<AgendamentoDto>.Falha("Nao e possivel reagendar um atendimento que ja foi concluido ou cancelado.");

            if (dto.NovoDataHoraInicio < AgoraBrt().AddMinutes(-5))
                return ApiResposta<AgendamentoDto>.Falha("O novo horario nao pode ser no passado.");

            var barbeiroId = !string.IsNullOrWhiteSpace(dto.NovoBarbeiroId) ? dto.NovoBarbeiroId : agendamento.BarbeiroId;

            var barbeiro = await _usuarioRepository.ObterPorIdAsync(barbeiroId);
            if (barbeiro == null)
                return ApiResposta<AgendamentoDto>.Falha($"Barbeiro com ID '{barbeiroId}' nao encontrado.");
            if (!barbeiro.Ativo)
                return ApiResposta<AgendamentoDto>.Falha("O barbeiro selecionado esta desativado no sistema.");

            var duracaoOriginal = agendamento.DataHoraFim - agendamento.DataHoraInicio;
            var novoDataHoraFim = dto.NovoDataHoraInicio.Add(duracaoOriginal);

            var possuiBloqueio = await _usuarioRepository.ExisteBloqueioNoPeriodoAsync(barbeiroId, dto.NovoDataHoraInicio, novoDataHoraFim);
            if (possuiBloqueio)
                return ApiResposta<AgendamentoDto>.Falha("O barbeiro possui um bloqueio de agenda no novo horario selecionado.");

            var possuiConflito = await _agendamentoRepository.ExisteConflitoDeHorarioAsync(barbeiroId, dto.NovoDataHoraInicio, novoDataHoraFim, agendamento.Id);
            if (possuiConflito)
                return ApiResposta<AgendamentoDto>.Falha("Ja existe outro agendamento para este barbeiro no novo horario selecionado.");

            agendamento.BarbeiroId = barbeiroId;
            agendamento.DataHoraInicio = dto.NovoDataHoraInicio;
            agendamento.DataHoraFim = novoDataHoraFim;
            agendamento.Status = StatusAgendamento.Confirmado;

            await _agendamentoRepository.AtualizarAsync(agendamento);
            return await ObterPorIdAsync(agendamento.Id);
        }
        catch (Exception ex)
        {
            return ApiResposta<AgendamentoDto>.Falha($"Erro ao reagendar agendamento com ID {agendamentoId}: {ex.Message}");
        }
    }

    private static List<DateTime> GerarSlotsTeoricos(DateTime data, int duracaoTotalMinutos)
    {
        var inicioExpediente = data.Date.AddHours(8);
        var fimExpediente = data.Date.AddHours(19);
        var slots = new List<DateTime>();

        for (var horario = inicioExpediente; horario.AddMinutes(duracaoTotalMinutos) <= fimExpediente; horario = horario.AddMinutes(45))
        {
            slots.Add(horario);
        }

        return slots;
    }

    public async Task<ApiResposta<IEnumerable<DateTime>>> ListarHorariosDisponiveisAsync(string barbeiroId, DateTime data, IEnumerable<int> servicoIds)
    {
        try
        {
            // Comparar sempre contra horário local BRT para não rejeitar
            // o dia atual quando o servidor operar em UTC (ex.: UTC-3 = dia seguinte após 21h).
            var agoraBrt = AgoraBrt();
            if (data.Date < agoraBrt.Date)
                return ApiResposta<IEnumerable<DateTime>>.Ok(Enumerable.Empty<DateTime>());

            var barbeiro = await _usuarioRepository.ObterPorIdAsync(barbeiroId);
            if (barbeiro == null)
                return ApiResposta<IEnumerable<DateTime>>.Falha($"Barbeiro com ID '{barbeiroId}' nao encontrado.");

            var servicos = (await _servicoRepository.ObterPorIdsAsync(servicoIds))
                .Where(s => s.Ativo)
                .ToList();

            var duracaoTotalMinutos = servicos.Any() ? servicos.Sum(s => s.DuracaoMinutos) : 45;

            var inicioDia = data.Date;
            var fimDia = data.Date.AddDays(1);

            var agendamentosExistentes = (await _agendamentoRepository.ObterPorBarbeiroEPeriodoAsync(barbeiroId, inicioDia, fimDia))
                .Where(a => a.Status != StatusAgendamento.Cancelado && a.Status != StatusAgendamento.NaoCompareceu)
                .ToList();

            var bloqueios = (await _usuarioRepository.ObterBloqueiosPorPeriodoAsync(barbeiroId, inicioDia, fimDia))
                .ToList();

            var slotsTeoricos = GerarSlotsTeoricos(data, duracaoTotalMinutos);
            var horariosDisponiveis = new List<DateTime>();

            foreach (var horario in slotsTeoricos)
            {
                if (horario <= agoraBrt)
                    continue;

                var terminoEstimado = horario.AddMinutes(duracaoTotalMinutos);
                var temConflito = agendamentosExistentes.Any(a => a.DataHoraInicio < terminoEstimado && a.DataHoraFim > horario);
                var temBloqueio = bloqueios.Any(b => b.DataHoraInicio < terminoEstimado && b.DataHoraFim > horario);

                if (!temConflito && !temBloqueio)
                    horariosDisponiveis.Add(horario);
            }

            return ApiResposta<IEnumerable<DateTime>>.Ok(horariosDisponiveis);
        }
        catch (Exception ex)
        {
            return ApiResposta<IEnumerable<DateTime>>.Falha($"Erro ao listar horarios disponiveis para o barbeiro '{barbeiroId}': {ex.Message}");
        }
    }

    public async Task<ApiResposta<IEnumerable<HorarioSlotDto>>> ListarTodosHorariosDoDiaAsync(string barbeiroId, DateTime data, IEnumerable<int> servicoIds)
    {
        try
        {
            var barbeiro = await _usuarioRepository.ObterPorIdAsync(barbeiroId);
            if (barbeiro == null)
                return ApiResposta<IEnumerable<HorarioSlotDto>>.Falha($"Barbeiro com ID '{barbeiroId}' nao encontrado.");

            var servicos = (await _servicoRepository.ObterPorIdsAsync(servicoIds))
                .Where(s => s.Ativo)
                .ToList();

            var duracaoTotalMinutos = servicos.Any() ? servicos.Sum(s => s.DuracaoMinutos) : 45;

            var inicioDia = data.Date;
            var fimDia = data.Date.AddDays(1);

            var agendamentosExistentes = (await _agendamentoRepository.ObterPorBarbeiroEPeriodoAsync(barbeiroId, inicioDia, fimDia))
                .Where(a => a.Status != StatusAgendamento.Cancelado && a.Status != StatusAgendamento.NaoCompareceu)
                .ToList();

            var bloqueios = (await _usuarioRepository.ObterBloqueiosPorPeriodoAsync(barbeiroId, inicioDia, fimDia))
                .ToList();

            var slotsTeoricos = GerarSlotsTeoricos(data, duracaoTotalMinutos);
            var todosHorarios = new List<HorarioSlotDto>();
            var agoraBrt = AgoraBrt(); // hora local BRT — fonte única de verdade

            foreach (var horario in slotsTeoricos)
            {
                // Se a data do agendamento for anterior a hoje, ou se for hoje e o horário já passou
                if (horario.Date < agoraBrt.Date || horario <= agoraBrt)
                {
                    todosHorarios.Add(new HorarioSlotDto { Horario = horario, Disponivel = false });
                    continue;
                }

                var terminoEstimado = horario.AddMinutes(duracaoTotalMinutos);
                var estrapolaExpediente = terminoEstimado > fimDia.Date.AddHours(19);
                var temConflito = agendamentosExistentes.Any(a => a.DataHoraInicio < terminoEstimado && a.DataHoraFim > horario);
                var temBloqueio = bloqueios.Any(b => b.DataHoraInicio < terminoEstimado && b.DataHoraFim > horario);

                todosHorarios.Add(new HorarioSlotDto
                {
                    Horario = horario,
                    Disponivel = !estrapolaExpediente && !temConflito && !temBloqueio
                });
            }

            return ApiResposta<IEnumerable<HorarioSlotDto>>.Ok(todosHorarios);
        }
        catch (Exception ex)
        {
            return ApiResposta<IEnumerable<HorarioSlotDto>>.Falha($"Erro ao listar horarios do dia para o barbeiro '{barbeiroId}': {ex.Message}");
        }
    }
}
