using Melobarbershop.Application.DTOs;

namespace Melobarbershop.Application.Servicos.Services;

public interface IProdutoService
{
    Task<ApiResposta<ProdutoDto>> ObterPorIdAsync(int id);
    Task<ApiResposta<ProdutoDto>> ObterPorCodigoBarrasAsync(string codigoBarras);
    Task<ApiResposta<IEnumerable<ProdutoDto>>> ListarAtivosAsync();
    Task<ApiResposta<IEnumerable<ProdutoDto>>> ListarTodosAsync();
    Task<ApiResposta<IEnumerable<ProdutoDto>>> ListarComEstoqueAbaixoDoMinimoAsync();
    Task<ApiResposta<ProdutoDto>> CriarAsync(CriarProdutoDto dto);
    Task<ApiResposta<ProdutoDto>> AtualizarAsync(int id, AtualizarProdutoDto dto);
    Task<ApiResposta<ProdutoDto>> MovimentarEstoqueAsync(MovimentarEstoqueDto dto);
    Task<ApiResposta<IEnumerable<MovimentacaoEstoqueDto>>> ListarMovimentacoesPorProdutoAsync(int produtoId, DateTime? inicio = null, DateTime? fim = null);
    Task<ApiResposta<bool>> PossuiEstoqueAsync(int produtoId, int quantidade);
    Task<ApiResposta<ProdutoDto>> DesativarAsync(int id);
    Task<ApiResposta<ProdutoDto>> AtivarAsync(int id);
    Task<ApiResposta<ProdutoDto>> RemoverPermanentementeAsync(int id);
}
