using Melobarbershop.Application.DTOs;

namespace Melobarbershop.Application.Interfaces.Services;

public interface IAvaliacaoService
{
    Task<AvaliacaoDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<AvaliacaoDto?> ObterPorAgendamentoAsync(int agendamentoId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AvaliacaoDto>> ListarPorBarbeiroAsync(string barbeiroId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AvaliacaoDto>> ListarPorClienteAsync(string clienteId, CancellationToken cancellationToken = default);
    Task<ResumoAvaliacoesDto> ObterResumoAvaliacoesBarbeiroAsync(string barbeiroId, CancellationToken cancellationToken = default);
    Task<AvaliacaoDto> RegistrarAvaliacaoAsync(CriarAvaliacaoDto dto, CancellationToken cancellationToken = default);
}
