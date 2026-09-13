using Melobarbershop.Domain.Entidades;

namespace Melobarbershop.Domain.Interfaces.Repositories;

public interface IAvaliacaoRepository
{
    Task<Avaliacao?> ObterPorIdAsync(int id);
    Task<Avaliacao?> ObterPorAgendamentoIdAsync(int agendamentoId);
    Task<IEnumerable<Avaliacao>> ObterPorBarbeiroAsync(string barbeiroId);
    Task<IEnumerable<Avaliacao>> ObterPorClienteAsync(string clienteId);
    Task<double> CalcularMediaAvaliacoesBarbeiroAsync(string barbeiroId);
    Task<bool> ExisteAvaliacaoParaAgendamentoAsync(int agendamentoId);
    Task AdicionarAsync(Avaliacao avaliacao);
}