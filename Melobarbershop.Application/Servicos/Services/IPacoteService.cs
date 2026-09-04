using Melobarbershop.Application.DTOs;

namespace Melobarbershop.Application.Servicos.Services;

public interface IPacoteService
{
    Task<PacoteDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PacoteDto>> ListarAtivosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<PacoteDto>> ListarTodosAsync(CancellationToken cancellationToken = default);
    Task<PacoteDto> CriarAsync(CriarPacoteDto dto, CancellationToken cancellationToken = default);
    Task<PacoteDto> AtualizarAsync(int id, AtualizarPacoteDto dto, CancellationToken cancellationToken = default);
    Task DesativarAsync(int id, CancellationToken cancellationToken = default);
    Task AtivarAsync(int id, CancellationToken cancellationToken = default);
}
