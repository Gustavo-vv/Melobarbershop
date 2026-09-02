namespace Barbearia.Application.Interfaces.Services;

using Barbearia.Domain.Enums;
using Melobarbershop.Domain.Entidades;

public interface IProdutoService
{
    Task<Produto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Produto?> ObterPorCodigoBarrasAsync(string codigoBarras, CancellationToken cancellationToken = default);
    Task<IEnumerable<Produto>> ListarAtivosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Produto>> ListarTodosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Produto>> ListarComEstoqueAbaixoDoMinimoAsync(CancellationToken cancellationToken = default);
    Task<Produto> CriarAsync(string codigoBarras, string nome, decimal precoCusto, decimal precoVenda, int estoqueInicial, int estoqueMinimoAlerta, CancellationToken cancellationToken = default);
    Task<Produto> AtualizarAsync(int id, string codigoBarras, string nome, decimal precoCusto, decimal precoVenda, int estoqueMinimoAlerta, CancellationToken cancellationToken = default);
    Task AdicionarEstoqueAsync(int produtoId, int quantidade, string? observacao = null, CancellationToken cancellationToken = default);
    Task RemoverEstoqueAsync(int produtoId, int quantidade, TipoMovimentacaoEstoque tipo, string? observacao = null, CancellationToken cancellationToken = default);
    Task<bool> PossuiEstoqueAsync(int produtoId, int quantidade, CancellationToken cancellationToken = default);
    Task DesativarAsync(int id, CancellationToken cancellationToken = default);
    Task AtivarAsync(int id, CancellationToken cancellationToken = default);
}