namespace Barbearia.Application.Interfaces.Services;

using Barbearia.Domain.Enums;
using Melobarbershop.Domain.Entidades;

public interface IPagamentoService
{
    Task<Pagamento?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Pagamento>> ListarPorVendaIdAsync(int vendaId, CancellationToken cancellationToken = default);
    Task<Pagamento> ProcessarPagamentoAsync(int vendaId, FormaPagamento forma, decimal valor, CancellationToken cancellationToken = default);
    Task EstornarPagamentoAsync(int pagamentoId, string motivo, CancellationToken cancellationToken = default);
    Task<decimal> ConsultarTotalRecebidoPorPeriodoAsync(DateTime inicio, DateTime fim, FormaPagamento? forma = null, CancellationToken cancellationToken = default);
}