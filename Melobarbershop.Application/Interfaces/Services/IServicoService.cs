using Melobarbershop.Application.DTOs;

namespace Melobarbershop.Application.Interfaces.Services;

public interface IServicoService
{
    Task<ServicoDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ServicoDto>> ListarAtivosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ServicoDto>> ListarExibidosNoSiteAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ServicoDto>> ListarTodosAsync(CancellationToken cancellationToken = default);
    Task<ServicoDto> CriarAsync(CriarServicoDto dto, CancellationToken cancellationToken = default);
    Task<ServicoDto> AtualizarAsync(int id, AtualizarServicoDto dto, CancellationToken cancellationToken = default);
    Task DesativarAsync(int id, CancellationToken cancellationToken = default);
    Task AtivarAsync(int id, CancellationToken cancellationToken = default);
}
