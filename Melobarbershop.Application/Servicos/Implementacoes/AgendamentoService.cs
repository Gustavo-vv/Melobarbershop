using AutoMapper;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Interfaces.Services;
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

    public async Task<AgendamentoDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var agendamento = await _agendamentoRepository.ObterPorIdCompletoAsync(id, cancellationToken);
        return agendamento == null ? null : _mapper.Map<AgendamentoDto>(agendamento);
    }

    public async Task<IEnumerable<AgendamentoDto>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim, string? barbeiroId = null, CancellationToken cancellationToken = default)
    {
        IEnumerable<Agendamento> agendamentos;

        if (!string.IsNullOrWhiteSpace(barbeiroId))
        {
            agendamentos = await _agendamentoRepository.ObterPorBarbeiroEPeriodoAsync(barbeiroId, inicio, fim, cancellationToken);
        }
        else
        {
            agendamentos = await _agendamentoRepository.ObterPorPeriodoAsync(inicio, fim, cancellationToken);
        }

        return _mapper.Map<IEnumerable<AgendamentoDto>>(agendamentos);
    }

    public async Task<IEnumerable<AgendamentoDto>> ListarPorClienteAsync(string clienteId, CancellationToken cancellationToken = default)
    {
        var agendamentos = await _agendamentoRepository.ObterPorClienteAsync(clienteId, cancellationToken);
        return _mapper.Map<IEnumerable<AgendamentoDto>>(agendamentos);
    }

    public async Task ConfirmarAsync(int agendamentoId, CancellationToken cancellationToken = default)
    {
        await ValidarEAtualizarStatusAsync(agendamentoId, StatusAgendamento.Confirmado, cancellationToken);
    }

    public async Task IniciarAtendimentoAsync(int agendamentoId, CancellationToken cancellationToken = default)
    {
        await ValidarEAtualizarStatusAsync(agendamentoId, StatusAgendamento.EmAtendimento, cancellationToken);
    }

    public async Task ConcluirAsync(int agendamentoId, CancellationToken cancellationToken = default)
    {
        await ValidarEAtualizarStatusAsync(agendamentoId, StatusAgendamento.Concluido, cancellationToken);
    }

    public async Task RegistrarNaoComparecimentoAsync(int agendamentoId, CancellationToken cancellationToken = default)
    {
        await ValidarEAtualizarStatusAsync(agendamentoId, StatusAgendamento.NaoCompareceu, cancellationToken);
    }

    public async Task CancelarAsync(int agendamentoId, string? motivo = null, CancellationToken cancellationToken = default)
    {
        var agendamento = await _agendamentoRepository.ObterPorIdAsync(agendamentoId, cancellationToken)
            ?? throw new KeyNotFoundException($"Agendamento com ID {agendamentoId} não encontrado.");

        agendamento.Status = StatusAgendamento.Cancelado;

        if (!string.IsNullOrWhiteSpace(motivo))
        {
            agendamento.Observacoes = string.IsNullOrWhiteSpace(agendamento.Observacoes)
                ? $"Cancelado: {motivo}"
                : $"{agendamento.Observacoes} | Cancelado: {motivo}";
        }

        await _agendamentoRepository.AtualizarAsync(agendamento, cancellationToken);
    }

    public async Task<AgendamentoDto> CriarAsync(CriarAgendamentoDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.ServicoIds == null || !dto.ServicoIds.Any())
            throw new ArgumentException("Pelo menos um serviço deve ser selecionado para o agendamento.", nameof(dto.ServicoIds));

        if (dto.DataHoraInicio < DateTime.UtcNow.AddMinutes(-5))
            throw new ArgumentException("A data e hora do agendamento não pode ser no passado.", nameof(dto.DataHoraInicio));

        var servicos = (await _servicoRepository.ObterPorIdsAsync(dto.ServicoIds, cancellationToken))
            .Where(s => s.Ativo)
            .ToList();

        if (!servicos.Any())
            throw new InvalidOperationException("Nenhum dos serviços selecionados está disponível ou ativo.");

        var duracaoTotalMinutos = servicos.Sum(s => s.DuracaoMinutos);
        var dataHoraFim = dto.DataHoraInicio.AddMinutes(duracaoTotalMinutos);

        // Validação 1: Bloqueios de agenda do barbeiro (folgas, almoço, ausências)
        var possuiBloqueio = await _usuarioRepository.ExisteBloqueioNoPeriodoAsync(dto.BarbeiroId, dto.DataHoraInicio, dataHoraFim, cancellationToken);
        if (possuiBloqueio)
            throw new InvalidOperationException("O barbeiro selecionado possui um bloqueio de agenda no horário solicitado.");

        // Validação 2: Conflito com outros agendamentos existentes
        var possuiConflito = await _agendamentoRepository.ExisteConflitoDeHorarioAsync(dto.BarbeiroId, dto.DataHoraInicio, dataHoraFim, null, cancellationToken);
        if (possuiConflito)
            throw new InvalidOperationException("Já existe outro agendamento para este barbeiro no horário solicitado.");

        var agendamento = new Agendamento
        {
            ClienteId = dto.ClienteId,
            BarbeiroId = dto.BarbeiroId,
            DataHoraInicio = dto.DataHoraInicio,
            DataHoraFim = dataHoraFim,
            Origem = dto.Origem,
            Observacoes = dto.Observacoes,
            Status = StatusAgendamento.Pendente,
            DataCriacao = DateTime.UtcNow,
            Itens = servicos.Select(s => new AgendamentoItem
            {
                ServicoId = s.Id,
                PrecoCobrado = s.Preco
            }).ToList()
        };

        await _agendamentoRepository.AdicionarAsync(agendamento, cancellationToken);

        return (await ObterPorIdAsync(agendamento.Id, cancellationToken))!;
    }

    public async Task<AgendamentoDto> ReagendarAsync(int agendamentoId, ReagendarAgendamentoDto dto, CancellationToken cancellationToken = default)
    {
        var agendamento = await _agendamentoRepository.ObterPorIdCompletoAsync(agendamentoId, cancellationToken)
            ?? throw new KeyNotFoundException($"Agendamento com ID {agendamentoId} não encontrado.");

        if (agendamento.Status == StatusAgendamento.Concluido || agendamento.Status == StatusAgendamento.Cancelado)
            throw new InvalidOperationException("Não é possível reagendar um atendimento que já foi concluído ou cancelado.");

        if (dto.NovoDataHoraInicio < DateTime.UtcNow.AddMinutes(-5))
            throw new ArgumentException("O novo horário não pode ser no passado.", nameof(dto.NovoDataHoraInicio));

        var barbeiroId = !string.IsNullOrWhiteSpace(dto.NovoBarbeiroId) ? dto.NovoBarbeiroId : agendamento.BarbeiroId;
        var duracaoOriginal = agendamento.DataHoraFim - agendamento.DataHoraInicio;
        var novoDataHoraFim = dto.NovoDataHoraInicio.Add(duracaoOriginal);

        var possuiBloqueio = await _usuarioRepository.ExisteBloqueioNoPeriodoAsync(barbeiroId, dto.NovoDataHoraInicio, novoDataHoraFim, cancellationToken);
        if (possuiBloqueio)
            throw new InvalidOperationException("O barbeiro possui um bloqueio de agenda no novo horário selecionado.");

        var possuiConflito = await _agendamentoRepository.ExisteConflitoDeHorarioAsync(barbeiroId, dto.NovoDataHoraInicio, novoDataHoraFim, agendamento.Id, cancellationToken);
        if (possuiConflito)
            throw new InvalidOperationException("Já existe outro agendamento para este barbeiro no novo horário selecionado.");

        agendamento.BarbeiroId = barbeiroId;
        agendamento.DataHoraInicio = dto.NovoDataHoraInicio;
        agendamento.DataHoraFim = novoDataHoraFim;
        agendamento.Status = StatusAgendamento.Pendente;

        await _agendamentoRepository.AtualizarAsync(agendamento, cancellationToken);

        return (await ObterPorIdAsync(agendamento.Id, cancellationToken))!;
    }

    public async Task<IEnumerable<DateTime>> ListarHorariosDisponiveisAsync(string barbeiroId, DateTime data, IEnumerable<int> servicoIds, CancellationToken cancellationToken = default)
    {
        var servicos = (await _servicoRepository.ObterPorIdsAsync(servicoIds, cancellationToken))
            .Where(s => s.Ativo)
            .ToList();

        var duracaoTotalMinutos = servicos.Any() ? servicos.Sum(s => s.DuracaoMinutos) : 30;

        // Horário de funcionamento padrão: 08:00 às 19:00 em intervalos de 30 minutos
        var inicioExpediente = data.Date.AddHours(8);
        var fimExpediente = data.Date.AddHours(19);

        var inicioDia = data.Date;
        var fimDia = data.Date.AddDays(1);

        var agendamentosExistentes = (await _agendamentoRepository.ObterPorBarbeiroEPeriodoAsync(barbeiroId, inicioDia, fimDia, cancellationToken))
            .Where(a => a.Status != StatusAgendamento.Cancelado && a.Status != StatusAgendamento.NaoCompareceu)
            .ToList();

        var bloqueios = (await _usuarioRepository.ObterBloqueiosPorPeriodoAsync(barbeiroId, inicioDia, fimDia, cancellationToken))
            .ToList();

        var horariosDisponiveis = new List<DateTime>();
        var agora = DateTime.UtcNow;

        for (var horario = inicioExpediente; horario.AddMinutes(duracaoTotalMinutos) <= fimExpediente; horario = horario.AddMinutes(30))
        {
            if (horario <= agora)
                continue;

            var terminoEstimado = horario.AddMinutes(duracaoTotalMinutos);

            var temConflitoAgendamento = agendamentosExistentes.Any(a => a.DataHoraInicio < terminoEstimado && a.DataHoraFim > horario);
            var temBloqueio = bloqueios.Any(b => b.DataHoraInicio < terminoEstimado && b.DataHoraFim > horario);

            if (!temConflitoAgendamento && !temBloqueio)
            {
                horariosDisponiveis.Add(horario);
            }
        }

        return horariosDisponiveis;
    }

    private async Task ValidarEAtualizarStatusAsync(int agendamentoId, StatusAgendamento novoStatus, CancellationToken cancellationToken)
    {
        var agendamento = await _agendamentoRepository.ObterPorIdAsync(agendamentoId, cancellationToken)
            ?? throw new KeyNotFoundException($"Agendamento com ID {agendamentoId} não encontrado.");

        await _agendamentoRepository.AtualizarStatusAsync(agendamentoId, novoStatus, cancellationToken);
    }
}
