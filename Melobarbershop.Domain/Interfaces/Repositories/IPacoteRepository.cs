namespace Melobarbershop.Domain.Interfaces.Repositories;

using Melobarbershop.Domain.Entidades;

public interface IPacoteRepository
{
    Task<Pacote?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Pacote?> ObterPorIdComItensAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Pacote>> ObterTodosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Pacote>> ObterAtivosAsync(CancellationToken cancellationToken = default);
    Task AdicionarAsync(Pacote pacote, CancellationToken cancellationToken = default);
    Task AtualizarAsync(Pacote pacote, CancellationToken cancellationToken = default);
    Task RemoverAsync(Pacote pacote, CancellationToken cancellationToken = default);
}