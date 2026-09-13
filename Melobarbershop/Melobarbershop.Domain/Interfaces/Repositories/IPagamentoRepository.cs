using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Domain.Interfaces.Repositories;

public interface IPagamentoRepository
{
    Task<Pagamento?> ObterPorIdAsync(int id);
    Task<IEnumerable<Pagamento>> ObterPorVendaIdAsync(int vendaId);
    Task<IEnumerable<Pagamento>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim);
    Task<IEnumerable<Pagamento>> ObterPorFormaPagamentoAsync(FormaPagamento formaPagamento, DateTime inicio, DateTime fim);
    Task<decimal> ObterTotalRecebidoPorPeriodoAsync(DateTime inicio, DateTime fim, FormaPagamento? forma = null);
    Task AdicionarAsync(Pagamento pagamento);
    Task RemoverAsync(Pagamento pagamento);
}