using Microsoft.EntityFrameworkCore;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;
using Melobarbershop.Infrastructure.Data;

namespace Melobarbershop.Infrastructure.Repositories;

public class AvaliacaoRepository : IAvaliacaoRepository
{
    private readonly BarbeariaDbContext _context;

    public AvaliacaoRepository(BarbeariaDbContext context)
    {
        _context = context;
    }

    public async Task<Avaliacao?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Avaliacoes
            .FirstOrDefaultAsync(av => av.Id == id, cancellationToken);
    }

    public async Task<Avaliacao?> ObterPorAgendamentoIdAsync(int agendamentoId, CancellationToken cancellationToken = default)
    {
        return await _context.Avaliacoes
            .Include(av => av.Cliente)
            .Include(av => av.Barbeiro)
            .FirstOrDefaultAsync(av => av.AgendamentoId == agendamentoId, cancellationToken);
    }

    public async Task<IEnumerable<Avaliacao>> ObterPorBarbeiroAsync(string barbeiroId, CancellationToken cancellationToken = default)
    {
        return await _context.Avaliacoes
            .Include(av => av.Cliente)
            .Where(av => av.BarbeiroId == barbeiroId)
            .OrderByDescending(av => av.DataCriacao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Avaliacao>> ObterPorClienteAsync(string clienteId, CancellationToken cancellationToken = default)
    {
        return await _context.Avaliacoes
            .Include(av => av.Barbeiro)
            .Where(av => av.ClienteId == clienteId)
            .OrderByDescending(av => av.DataCriacao)
            .ToListAsync(cancellationToken);
    }

    public async Task<double> CalcularMediaAvaliacoesBarbeiroAsync(string barbeiroId, CancellationToken cancellationToken = default)
    {
        var avaliacoes = await _context.Avaliacoes
            .Where(av => av.BarbeiroId == barbeiroId)
            .Select(av => av.NotaEstrelas)
            .ToListAsync(cancellationToken);

        if (!avaliacoes.Any())
            return 0.0;

        return avaliacoes.Average();
    }

    public async Task<bool> ExisteAvaliacaoParaAgendamentoAsync(int agendamentoId, CancellationToken cancellationToken = default)
    {
        return await _context.Avaliacoes
            .AnyAsync(av => av.AgendamentoId == agendamentoId, cancellationToken);
    }

    public async Task AdicionarAsync(Avaliacao avaliacao, CancellationToken cancellationToken = default)
    {
        await _context.Avaliacoes.AddAsync(avaliacao, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
