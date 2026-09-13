using Microsoft.EntityFrameworkCore;
// Arquivo: Melobarbershop.Infrastructure/Repositories/AvaliacaoRepository.cs
// Namespace: Melobarbershop.Infrastructure.Repositories
// Conteúdo: class AvaliacaoRepository : IAvaliacaoRepository
// Resumo: Implementação do repositório para Avaliacao, encapsulando operações de persistência.
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

    public async Task<Avaliacao?> ObterPorIdAsync(int id)
    {
        return await _context.Avaliacoes
            .FirstOrDefaultAsync(av => av.Id == id);
    }

    public async Task<Avaliacao?> ObterPorAgendamentoIdAsync(int agendamentoId)
    {
        return await _context.Avaliacoes
            .Include(av => av.Cliente)
            .Include(av => av.Barbeiro)
            .FirstOrDefaultAsync(av => av.AgendamentoId == agendamentoId);
    }

    public async Task<IEnumerable<Avaliacao>> ObterPorBarbeiroAsync(string barbeiroId)
    {
        return await _context.Avaliacoes
            .Include(av => av.Cliente)
            .Where(av => av.BarbeiroId == barbeiroId)
            .OrderByDescending(av => av.DataCriacao)
            .ToListAsync();
    }

    public async Task<IEnumerable<Avaliacao>> ObterPorClienteAsync(string clienteId)
    {
        return await _context.Avaliacoes
            .Include(av => av.Barbeiro)
            .Where(av => av.ClienteId == clienteId)
            .OrderByDescending(av => av.DataCriacao)
            .ToListAsync();
    }

    public async Task<double> CalcularMediaAvaliacoesBarbeiroAsync(string barbeiroId)
    {
        var avaliacoes = await _context.Avaliacoes
            .Where(av => av.BarbeiroId == barbeiroId)
            .Select(av => av.NotaEstrelas)
            .ToListAsync();

        if (!avaliacoes.Any())
            return 0.0;

        return avaliacoes.Average();
    }

    public async Task<bool> ExisteAvaliacaoParaAgendamentoAsync(int agendamentoId)
    {
        return await _context.Avaliacoes
            .AnyAsync(av => av.AgendamentoId == agendamentoId);
    }

    public async Task AdicionarAsync(Avaliacao avaliacao)
    {
        await _context.Avaliacoes.AddAsync(avaliacao);
        await _context.SaveChangesAsync();
    }
}
