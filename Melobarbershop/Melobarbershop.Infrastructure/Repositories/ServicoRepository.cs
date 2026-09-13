using Microsoft.EntityFrameworkCore;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;
using Melobarbershop.Infrastructure.Data;

namespace Melobarbershop.Infrastructure.Repositories;

public class ServicoRepository : IServicoRepository
{
    private readonly BarbeariaDbContext _context;

    public ServicoRepository(BarbeariaDbContext context)
    {
        _context = context;
    }

    public async Task<Servico?> ObterPorIdAsync(int id)
    {
        return await _context.Servicos
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Servico>> ObterPorIdsAsync(IEnumerable<int> ids)
    {
        return await _context.Servicos
            .Where(s => ids.Contains(s.Id))
            .ToListAsync();
    }

    public async Task<IEnumerable<Servico>> ObterTodosAsync()
    {
        return await _context.Servicos
            .OrderBy(s => s.Nome)
            .ToListAsync();
    }

    public async Task<IEnumerable<Servico>> ObterAtivosAsync()
    {
        return await _context.Servicos
            .Where(s => s.Ativo)
            .OrderBy(s => s.Nome)
            .ToListAsync();
    }

    public async Task<IEnumerable<Servico>> ObterExibidosNoSiteAsync()
    {
        return await _context.Servicos
            .Where(s => s.Ativo && s.ExibirNoSite)
            .OrderBy(s => s.Nome)
            .ToListAsync();
    }

    public async Task AdicionarAsync(Servico servico)
    {
        await _context.Servicos.AddAsync(servico);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Servico servico)
    {
        _context.Servicos.Update(servico);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Servico servico)
    {
        _context.Servicos.Remove(servico);
        await _context.SaveChangesAsync();
    }
}