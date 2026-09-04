using Melobarbershop.Application.DTOs;

namespace Melobarbershop.Application.Servicos.Services;

public interface IUsuarioService
{
    Task<UsuarioDto?> ObterPorIdAsync(string id, CancellationToken cancellationToken = default);
    Task<UsuarioDto?> ObterPorTelefoneAsync(string telefone, CancellationToken cancellationToken = default);
    Task<UsuarioDto?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<UsuarioDto>> ListarPorRoleAsync(string roleName, bool apenasAtivos = true, CancellationToken cancellationToken = default);
    Task<UsuarioDto> CriarAsync(CriarUsuarioDto dto, CancellationToken cancellationToken = default);
    Task<UsuarioDto> AtualizarAsync(string id, AtualizarUsuarioDto dto, CancellationToken cancellationToken = default);
    Task DesativarAsync(string id, CancellationToken cancellationToken = default);
    Task AtivarAsync(string id, CancellationToken cancellationToken = default);
    Task<BloqueioAgendaDto> AdicionarBloqueioAgendaAsync(CriarBloqueioAgendaDto dto, CancellationToken cancellationToken = default);
    Task RemoverBloqueioAgendaAsync(int bloqueioId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BloqueioAgendaDto>> ListarBloqueiosBarbeiroAsync(string barbeiroId, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
    Task<bool> VerificarDisponibilidadeBarbeiroAsync(string barbeiroId, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
}
