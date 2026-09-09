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

    public async Task<Pacote?> ObterPorIdAsync(int id)
    {
        return await _context.Pacotes
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Pacote?> ObterPorIdComItensAsync(int id)
    {
        return await _context.Pacotes
            .Include(p => p.Itens)
                .ThenInclude(i => i.Servico)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Pacote>> ObterTodosAsync()
    {
        return await _context.Pacotes
            .Include(p => p.Itens)
                .ThenInclude(i => i.Servico)
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }

    public async Task<IEnumerable<Pacote>> ObterAtivosAsync()
    {
        return await _context.Pacotes
            .Include(p => p.Itens)
                .ThenInclude(i => i.Servico)
            .Where(p => p.Ativo)
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }

    public async Task AdicionarAsync(Pacote pacote)
    {
        await _context.Pacotes.AddAsync(pacote);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Pacote pacote)
    {
        _context.Pacotes.Update(pacote);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Pacote pacote)
    {
        _context.Pacotes.Remove(pacote);
        await _context.SaveChangesAsync();
    }
}
