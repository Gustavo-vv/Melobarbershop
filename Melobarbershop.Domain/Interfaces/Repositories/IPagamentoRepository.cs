using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Domain.Interfaces.Repositories;

public interface IPagamentoRepository
{
    Task<Pagamento?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Pagamento>> ObterPorVendaIdAsync(int vendaId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Pagamento>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
    Task<IEnumerable<Pagamento>> ObterPorFormaPagamentoAsync(FormaPagamento formaPagamento, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
    Task<decimal> ObterTotalRecebidoPorPeriodoAsync(DateTime inicio, DateTime fim, FormaPagamento? forma = null, CancellationToken cancellationToken = default);
    Task AdicionarAsync(Pagamento pagamento, CancellationToken cancellationToken = default);
    Task RemoverAsync(Pagamento pagamento, CancellationToken cancellationToken = default);
}