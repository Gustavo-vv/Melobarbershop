using Melobarbershop.Application.DTOs;

namespace Melobarbershop.Application.Servicos.Services;

public interface IUsuarioService
{
    Task<UsuarioDto?> ObterPorIdAsync(string id);
    Task<UsuarioDto?> ObterPorTelefoneAsync(string telefone);
    Task<UsuarioDto?> ObterPorEmailAsync(string email);
    Task<IEnumerable<UsuarioDto>> ListarPorRoleAsync(string roleName, bool apenasAtivos = true);
    Task<UsuarioDto> CriarAsync(CriarUsuarioDto dto);
    Task<UsuarioDto> AtualizarAsync(string id, AtualizarUsuarioDto dto);
    Task<UsuarioDto> AtualizarDadosClienteAsync(string id, AtualizarDadosClienteDto dto);
    Task DesativarAsync(string id);
    Task AtivarAsync(string id);
    Task<BloqueioAgendaDto> AdicionarBloqueioAgendaAsync(CriarBloqueioAgendaDto dto);
    Task RemoverBloqueioAgendaAsync(int bloqueioId);
    Task<IEnumerable<BloqueioAgendaDto>> ListarBloqueiosBarbeiroAsync(string barbeiroId, DateTime inicio, DateTime fim);
    Task<bool> VerificarDisponibilidadeBarbeiroAsync(string barbeiroId, DateTime inicio, DateTime fim);
}
