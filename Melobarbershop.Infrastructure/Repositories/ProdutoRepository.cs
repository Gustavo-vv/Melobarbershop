using Microsoft.EntityFrameworkCore;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;
using Melobarbershop.Infrastructure.Data;

namespace Melobarbershop.Infrastructure.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly BarbeariaDbContext _context;

    public ProdutoRepository(BarbeariaDbContext context)
    {
        _context = context;
    }

    public async Task<Produto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Produtos
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Produto?> ObterPorCodigoBarrasAsync(string codigoBarras, CancellationToken cancellationToken = default)
    {
        return await _context.Produtos
            .FirstOrDefaultAsync(p => p.CodigoBarras == codigoBarras, cancellationToken);
    }

    public async Task<IEnumerable<Produto>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Produtos
            .OrderBy(p => p.Nome)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Produto>> ObterAtivosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Produtos
            .Where(p => p.Ativo)
            .OrderBy(p => p.Nome)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Produto>> ObterComEstoqueAbaixoDoMinimoAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Produtos
            .Where(p => p.Ativo && p.EstoqueAtual <= p.EstoqueMinimoAlerta)
            .OrderBy(p => p.Nome)
            .ToListAsync(cancellationToken);
    }

    public async Task AdicionarAsync(Produto produto, CancellationToken cancellationToken = default)
    {
        await _context.Produtos.AddAsync(produto, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AtualizarAsync(Produto produto, CancellationToken cancellationToken = default)
    {
        _context.Produtos.Update(produto);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AdicionarMovimentacaoEstoqueAsync(MovimentacaoEstoque movimentacao, CancellationToken cancellationToken = default)
    {
        await _context.MovimentacoesEstoque.AddAsync(movimentacao, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<MovimentacaoEstoque>> ObterMovimentacoesPorProdutoAsync(int produtoId, DateTime? inicio = null, DateTime? fim = null, CancellationToken cancellationToken = default)
    {
        var query = _context.MovimentacoesEstoque
            .Include(m => m.Produto)
            .Where(m => m.ProdutoId == produtoId);

        if (inicio.HasValue)
            query = query.Where(m => m.DataHora >= inicio.Value);

        if (fim.HasValue)
            query = query.Where(m => m.DataHora <= fim.Value);

        return await query
            .OrderByDescending(m => m.DataHora)
            .ToListAsync(cancellationToken);
    }

    public async Task RemoverAsync(Produto produto, CancellationToken cancellationToken = default)
    {
        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
