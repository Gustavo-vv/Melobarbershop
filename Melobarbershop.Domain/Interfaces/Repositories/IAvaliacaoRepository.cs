namespace Melobarbershop.Domain.Interfaces.Repositories;

using Melobarbershop.Domain.Entidades;

public interface IAvaliacaoRepository
{
    Task<Avaliacao?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Avaliacao?> ObterPorAgendamentoIdAsync(int agendamentoId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Avaliacao>> ObterPorBarbeiroAsync(int barbeiroId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Avaliacao>> ObterPorClienteAsync(int clienteId, CancellationToken cancellationToken = default);
    Task<double> CalcularMediaAvaliacoesBarbeiroAsync(int barbeiroId, CancellationToken cancellationToken = default);
    Task<bool> ExisteAvaliacaoParaAgendamentoAsync(int agendamentoId, CancellationToken cancellationToken = default);
    Task AdicionarAsync(Avaliacao avaliacao, CancellationToken cancellationToken = default);
}