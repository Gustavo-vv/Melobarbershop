// Arquivo: Melobarbershop.Infrastructure/Repositories/UsuarioRepository.cs
// Namespace: Melobarbershop.Infrastructure.Repositories
// Conteúdo: class UsuarioRepository : IUsuarioRepository
// Resumo: Implementação do repositório para usuários (consultas por perfil, atualização de dados).
using Microsoft.EntityFrameworkCore;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;
using Melobarbershop.Infrastructure.Data;

namespace Melobarbershop.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly BarbeariaDbContext _context;

    public UsuarioRepository(BarbeariaDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationUser?> ObterPorIdAsync(string id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<ApplicationUser?> ObterPorTelefoneAsync(string telefone)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == telefone);
    }

    public async Task<ApplicationUser?> ObterPorEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<ApplicationUser>> ObterPorRoleAsync(string roleName)
    {
        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.NormalizedName == roleName.ToUpper());

        if (role == null)
            return Enumerable.Empty<ApplicationUser>();

        var userIds = await _context.UserRoles
            .Where(ur => ur.RoleId == role.Id)
            .Select(ur => ur.UserId)
            .ToListAsync();

        return await _context.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync();
    }

    public async Task<IEnumerable<ApplicationUser>> ObterAtivosPorRoleAsync(string roleName)
    {
        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.NormalizedName == roleName.ToUpper());

        if (role == null)
            return Enumerable.Empty<ApplicationUser>();

        var userIds = await _context.UserRoles
            .Where(ur => ur.RoleId == role.Id)
            .Select(ur => ur.UserId)
            .ToListAsync();

        return await _context.Users
            .Where(u => userIds.Contains(u.Id) && u.Ativo)
            .ToListAsync();
    }

    public async Task<bool> ExisteTelefoneAsync(string telefone, string? usuarioIdIgnorar = null)
    {
        return await _context.Users
            .AnyAsync(u => u.PhoneNumber == telefone && (usuarioIdIgnorar == null || u.Id != usuarioIdIgnorar));
    }

    public async Task<bool> ExisteEmailAsync(string email, string? usuarioIdIgnorar = null)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email && (usuarioIdIgnorar == null || u.Id != usuarioIdIgnorar));
    }

    public async Task<IEnumerable<BloqueioAgenda>> ObterBloqueiosPorPeriodoAsync(string barbeiroId, DateTime inicio, DateTime fim)
    {
        return await _context.BloqueiosAgenda
            .Where(b => b.BarbeiroId == barbeiroId && b.DataHoraInicio < fim && b.DataHoraFim > inicio)
            .ToListAsync();
    }

    public async Task<bool> ExisteBloqueioNoPeriodoAsync(string barbeiroId, DateTime inicio, DateTime fim)
    {
        return await _context.BloqueiosAgenda
            .AnyAsync(b => b.BarbeiroId == barbeiroId && b.DataHoraInicio < fim && b.DataHoraFim > inicio);
    }

    public async Task<BloqueioAgenda?> ObterBloqueioPorIdAsync(int bloqueioId)
    {
        return await _context.BloqueiosAgenda
            .FirstOrDefaultAsync(b => b.Id == bloqueioId);
    }

    public async Task AdicionarBloqueioAsync(BloqueioAgenda bloqueio)
    {
        await _context.BloqueiosAgenda.AddAsync(bloqueio);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverBloqueioAsync(BloqueioAgenda bloqueio)
    {
        _context.BloqueiosAgenda.Remove(bloqueio);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(ApplicationUser usuario)
    {
        _context.Users.Update(usuario);
        await _context.SaveChangesAsync();
    }
}
