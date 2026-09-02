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

    public async Task<ApplicationUser?> ObterPorIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<ApplicationUser?> ObterPorTelefoneAsync(string telefone, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == telefone, cancellationToken);
    }

    public async Task<ApplicationUser?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<IEnumerable<ApplicationUser>> ObterPorRoleAsync(string roleName, CancellationToken cancellationToken = default)
    {
        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.NormalizedName == roleName.ToUpper(), cancellationToken);

        if (role == null)
            return Enumerable.Empty<ApplicationUser>();

        var userIds = await _context.UserRoles
            .Where(ur => ur.RoleId == role.Id)
            .Select(ur => ur.UserId)
            .ToListAsync(cancellationToken);

        return await _context.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ApplicationUser>> ObterAtivosPorRoleAsync(string roleName, CancellationToken cancellationToken = default)
    {
        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.NormalizedName == roleName.ToUpper(), cancellationToken);

        if (role == null)
            return Enumerable.Empty<ApplicationUser>();

        var userIds = await _context.UserRoles
            .Where(ur => ur.RoleId == role.Id)
            .Select(ur => ur.UserId)
            .ToListAsync(cancellationToken);

        return await _context.Users
            .Where(u => userIds.Contains(u.Id) && u.Ativo)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExisteTelefoneAsync(string telefone, string? usuarioIdIgnorar = null, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AnyAsync(u => u.PhoneNumber == telefone && (usuarioIdIgnorar == null || u.Id != usuarioIdIgnorar), cancellationToken);
    }

    public async Task<bool> ExisteEmailAsync(string email, string? usuarioIdIgnorar = null, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email && (usuarioIdIgnorar == null || u.Id != usuarioIdIgnorar), cancellationToken);
    }

    public async Task<IEnumerable<BloqueioAgenda>> ObterBloqueiosPorPeriodoAsync(string barbeiroId, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
    {
        return await _context.BloqueiosAgenda
            .Where(b => b.BarbeiroId == barbeiroId && b.DataHoraInicio < fim && b.DataHoraFim > inicio)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExisteBloqueioNoPeriodoAsync(string barbeiroId, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
    {
        return await _context.BloqueiosAgenda
            .AnyAsync(b => b.BarbeiroId == barbeiroId && b.DataHoraInicio < fim && b.DataHoraFim > inicio, cancellationToken);
    }

    public async Task<BloqueioAgenda?> ObterBloqueioPorIdAsync(int bloqueioId, CancellationToken cancellationToken = default)
    {
        return await _context.BloqueiosAgenda
            .FirstOrDefaultAsync(b => b.Id == bloqueioId, cancellationToken);
    }

    public async Task AdicionarBloqueioAsync(BloqueioAgenda bloqueio, CancellationToken cancellationToken = default)
    {
        await _context.BloqueiosAgenda.AddAsync(bloqueio, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoverBloqueioAsync(BloqueioAgenda bloqueio, CancellationToken cancellationToken = default)
    {
        _context.BloqueiosAgenda.Remove(bloqueio);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AtualizarAsync(ApplicationUser usuario, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(usuario);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
