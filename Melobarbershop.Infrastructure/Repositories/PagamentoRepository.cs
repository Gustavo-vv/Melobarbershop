// ============================================================================
// Arquivo: PagamentoRepository.cs
// Camada: Melobarbershop.Infrastructure (Repositories)
// Objetivo: Repositório para persistência de transações financeiras e controle de recebimentos.
// Papel na Arquitetura:
//   - Recupera registros de pagamentos vinculados a vendas com suporte a relatórios por período.
//   - Agrupa totais financeiros recebidos por método de pagamento (Dinheiro, PIX, Cartão, etc.).
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Enums;
using Melobarbershop.Domain.Interfaces.Repositories;
using Melobarbershop.Infrastructure.Data;

namespace Melobarbershop.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório para operações financeiras relacionadas a Pagamento.
/// </summary>
public class PagamentoRepository : IPagamentoRepository
{
    private readonly BarbeariaDbContext _context;

    /// <summary>
    /// Construtor com injeção de dependência do contexto EF Core.
    /// </summary>
    public PagamentoRepository(BarbeariaDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Busca um pagamento por seu ID.
    /// </summary>
    public async Task<Pagamento?> ObterPorIdAsync(int id)
    {
        return await _context.Pagamentos
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// Lista todos os pagamentos vinculados a uma venda específica.
    /// </summary>
    public async Task<IEnumerable<Pagamento>> ObterPorVendaIdAsync(int vendaId)
    {
        return await _context.Pagamentos
            .Where(p => p.VendaId == vendaId)
            .OrderBy(p => p.DataHora)
            .ToListAsync();
    }

    /// <summary>
    /// Consulta pagamentos realizados dentro de um intervalo de datas, incluindo os dados da venda.
    /// </summary>
    public async Task<IEnumerable<Pagamento>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim)
    {
        return await _context.Pagamentos
            .Include(p => p.Venda)
            .Where(p => p.DataHora >= inicio && p.DataHora <= fim)
            .OrderByDescending(p => p.DataHora)
            .ToListAsync();
    }

    /// <summary>
    /// Consulta pagamentos filtrados por forma de pagamento específica e intervalo de datas.
    /// </summary>
    public async Task<IEnumerable<Pagamento>> ObterPorFormaPagamentoAsync(FormaPagamento formaPagamento, DateTime inicio, DateTime fim)
    {
        return await _context.Pagamentos
            .Include(p => p.Venda)
            .Where(p => p.Forma == formaPagamento && p.DataHora >= inicio && p.DataHora <= fim)
            .OrderByDescending(p => p.DataHora)
            .ToListAsync();
    }

    /// <summary>
    /// Calcula o somatório total de valores recebidos no período, opcionalmente filtrando por forma de pagamento.
    /// </summary>
    public async Task<decimal> ObterTotalRecebidoPorPeriodoAsync(DateTime inicio, DateTime fim, FormaPagamento? forma = null)
    {
        var query = _context.Pagamentos
            .Where(p => p.DataHora >= inicio && p.DataHora <= fim);

        if (forma.HasValue)
            query = query.Where(p => p.Forma == forma.Value);

        return await query.SumAsync(p => p.Valor);
    }

    /// <summary>
    /// Insere um novo registro de pagamento no banco de dados.
    /// </summary>
    public async Task AdicionarAsync(Pagamento pagamento)
    {
        await _context.Pagamentos.AddAsync(pagamento);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Remove um registro de pagamento do banco de dados (ex: cancelamento de transação/estorno).
    /// </summary>
    public async Task RemoverAsync(Pagamento pagamento)
    {
        _context.Pagamentos.Remove(pagamento);
        await _context.SaveChangesAsync();
    }
}
