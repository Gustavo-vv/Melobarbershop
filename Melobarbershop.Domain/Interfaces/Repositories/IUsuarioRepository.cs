using Melobarbershop.Domain.Entidades;

namespace Melobarbershop.Domain.Interfaces.Repositories;

public interface IUsuarioRepository
{
    Task<ApplicationUser?> ObterPorIdAsync(string id, CancellationToken cancellationToken = default);
    Task<ApplicationUser?> ObterPorTelefoneAsync(string telefone, CancellationToken cancellationToken = default);
    Task<ApplicationUser?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApplicationUser>> ObterPorRoleAsync(string roleName, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApplicationUser>> ObterAtivosPorRoleAsync(string roleName, CancellationToken cancellationToken = default);
    Task<bool> ExisteTelefoneAsync(string telefone, string? usuarioIdIgnorar = null, CancellationToken cancellationToken = default);
    Task<bool> ExisteEmailAsync(string email, string? usuarioIdIgnorar = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<BloqueioAgenda>> ObterBloqueiosPorPeriodoAsync(string barbeiroId, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
    Task<bool> ExisteBloqueioNoPeriodoAsync(string barbeiroId, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
    Task<BloqueioAgenda?> ObterBloqueioPorIdAsync(int bloqueioId, CancellationToken cancellationToken = default);
    Task AdicionarBloqueioAsync(BloqueioAgenda bloqueio, CancellationToken cancellationToken = default);
    Task RemoverBloqueioAsync(BloqueioAgenda bloqueio, CancellationToken cancellationToken = default);
    Task AtualizarAsync(ApplicationUser usuario, CancellationToken cancellationToken = default);
}
