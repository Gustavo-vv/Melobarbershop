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

    public async Task<Produto?> ObterPorIdAsync(int id)
    {
        return await _context.Produtos
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Produto?> ObterPorCodigoBarrasAsync(string codigoBarras)
    {
        return await _context.Produtos
            .FirstOrDefaultAsync(p => p.CodigoBarras == codigoBarras);
    }

    public async Task<IEnumerable<Produto>> ObterTodosAsync()
    {
        return await _context.Produtos
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }

    public async Task<IEnumerable<Produto>> ObterAtivosAsync()
    {
        return await _context.Produtos
            .Where(p => p.Ativo)
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }

    public async Task<IEnumerable<Produto>> ObterComEstoqueAbaixoDoMinimoAsync()
    {
        return await _context.Produtos
            .Where(p => p.Ativo && p.EstoqueAtual <= p.EstoqueMinimoAlerta)
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }

    public async Task AdicionarAsync(Produto produto)
    {
        await _context.Produtos.AddAsync(produto);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Produto produto)
    {
        _context.Produtos.Update(produto);
        await _context.SaveChangesAsync();
    }

    public async Task AdicionarMovimentacaoEstoqueAsync(MovimentacaoEstoque movimentacao)
    {
        await _context.MovimentacoesEstoque.AddAsync(movimentacao);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<MovimentacaoEstoque>> ObterMovimentacoesPorProdutoAsync(int produtoId, DateTime? inicio = null, DateTime? fim = null)
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
            .ToListAsync();
    }

    public async Task RemoverAsync(Produto produto)
    {
        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();
    }
}
