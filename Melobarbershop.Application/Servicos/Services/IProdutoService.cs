using Melobarbershop.Application.DTOs;

namespace Melobarbershop.Application.Servicos.Services;

public interface IProdutoService
{
    Task<ProdutoDto?> ObterPorIdAsync(int id);
    Task<ProdutoDto?> ObterPorCodigoBarrasAsync(string codigoBarras);
    Task<IEnumerable<ProdutoDto>> ListarAtivosAsync();
    Task<IEnumerable<ProdutoDto>> ListarTodosAsync();
    Task<IEnumerable<ProdutoDto>> ListarComEstoqueAbaixoDoMinimoAsync();
    Task<ProdutoDto> CriarAsync(CriarProdutoDto dto);
    Task<ProdutoDto> AtualizarAsync(int id, AtualizarProdutoDto dto);
    Task MovimentarEstoqueAsync(MovimentarEstoqueDto dto);
    Task<IEnumerable<MovimentacaoEstoqueDto>> ListarMovimentacoesPorProdutoAsync(int produtoId, DateTime? inicio = null, DateTime? fim = null);
    Task<bool> PossuiEstoqueAsync(int produtoId, int quantidade);
    Task DesativarAsync(int id);
    Task AtivarAsync(int id);
    Task RemoverPermanentementeAsync(int id);
}
