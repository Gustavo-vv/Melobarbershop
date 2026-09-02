namespace Melobarbershop.Domain.Interfaces.Repositories;

using Melobarbershop.Domain.Entidades;

public interface IBarbeiroRepository
{
    Task<Barbeiro?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Barbeiro?> ObterPorUsuarioIdAsync(string usuarioId, CancellationToken cancellationToken = default);
    Task<Barbeiro?> ObterComHorariosEBloqueiosAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Barbeiro>> ObterTodosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Barbeiro>> ObterAtivosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<BloqueioAgenda>> ObterBloqueiosPorPeriodoAsync(int barbeiroId, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
    Task<bool> ExisteBloqueioNoPeriodoAsync(int barbeiroId, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
    Task<BloqueioAgenda?> ObterBloqueioPorIdAsync(int bloqueioId, CancellationToken cancellationToken = default);
    Task AdicionarAsync(Barbeiro barbeiro, CancellationToken cancellationToken = default);
    Task AtualizarAsync(Barbeiro barbeiro, CancellationToken cancellationToken = default);
    Task AdicionarBloqueioAsync(BloqueioAgenda bloqueio, CancellationToken cancellationToken = default);
    Task RemoverBloqueioAsync(BloqueioAgenda bloqueio, CancellationToken cancellationToken = default);
}