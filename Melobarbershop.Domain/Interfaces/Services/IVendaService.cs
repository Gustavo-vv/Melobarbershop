namespace Barbearia.Application.Interfaces.Services;

using Barbearia.Domain.Enums;
using Melobarbershop.Domain.Entidades;

public interface IVendaService
{
    Task<Venda?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Venda?> ObterDetalhesCompletosAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Venda>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
    Task<IEnumerable<Venda>> ListarPorClienteAsync(int clienteId, CancellationToken cancellationToken = default);
    Task<Venda> IniciarVendaAsync(int? clienteId = null, int? agendamentoId = null, CancellationToken cancellationToken = default);
    Task<VendaItem> AdicionarItemServicoAsync(int vendaId, int servicoId, int? barbeiroId = null, decimal? precoCustomizado = null, CancellationToken cancellationToken = default);
    Task<VendaItem> AdicionarItemProdutoAsync(int vendaId, int produtoId, int quantidade, int? barbeiroId = null, decimal? precoCustomizado = null, CancellationToken cancellationToken = default);
    Task RemoverItemAsync(int vendaId, int vendaItemId, CancellationToken cancellationToken = default);
    Task AplicarDescontoAsync(int vendaId, decimal valorDesconto, CancellationToken cancellationToken = default);
    Task<Pagamento> AdicionarPagamentoAsync(int vendaId, FormaPagamento forma, decimal valor, CancellationToken cancellationToken = default);
    Task RemoverPagamentoAsync(int vendaId, int pagamentoId, CancellationToken cancellationToken = default);
    Task<Venda> FinalizarVendaAsync(int vendaId, CancellationToken cancellationToken = default);
    Task CancelarVendaAsync(int vendaId, string motivo, CancellationToken cancellationToken = default);
}