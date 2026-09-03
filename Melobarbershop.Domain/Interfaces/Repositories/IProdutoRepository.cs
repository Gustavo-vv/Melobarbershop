namespace Melobarbershop.Domain.Interfaces.Repositories;

using Melobarbershop.Domain.Entidades;

public interface IProdutoRepository
{
    Task<Produto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Produto?> ObterPorCodigoBarrasAsync(string codigoBarras, CancellationToken cancellationToken = default);
    Task<IEnumerable<Produto>> ObterTodosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Produto>> ObterAtivosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Produto>> ObterComEstoqueAbaixoDoMinimoAsync(CancellationToken cancellationToken = default);
    Task AdicionarAsync(Produto produto, CancellationToken cancellationToken = default);
    Task AtualizarAsync(Produto produto, CancellationToken cancellationToken = default);
    Task AdicionarMovimentacaoEstoqueAsync(MovimentacaoEstoque movimentacao, CancellationToken cancellationToken = default);
    Task<IEnumerable<MovimentacaoEstoque>> ObterMovimentacoesPorProdutoAsync(int produtoId, DateTime? inicio = null, DateTime? fim = null, CancellationToken cancellationToken = default);
    Task RemoverAsync(Produto produto, CancellationToken cancellationToken = default);
}