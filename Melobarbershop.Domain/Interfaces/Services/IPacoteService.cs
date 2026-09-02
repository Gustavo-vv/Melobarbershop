namespace Barbearia.Application.Interfaces.Services;

using Melobarbershop.Domain.Entidades;

public interface IPacoteService
{
    Task<Pacote?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Pacote>> ListarAtivosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Pacote>> ListarTodosAsync(CancellationToken cancellationToken = default);
    Task<Pacote> CriarAsync(string nome, decimal precoTotal, IEnumerable<int> servicoIds, CancellationToken cancellationToken = default);
    Task<Pacote> AtualizarAsync(int id, string nome, decimal precoTotal, IEnumerable<int> servicoIds, CancellationToken cancellationToken = default);
    Task DesativarAsync(int id, CancellationToken cancellationToken = default);
    Task AtivarAsync(int id, CancellationToken cancellationToken = default);
}