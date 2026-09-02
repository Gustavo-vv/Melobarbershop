using Microsoft.EntityFrameworkCore;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;
using Melobarbershop.Infrastructure.Data;

namespace Melobarbershop.Infrastructure.Repositories;

public class VendaRepository : IVendaRepository
{
    private readonly BarbeariaDbContext _context;

    public VendaRepository(BarbeariaDbContext context)
    {
        _context = context;
    }

    public async Task<Venda?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Vendas
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<Venda?> ObterPorIdCompletoAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Vendas
            .Include(v => v.Cliente)
            .Include(v => v.Itens)
                .ThenInclude(i => i.Servico)
            .Include(v => v.Itens)
                .ThenInclude(i => i.Produto)
            .Include(v => v.Itens)
                .ThenInclude(i => i.Barbeiro)
            .Include(v => v.Pagamentos)
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Venda>> ObterPorClienteAsync(string clienteId, CancellationToken cancellationToken = default)
    {
        return await _context.Vendas
            .Include(v => v.Itens)
            .Include(v => v.Pagamentos)
            .Where(v => v.ClienteId == clienteId)
            .OrderByDescending(v => v.DataHora)
            .ToListAsync(cancellationToken);
    }

    public async Task<Venda?> ObterPorAgendamentoIdAsync(int agendamentoId, CancellationToken cancellationToken = default)
    {
        return await _context.Vendas
            .Include(v => v.Itens)
            .Include(v => v.Pagamentos)
            .FirstOrDefaultAsync(v => v.AgendamentoId == agendamentoId, cancellationToken);
    }

    public async Task<IEnumerable<Venda>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
    {
        return await _context.Vendas
            .Include(v => v.Cliente)
            .Include(v => v.Itens)
                .ThenInclude(i => i.Servico)
            .Include(v => v.Itens)
                .ThenInclude(i => i.Produto)
            .Include(v => v.Pagamentos)
            .Where(v => v.DataHora >= inicio && v.DataHora <= fim)
            .OrderByDescending(v => v.DataHora)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> ObterTotalFaturadoPorPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
    {
        return await _context.Vendas
            .Where(v => v.DataHora >= inicio && v.DataHora <= fim)
            .SumAsync(v => v.ValorFinal, cancellationToken);
    }

    public async Task AdicionarAsync(Venda venda, CancellationToken cancellationToken = default)
    {
        await _context.Vendas.AddAsync(venda, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AtualizarAsync(Venda venda, CancellationToken cancellationToken = default)
    {
        _context.Vendas.Update(venda);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
