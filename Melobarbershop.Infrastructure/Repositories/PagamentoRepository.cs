using Microsoft.EntityFrameworkCore;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Enums;
using Melobarbershop.Domain.Interfaces.Repositories;
using Melobarbershop.Infrastructure.Data;

namespace Melobarbershop.Infrastructure.Repositories;

public class PagamentoRepository : IPagamentoRepository
{
    private readonly BarbeariaDbContext _context;

    public PagamentoRepository(BarbeariaDbContext context)
    {
        _context = context;
    }

    public async Task<Pagamento?> ObterPorIdAsync(int id)
    {
        return await _context.Pagamentos
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Pagamento>> ObterPorVendaIdAsync(int vendaId)
    {
        return await _context.Pagamentos
            .Where(p => p.VendaId == vendaId)
            .OrderBy(p => p.DataHora)
            .ToListAsync();
    }

    public async Task<IEnumerable<Pagamento>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim)
    {
        return await _context.Pagamentos
            .Include(p => p.Venda)
            .Where(p => p.DataHora >= inicio && p.DataHora <= fim)
            .OrderByDescending(p => p.DataHora)
            .ToListAsync();
    }

    public async Task<IEnumerable<Pagamento>> ObterPorFormaPagamentoAsync(FormaPagamento formaPagamento, DateTime inicio, DateTime fim)
    {
        return await _context.Pagamentos
            .Include(p => p.Venda)
            .Where(p => p.Forma == formaPagamento && p.DataHora >= inicio && p.DataHora <= fim)
            .OrderByDescending(p => p.DataHora)
            .ToListAsync();
    }

    public async Task<decimal> ObterTotalRecebidoPorPeriodoAsync(DateTime inicio, DateTime fim, FormaPagamento? forma = null)
    {
        var query = _context.Pagamentos
            .Where(p => p.DataHora >= inicio && p.DataHora <= fim);

        if (forma.HasValue)
            query = query.Where(p => p.Forma == forma.Value);

        return await query.SumAsync(p => p.Valor);
    }

    public async Task AdicionarAsync(Pagamento pagamento)
    {
        await _context.Pagamentos.AddAsync(pagamento);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Pagamento pagamento)
    {
        _context.Pagamentos.Remove(pagamento);
        await _context.SaveChangesAsync();
    }
}
