using Melobarbershop.Domain.Entidades;

namespace Melobarbershop.Domain.Interfaces.Repositories;

public interface IAvaliacaoRepository
{
    Task<Avaliacao?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Avaliacao?> ObterPorAgendamentoIdAsync(int agendamentoId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Avaliacao>> ObterPorBarbeiroAsync(string barbeiroId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Avaliacao>> ObterPorClienteAsync(string clienteId, CancellationToken cancellationToken = default);
    Task<double> CalcularMediaAvaliacoesBarbeiroAsync(string barbeiroId, CancellationToken cancellationToken = default);
    Task<bool> ExisteAvaliacaoParaAgendamentoAsync(int agendamentoId, CancellationToken cancellationToken = default);
    Task AdicionarAsync(Avaliacao avaliacao, CancellationToken cancellationToken = default);
}