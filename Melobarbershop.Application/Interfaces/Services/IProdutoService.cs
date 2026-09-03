using Melobarbershop.Application.DTOs;

namespace Melobarbershop.Application.Interfaces.Services;

public interface IProdutoService
{
    Task<ProdutoDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ProdutoDto?> ObterPorCodigoBarrasAsync(string codigoBarras, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProdutoDto>> ListarAtivosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ProdutoDto>> ListarTodosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ProdutoDto>> ListarComEstoqueAbaixoDoMinimoAsync(CancellationToken cancellationToken = default);
    Task<ProdutoDto> CriarAsync(CriarProdutoDto dto, CancellationToken cancellationToken = default);
    Task<ProdutoDto> AtualizarAsync(int id, AtualizarProdutoDto dto, CancellationToken cancellationToken = default);
    Task MovimentarEstoqueAsync(MovimentarEstoqueDto dto, CancellationToken cancellationToken = default);
    Task<IEnumerable<MovimentacaoEstoqueDto>> ListarMovimentacoesPorProdutoAsync(int produtoId, DateTime? inicio = null, DateTime? fim = null, CancellationToken cancellationToken = default);
    Task<bool> PossuiEstoqueAsync(int produtoId, int quantidade, CancellationToken cancellationToken = default);
    Task DesativarAsync(int id, CancellationToken cancellationToken = default);
    Task AtivarAsync(int id, CancellationToken cancellationToken = default);
    Task RemoverPermanentementeAsync(int id, CancellationToken cancellationToken = default);
}
