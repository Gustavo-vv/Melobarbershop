namespace Melobarbershop.Domain.Interfaces.Repositories;

using Melobarbershop.Domain.Entidades;

public interface IPacoteRepository
{
    Task<Pacote?> ObterPorIdAsync(int id);
    Task<Pacote?> ObterPorIdComItensAsync(int id);
    Task<IEnumerable<Pacote>> ObterTodosAsync();
    Task<IEnumerable<Pacote>> ObterAtivosAsync();
    Task AdicionarAsync(Pacote pacote);
    Task AtualizarAsync(Pacote pacote);
    Task RemoverAsync(Pacote pacote);
}