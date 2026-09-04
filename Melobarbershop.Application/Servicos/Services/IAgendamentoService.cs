using Melobarbershop.Application.DTOs;

namespace Melobarbershop.Application.Servicos.Services;

public interface IAgendamentoService
{
    Task<AgendamentoDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AgendamentoDto>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim, string? barbeiroId = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<AgendamentoDto>> ListarPorClienteAsync(string clienteId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DateTime>> ListarHorariosDisponiveisAsync(string barbeiroId, DateTime data, IEnumerable<int> servicoIds, CancellationToken cancellationToken = default);
    Task<AgendamentoDto> CriarAsync(CriarAgendamentoDto dto, CancellationToken cancellationToken = default);
    Task ConfirmarAsync(int agendamentoId, CancellationToken cancellationToken = default);
    Task IniciarAtendimentoAsync(int agendamentoId, CancellationToken cancellationToken = default);
    Task ConcluirAsync(int agendamentoId, CancellationToken cancellationToken = default);
    Task CancelarAsync(int agendamentoId, string? motivo = null, CancellationToken cancellationToken = default);
    Task RegistrarNaoComparecimentoAsync(int agendamentoId, CancellationToken cancellationToken = default);
    Task<AgendamentoDto> ReagendarAsync(int agendamentoId, ReagendarAgendamentoDto dto, CancellationToken cancellationToken = default);
}
