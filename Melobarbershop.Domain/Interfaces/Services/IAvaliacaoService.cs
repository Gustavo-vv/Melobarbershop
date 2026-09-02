namespace Barbearia.Application.Interfaces.Services;

using Melobarbershop.Domain.Entidades;

public interface IAvaliacaoService
{
    Task<Avaliacao?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Avaliacao?> ObterPorAgendamentoAsync(int agendamentoId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Avaliacao>> ListarPorBarbeiroAsync(int barbeiroId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Avaliacao>> ListarPorClienteAsync(int clienteId, CancellationToken cancellationToken = default);
    Task<double> ObterMediaAvaliacoesBarbeiroAsync(int barbeiroId, CancellationToken cancellationToken = default);
    Task<Avaliacao> RegistrarAvaliacaoAsync(int agendamentoId, int notaEstrelas, string? comentario = null, CancellationToken cancellationToken = default);
}