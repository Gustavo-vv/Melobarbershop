// ============================================================================
// Arquivo: ProdutoRepository.cs
// Camada: Melobarbershop.Infrastructure (Repositories)
// Objetivo: Repositório para persistência de dados de produtos e histórico de movimentações de estoque.
// Papel na Arquitetura:
//   - Executa consultas otimizadas para produtos por código de barras e alertas de estoque mínimo.
//   - Persiste movimentações de auditoria de estoque (MovimentacaoEstoque).
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;
using Melobarbershop.Infrastructure.Data;

namespace Melobarbershop.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório para Produtos e controle de movimentações de estoque.
/// </summary>
public class ProdutoRepository : IProdutoRepository
{
    private readonly BarbeariaDbContext _context;

    /// <summary>
    /// Construtor com injeção do DbContext.
    /// </summary>
    public ProdutoRepository(BarbeariaDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Busca um produto pelo ID.
    /// </summary>
    public async Task<Produto?> ObterPorIdAsync(int id)
    {
        return await _context.Produtos
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// Busca um produto pelo seu código de barras único.
    /// </summary>
    public async Task<Produto?> ObterPorCodigoBarrasAsync(string codigoBarras)
    {
        return await _context.Produtos
            .FirstOrDefaultAsync(p => p.CodigoBarras == codigoBarras);
    }

    /// <summary>
    /// Lista todos os produtos cadastrados ordenados por nome.
    /// </summary>
    public async Task<IEnumerable<Produto>> ObterTodosAsync()
    {
        return await _context.Produtos
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }

    /// <summary>
    /// Lista apenas os produtos que estão com o status Ativo.
    /// </summary>
    public async Task<IEnumerable<Produto>> ObterAtivosAsync()
    {
        return await _context.Produtos
            .Where(p => p.Ativo)
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }

    /// <summary>
    /// Retorna produtos ativos cujo estoque atual está igual ou abaixo do patamar de alerta configurado.
    /// </summary>
    public async Task<IEnumerable<Produto>> ObterComEstoqueAbaixoDoMinimoAsync()
    {
        return await _context.Produtos
            .Where(p => p.Ativo && p.EstoqueAtual <= p.EstoqueMinimoAlerta)
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }

    /// <summary>
    /// Adiciona um novo produto no banco de dados.
    /// </summary>
    public async Task AdicionarAsync(Produto produto)
    {
        await _context.Produtos.AddAsync(produto);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Atualiza os dados de um produto existente.
    /// </summary>
    public async Task AtualizarAsync(Produto produto)
    {
        _context.Produtos.Update(produto);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Grava uma movimentação de estoque (entrada, saída, perda) na tabela de auditoria.
    /// </summary>
    public async Task AdicionarMovimentacaoEstoqueAsync(MovimentacaoEstoque movimentacao)
    {
        await _context.MovimentacoesEstoque.AddAsync(movimentacao);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Consulta o histórico de movimentações de um determinado produto filtrando opcionalmente por período.
    /// </summary>
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

    /// <summary>
    /// Remove um produto permanentemente do banco de dados (exclusão física).
    /// </summary>
    public async Task RemoverAsync(Produto produto)
    {
        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();
    }
}
