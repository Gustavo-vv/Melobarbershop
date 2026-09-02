namespace Barbearia.Application.Interfaces.Services;

using Melobarbershop.Domain.Entidades;

public interface IClienteService
{
    Task<Cliente?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Cliente?> ObterPorTelefoneAsync(string telefone, CancellationToken cancellationToken = default);
    Task<IEnumerable<Cliente>> ListarAtivosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Cliente>> ListarTodosAsync(CancellationToken cancellationToken = default);
    Task<Cliente> CriarAsync(string nome, string telefoneWhatsApp, string? email = null, DateTime? dataNascimento = null, string? preferenciasNotas = null, string? usuarioId = null, CancellationToken cancellationToken = default);
    Task<Cliente> AtualizarAsync(int id, string nome, string telefoneWhatsApp, string? email = null, DateTime? dataNascimento = null, string? preferenciasNotas = null, CancellationToken cancellationToken = default);
    Task DesativarAsync(int id, CancellationToken cancellationToken = default);
    Task AtivarAsync(int id, CancellationToken cancellationToken = default);
    Task AtualizarPreferenciasAsync(int id, string preferenciasNotas, CancellationToken cancellationToken = default);
}