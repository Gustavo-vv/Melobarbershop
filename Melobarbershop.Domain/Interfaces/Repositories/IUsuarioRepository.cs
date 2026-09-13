// Arquivo: Melobarbershop.Domain/Interfaces/Repositories/IUsuarioRepository.cs
// Namespace: Melobarbershop.Domain.Interfaces.Repositories
// Conteúdo: interface IUsuarioRepository
// Resumo: Contrato de repositório para operações relacionadas a ApplicationUser (usuários do sistema).
using Melobarbershop.Domain.Entidades;

namespace Melobarbershop.Domain.Interfaces.Repositories;

public interface IUsuarioRepository
{
    Task<ApplicationUser?> ObterPorIdAsync(string id);
    Task<ApplicationUser?> ObterPorTelefoneAsync(string telefone);
    Task<ApplicationUser?> ObterPorEmailAsync(string email);
    Task<IEnumerable<ApplicationUser>> ObterPorRoleAsync(string roleName);
    Task<IEnumerable<ApplicationUser>> ObterAtivosPorRoleAsync(string roleName);
    Task<bool> ExisteTelefoneAsync(string telefone, string? usuarioIdIgnorar = null);
    Task<bool> ExisteEmailAsync(string email, string? usuarioIdIgnorar = null);
    Task<IEnumerable<BloqueioAgenda>> ObterBloqueiosPorPeriodoAsync(string barbeiroId, DateTime inicio, DateTime fim);
    Task<bool> ExisteBloqueioNoPeriodoAsync(string barbeiroId, DateTime inicio, DateTime fim);
    Task<BloqueioAgenda?> ObterBloqueioPorIdAsync(int bloqueioId);
    Task AdicionarBloqueioAsync(BloqueioAgenda bloqueio);
    Task RemoverBloqueioAsync(BloqueioAgenda bloqueio);
    Task AtualizarAsync(ApplicationUser usuario);
}
