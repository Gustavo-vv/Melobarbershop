namespace Melobarbershop.Domain.Interfaces.Repositories;

using Melobarbershop.Domain.Entidades;

public interface IVendaRepository
{
    Task<Venda?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Venda?> ObterPorIdCompletoAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Venda>> ObterPorClienteAsync(int clienteId, CancellationToken cancellationToken = default);
    Task<Venda?> ObterPorAgendamentoIdAsync(int agendamentoId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Venda>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
    Task<decimal> ObterTotalFaturadoPorPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
    Task AdicionarAsync(Venda venda, CancellationToken cancellationToken = default);
    Task AtualizarAsync(Venda venda, CancellationToken cancellationToken = default);
}