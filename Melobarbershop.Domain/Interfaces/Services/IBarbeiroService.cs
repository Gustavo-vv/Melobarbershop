namespace Barbearia.Application.Interfaces.Services;

using Melobarbershop.Domain.Entidades;

public interface IBarbeiroService
{
    Task<Barbeiro?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Barbeiro>> ListarAtivosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Barbeiro>> ListarTodosAsync(CancellationToken cancellationToken = default);
    Task<Barbeiro> CriarAsync(string nome, string telefone, decimal percentualComissaoPadrao, string? fotoUrl = null, string? usuarioId = null, CancellationToken cancellationToken = default);
    Task<Barbeiro> AtualizarAsync(int id, string nome, string telefone, decimal percentualComissaoPadrao, string? fotoUrl = null, CancellationToken cancellationToken = default);
    Task DesativarAsync(int id, CancellationToken cancellationToken = default);
    Task AtivarAsync(int id, CancellationToken cancellationToken = default);
    Task<BloqueioAgenda> AdicionarBloqueioAgendaAsync(int barbeiroId, DateTime inicio, DateTime fim, string motivo, CancellationToken cancellationToken = default);
    Task RemoverBloqueioAgendaAsync(int bloqueioId, CancellationToken cancellationToken = default);
    Task<bool> VerificarDisponibilidadeAsync(int barbeiroId, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
}