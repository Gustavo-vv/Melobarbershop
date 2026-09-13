// Arquivo: Melobarbershop.Domain/Interfaces/Repositories/IProdutoRepository.cs
// Namespace: Melobarbershop.Domain.Interfaces.Repositories
// Conteúdo: interface IProdutoRepository
// Resumo: Contrato de repositório para operações CRUD e consultas sobre produtos e estoque.
namespace Melobarbershop.Domain.Interfaces.Repositories;

using Melobarbershop.Domain.Entidades;

public interface IProdutoRepository
{
    Task<Produto?> ObterPorIdAsync(int id);
    Task<Produto?> ObterPorCodigoBarrasAsync(string codigoBarras);
    Task<IEnumerable<Produto>> ObterTodosAsync();
    Task<IEnumerable<Produto>> ObterAtivosAsync();
    Task<IEnumerable<Produto>> ObterComEstoqueAbaixoDoMinimoAsync();
    Task AdicionarAsync(Produto produto);
    Task AtualizarAsync(Produto produto);
    Task AdicionarMovimentacaoEstoqueAsync(MovimentacaoEstoque movimentacao);
    Task<IEnumerable<MovimentacaoEstoque>> ObterMovimentacoesPorProdutoAsync(int produtoId, DateTime? inicio = null, DateTime? fim = null);
    Task RemoverAsync(Produto produto);
}