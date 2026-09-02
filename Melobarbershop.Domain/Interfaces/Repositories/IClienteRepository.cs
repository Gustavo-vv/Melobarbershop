namespace Melobarbershop.Domain.Interfaces.Repositories;

using Melobarbershop.Domain.Entidades;

public interface IClienteRepository
{
    Task<Cliente?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Cliente?> ObterPorTelefoneAsync(string telefone, CancellationToken cancellationToken = default);
    Task<Cliente?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Cliente?> ObterPorUsuarioIdAsync(string usuarioId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Cliente>> ObterTodosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Cliente>> ObterAtivosAsync(CancellationToken cancellationToken = default);
    Task<bool> ExisteTelefoneAsync(string telefone, int? clienteIdIgnorar = null, CancellationToken cancellationToken = default);
    Task<bool> ExisteEmailAsync(string email, int? clienteIdIgnorar = null, CancellationToken cancellationToken = default);
    Task AdicionarAsync(Cliente cliente, CancellationToken cancellationToken = default);
    Task AtualizarAsync(Cliente cliente, CancellationToken cancellationToken = default);
    Task RemoverAsync(Cliente cliente, CancellationToken cancellationToken = default);
}