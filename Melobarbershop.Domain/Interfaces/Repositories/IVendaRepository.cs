using Melobarbershop.Domain.Entidades;

namespace Melobarbershop.Domain.Interfaces.Repositories;

public interface IVendaRepository
{
    Task<Venda?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Venda?> ObterPorIdCompletoAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Venda>> ObterPorClienteAsync(string clienteId, CancellationToken cancellationToken = default);
    Task<Venda?> ObterPorAgendamentoIdAsync(int agendamentoId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Venda>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
    Task<decimal> ObterTotalFaturadoPorPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
    Task AdicionarAsync(Venda venda, CancellationToken cancellationToken = default);
    Task AtualizarAsync(Venda venda, CancellationToken cancellationToken = default);
}