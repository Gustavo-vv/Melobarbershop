namespace Melobarbershop.Domain.Interfaces.Repositories;

using Melobarbershop.Domain.Entidades;

public interface IServicoRepository
{
    Task<Servico?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Servico>> ObterPorIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
    Task<IEnumerable<Servico>> ObterTodosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Servico>> ObterAtivosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Servico>> ObterExibidosNoSiteAsync(CancellationToken cancellationToken = default);
    Task AdicionarAsync(Servico servico, CancellationToken cancellationToken = default);
    Task AtualizarAsync(Servico servico, CancellationToken cancellationToken = default);
    Task RemoverAsync(Servico servico, CancellationToken cancellationToken = default);
}