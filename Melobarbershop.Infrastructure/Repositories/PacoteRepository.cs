using Microsoft.EntityFrameworkCore;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;
using Melobarbershop.Infrastructure.Data;

namespace Melobarbershop.Infrastructure.Repositories;

public class PacoteRepository : IPacoteRepository
{
    private readonly BarbeariaDbContext _context;

    public PacoteRepository(BarbeariaDbContext context)
    {
        _context = context;
    }

    public async Task<Pacote?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Pacotes
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Pacote?> ObterPorIdComItensAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Pacotes
            .Include(p => p.Itens)
                .ThenInclude(i => i.Servico)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Pacote>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Pacotes
            .Include(p => p.Itens)
                .ThenInclude(i => i.Servico)
            .OrderBy(p => p.Nome)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Pacote>> ObterAtivosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Pacotes
            .Include(p => p.Itens)
                .ThenInclude(i => i.Servico)
            .Where(p => p.Ativo)
            .OrderBy(p => p.Nome)
            .ToListAsync(cancellationToken);
    }

    public async Task AdicionarAsync(Pacote pacote, CancellationToken cancellationToken = default)
    {
        await _context.Pacotes.AddAsync(pacote, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AtualizarAsync(Pacote pacote, CancellationToken cancellationToken = default)
    {
        _context.Pacotes.Update(pacote);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoverAsync(Pacote pacote, CancellationToken cancellationToken = default)
    {
        _context.Pacotes.Remove(pacote);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
