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

    public async Task<Servico?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Servicos
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Servico>> ObterPorIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
    {
        return await _context.Servicos
            .Where(s => ids.Contains(s.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Servico>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Servicos
            .OrderBy(s => s.Nome)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Servico>> ObterAtivosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Servicos
            .Where(s => s.Ativo)
            .OrderBy(s => s.Nome)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Servico>> ObterExibidosNoSiteAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Servicos
            .Where(s => s.Ativo && s.ExibirNoSite)
            .OrderBy(s => s.Nome)
            .ToListAsync(cancellationToken);
    }

    public async Task AdicionarAsync(Servico servico, CancellationToken cancellationToken = default)
    {
        await _context.Servicos.AddAsync(servico, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AtualizarAsync(Servico servico, CancellationToken cancellationToken = default)
    {
        _context.Servicos.Update(servico);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoverAsync(Servico servico, CancellationToken cancellationToken = default)
    {
        _context.Servicos.Remove(servico);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
