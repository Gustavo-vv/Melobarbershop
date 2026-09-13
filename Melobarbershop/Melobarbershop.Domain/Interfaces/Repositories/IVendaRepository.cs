using Melobarbershop.Domain.Entidades;

namespace Melobarbershop.Domain.Interfaces.Repositories;

public interface IVendaRepository
{
    Task<Venda?> ObterPorIdAsync(int id);
    Task<Venda?> ObterPorIdCompletoAsync(int id);
    Task<IEnumerable<Venda>> ObterPorClienteAsync(string clienteId);
    Task<Venda?> ObterPorAgendamentoIdAsync(int agendamentoId);
    Task<IEnumerable<Venda>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim);
    Task<decimal> ObterTotalFaturadoPorPeriodoAsync(DateTime inicio, DateTime fim);
    Task AdicionarAsync(Venda venda);
    Task AtualizarAsync(Venda venda);
}