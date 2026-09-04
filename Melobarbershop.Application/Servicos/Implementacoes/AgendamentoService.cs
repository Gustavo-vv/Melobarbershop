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
        try
        {
            var agendamento = await _agendamentoRepository.ObterPorIdCompletoAsync(id, cancellationToken);
            return agendamento == null ? null : _mapper.Map<AgendamentoDto>(agendamento);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao obter agendamento com ID {id}.", ex);
        }
    }

    public async Task<IEnumerable<AgendamentoDto>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim, string? barbeiroId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            IEnumerable<Agendamento> agendamentos;

            if (!string.IsNullOrWhiteSpace(barbeiroId))
                agendamentos = await _agendamentoRepository.ObterPorBarbeiroEPeriodoAsync(barbeiroId, inicio, fim, cancellationToken);
            else
                agendamentos = await _agendamentoRepository.ObterPorPeriodoAsync(inicio, fim, cancellationToken);

            return _mapper.Map<IEnumerable<AgendamentoDto>>(agendamentos);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Erro ao listar agendamentos por periodo.", ex);
        }
    }

    public async Task<IEnumerable<AgendamentoDto>> ListarPorClienteAsync(string clienteId, CancellationToken cancellationToken = default)
    {
        try
        {
            var agendamentos = await _agendamentoRepository.ObterPorClienteAsync(clienteId, cancellationToken);
            return _mapper.Map<IEnumerable<AgendamentoDto>>(agendamentos);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao listar agendamentos do cliente '{clienteId}'.", ex);
        }
    }

    public async Task ConfirmarAsync(int agendamentoId, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidarEAtualizarStatusAsync(agendamentoId, StatusAgendamento.Confirmado, cancellationToken);
        }
        catch (KeyNotFoundException) { throw; }
        catch (InvalidOperationException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao confirmar agendamento com ID {agendamentoId}.", ex);
        }
    }

    public async Task IniciarAtendimentoAsync(int agendamentoId, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidarEAtualizarStatusAsync(agendamentoId, StatusAgendamento.EmAtendimento, cancellationToken);
        }
        catch (KeyNotFoundException) { throw; }
        catch (InvalidOperationException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao iniciar atendimento do agendamento com ID {agendamentoId}.", ex);
        }
    }

    public async Task ConcluirAsync(int agendamentoId, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidarEAtualizarStatusAsync(agendamentoId, StatusAgendamento.Concluido, cancellationToken);
        }
        catch (KeyNotFoundException) { throw; }
        catch (InvalidOperationException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao concluir agendamento com ID {agendamentoId}.", ex);
        }
    }

    public async Task RegistrarNaoComparecimentoAsync(int agendamentoId, CancellationToken cancellationToken = default)
    {
        try
        {
            await ValidarEAtualizarStatusAsync(agendamentoId, StatusAgendamento.NaoCompareceu, cancellationToken);
        }
        catch (KeyNotFoundException) { throw; }
        catch (InvalidOperationException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao registrar nao comparecimento do agendamento com ID {agendamentoId}.", ex);
        }
    }

    public async Task CancelarAsync(int agendamentoId, string? motivo = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var agendamento = await _agendamentoRepository.ObterPorIdAsync(agendamentoId, cancellationToken);
            if (agendamento == null)
                throw new KeyNotFoundException($"Agendamento com ID {agendamentoId} nao encontrado.");

            if (agendamento.Status == StatusAgendamento.Concluido)
                throw new InvalidOperationException("Nao e possivel cancelar um agendamento que ja foi concluido.");

            if (agendamento.Status == StatusAgendamento.Cancelado)
                throw new InvalidOperationException("Este agendamento ja se encontra cancelado.");

            agendamento.Status = StatusAgendamento.Cancelado;

            if (!string.IsNullOrWhiteSpace(motivo))
            {
                agendamento.Observacoes = string.IsNullOrWhiteSpace(agendamento.Observacoes)
                    ? $"Cancelado: {motivo}"
                    : $"{agendamento.Observacoes} | Cancelado: {motivo}";
            }

            await _agendamentoRepository.AtualizarAsync(agendamento, cancellationToken);
        }
        catch (KeyNotFoundException) { throw; }
        catch (InvalidOperationException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao cancelar agendamento com ID {agendamentoId}.", ex);
        }
    }

    public async Task<AgendamentoDto> CriarAsync(CriarAgendamentoDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            if (dto.ServicoIds == null || !dto.ServicoIds.Any())
                throw new ArgumentException("Pelo menos um servico deve ser selecionado para o agendamento.", nameof(dto.ServicoIds));

            if (dto.DataHoraInicio < DateTime.UtcNow.AddMinutes(-5))
                throw new ArgumentException("A data e hora do agendamento nao pode ser no passado.", nameof(dto.DataHoraInicio));

            var cliente = await _usuarioRepository.ObterPorIdAsync(dto.ClienteId, cancellationToken);
            if (cliente == null)
                throw new KeyNotFoundException($"Cliente com ID '{dto.ClienteId}' nao encontrado.");
            if (!cliente.Ativo)
                throw new InvalidOperationException("O cliente informado esta desativado no sistema.");

            var barbeiro = await _usuarioRepository.ObterPorIdAsync(dto.BarbeiroId, cancellationToken);
            if (barbeiro == null)
                throw new KeyNotFoundException($"Barbeiro com ID '{dto.BarbeiroId}' nao encontrado.");
            if (!barbeiro.Ativo)
                throw new InvalidOperationException("O barbeiro informado esta desativado no sistema.");

            var servicos = (await _servicoRepository.ObterPorIdsAsync(dto.ServicoIds, cancellationToken))
                .Where(s => s.Ativo)
                .ToList();

            if (servicos.Count != dto.ServicoIds.Distinct().Count())
                throw new InvalidOperationException("Um ou mais servicos selecionados nao foram encontrados ou estao inativos.");

            var duracaoTotalMinutos = servicos.Sum(s => s.DuracaoMinutos);
            var dataHoraFim = dto.DataHoraInicio.AddMinutes(duracaoTotalMinutos);

            var possuiBloqueio = await _usuarioRepository.ExisteBloqueioNoPeriodoAsync(dto.BarbeiroId, dto.DataHoraInicio, dataHoraFim, cancellationToken);
            if (possuiBloqueio)
                throw new InvalidOperationException("O barbeiro selecionado possui um bloqueio de agenda no horario solicitado.");

            var possuiConflito = await _agendamentoRepository.ExisteConflitoDeHorarioAsync(dto.BarbeiroId, dto.DataHoraInicio, dataHoraFim, null, cancellationToken);
            if (possuiConflito)
                throw new InvalidOperationException("Ja existe outro agendamento para este barbeiro no horario solicitado.");

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
        catch (KeyNotFoundException) { throw; }
        catch (ArgumentException) { throw; }
        catch (InvalidOperationException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Erro ao criar agendamento.", ex);
        }
    }

    public async Task<AgendamentoDto> ReagendarAsync(int agendamentoId, ReagendarAgendamentoDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var agendamento = await _agendamentoRepository.ObterPorIdCompletoAsync(agendamentoId, cancellationToken);
            if (agendamento == null)
                throw new KeyNotFoundException($"Agendamento com ID {agendamentoId} nao encontrado.");

            if (agendamento.Status == StatusAgendamento.Concluido || agendamento.Status == StatusAgendamento.Cancelado)
                throw new InvalidOperationException("Nao e possivel reagendar um atendimento que ja foi concluido ou cancelado.");

            if (dto.NovoDataHoraInicio < DateTime.UtcNow.AddMinutes(-5))
                throw new ArgumentException("O novo horario nao pode ser no passado.", nameof(dto.NovoDataHoraInicio));

            var barbeiroId = !string.IsNullOrWhiteSpace(dto.NovoBarbeiroId) ? dto.NovoBarbeiroId : agendamento.BarbeiroId;

            var barbeiro = await _usuarioRepository.ObterPorIdAsync(barbeiroId, cancellationToken);
            if (barbeiro == null)
                throw new KeyNotFoundException($"Barbeiro com ID '{barbeiroId}' nao encontrado.");
            if (!barbeiro.Ativo)
                throw new InvalidOperationException("O barbeiro selecionado esta desativado no sistema.");

            var duracaoOriginal = agendamento.DataHoraFim - agendamento.DataHoraInicio;
            var novoDataHoraFim = dto.NovoDataHoraInicio.Add(duracaoOriginal);

            var possuiBloqueio = await _usuarioRepository.ExisteBloqueioNoPeriodoAsync(barbeiroId, dto.NovoDataHoraInicio, novoDataHoraFim, cancellationToken);
            if (possuiBloqueio)
                throw new InvalidOperationException("O barbeiro possui um bloqueio de agenda no novo horario selecionado.");

            var possuiConflito = await _agendamentoRepository.ExisteConflitoDeHorarioAsync(barbeiroId, dto.NovoDataHoraInicio, novoDataHoraFim, agendamento.Id, cancellationToken);
            if (possuiConflito)
                throw new InvalidOperationException("Ja existe outro agendamento para este barbeiro no novo horario selecionado.");

            agendamento.BarbeiroId = barbeiroId;
            agendamento.DataHoraInicio = dto.NovoDataHoraInicio;
            agendamento.DataHoraFim = novoDataHoraFim;
            agendamento.Status = StatusAgendamento.Pendente;

            await _agendamentoRepository.AtualizarAsync(agendamento, cancellationToken);
            return (await ObterPorIdAsync(agendamento.Id, cancellationToken))!;
        }
        catch (KeyNotFoundException) { throw; }
        catch (InvalidOperationException) { throw; }
        catch (ArgumentException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao reagendar agendamento com ID {agendamentoId}.", ex);
        }
    }

    public async Task<IEnumerable<DateTime>> ListarHorariosDisponiveisAsync(string barbeiroId, DateTime data, IEnumerable<int> servicoIds, CancellationToken cancellationToken = default)
    {
        try
        {
            if (data.Date < DateTime.UtcNow.Date)
                return Enumerable.Empty<DateTime>();

            var barbeiro = await _usuarioRepository.ObterPorIdAsync(barbeiroId, cancellationToken);
            if (barbeiro == null)
                throw new KeyNotFoundException($"Barbeiro com ID '{barbeiroId}' nao encontrado.");

            var servicos = (await _servicoRepository.ObterPorIdsAsync(servicoIds, cancellationToken))
                .Where(s => s.Ativo)
                .ToList();

            var duracaoTotalMinutos = servicos.Any() ? servicos.Sum(s => s.DuracaoMinutos) : 30;

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
                var temConflito = agendamentosExistentes.Any(a => a.DataHoraInicio < terminoEstimado && a.DataHoraFim > horario);
                var temBloqueio = bloqueios.Any(b => b.DataHoraInicio < terminoEstimado && b.DataHoraFim > horario);

                if (!temConflito && !temBloqueio)
                    horariosDisponiveis.Add(horario);
            }

            return horariosDisponiveis;
        }
        catch (KeyNotFoundException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao listar horarios disponiveis para o barbeiro '{barbeiroId}'.", ex);
        }
    }

    private async Task ValidarEAtualizarStatusAsync(int agendamentoId, StatusAgendamento novoStatus, CancellationToken cancellationToken)
    {
        try
        {
            var agendamento = await _agendamentoRepository.ObterPorIdAsync(agendamentoId, cancellationToken);
            if (agendamento == null)
                throw new KeyNotFoundException($"Agendamento com ID {agendamentoId} nao encontrado.");

            if (agendamento.Status == StatusAgendamento.Cancelado)
                throw new InvalidOperationException("Nao e possivel alterar o status de um agendamento cancelado.");

            if (agendamento.Status == StatusAgendamento.Concluido)
                throw new InvalidOperationException("Nao e possivel alterar o status de um agendamento ja concluido.");

            await _agendamentoRepository.AtualizarStatusAsync(agendamentoId, novoStatus, cancellationToken);
        }
        catch (KeyNotFoundException) { throw; }
        catch (InvalidOperationException) { throw; }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao atualizar status do agendamento com ID {agendamentoId}.", ex);
        }
    }
}
