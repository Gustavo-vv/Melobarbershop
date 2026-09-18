// ============================================================================
// Arquivo: VendaRepository.cs
// Camada: Melobarbershop.Infrastructure (Repositories)
// Objetivo: Implementação do repositório para persistência de vendas, comandas e pagamentos.
// Papel na Arquitetura:
//   - Recupera comandas com carregamento completo de cliente, itens (serviços/produtos/barbeiros) e pagamentos.
//   - Executa consultas agregadas financeiras (total faturado por período).
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;
using Melobarbershop.Infrastructure.Data;

namespace Melobarbershop.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório para Vendas, comandas e pagamentos.
/// </summary>
public class VendaRepository : IVendaRepository
{
    private readonly BarbeariaDbContext _context;

    /// <summary>
    /// Construtor com injeção do DbContext.
    /// </summary>
    public VendaRepository(BarbeariaDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtém dados básicos da venda sem carregar coleções filhas.
    /// </summary>
    public async Task<Venda?> ObterPorIdAsync(int id)
    {
        return await _context.Vendas
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    /// <summary>
    /// Obtém a comanda de venda com todas as suas dependências (Cliente, Itens com Serviços/Produtos/Barbeiros e Pagamentos).
    /// </summary>
    public async Task<Venda?> ObterPorIdCompletoAsync(int id)
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
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    /// <summary>
    /// Lista o histórico de compras e atendimentos de um cliente ordenado por data decrescente.
    /// </summary>
    public async Task<IEnumerable<Venda>> ObterPorClienteAsync(string clienteId)
    {
        return await _context.Vendas
            .Include(v => v.Itens)
            .Include(v => v.Pagamentos)
            .Where(v => v.ClienteId == clienteId)
            .OrderByDescending(v => v.DataHora)
            .ToListAsync();
    }

    /// <summary>
    /// Localiza a venda associada a um determinado ID de agendamento.
    /// </summary>
    public async Task<Venda?> ObterPorAgendamentoIdAsync(int agendamentoId)
    {
        return await _context.Vendas
            .Include(v => v.Itens)
            .Include(v => v.Pagamentos)
            .FirstOrDefaultAsync(v => v.AgendamentoId == agendamentoId);
    }

    /// <summary>
    /// Lista as vendas realizadas em uma janela de datas para conciliação ou relatórios de caixa.
    /// </summary>
    public async Task<IEnumerable<Venda>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim)
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
            .ToListAsync();
    }

    /// <summary>
    /// Calcula o somatório do valor final faturado em um período específico diretamente no banco de dados.
    /// </summary>
    public async Task<decimal> ObterTotalFaturadoPorPeriodoAsync(DateTime inicio, DateTime fim)
    {
        return await _context.Vendas
            .Where(v => v.DataHora >= inicio && v.DataHora <= fim)
            .SumAsync(v => v.ValorFinal);
    }

    /// <summary>
    /// Adiciona uma nova comanda de venda ao banco.
    /// </summary>
    public async Task AdicionarAsync(Venda venda)
    {
        await _context.Vendas.AddAsync(venda);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Atualiza as informações da comanda, itens ou pagamentos.
    /// </summary>
    public async Task AtualizarAsync(Venda venda)
    {
        _context.Vendas.Update(venda);
        await _context.SaveChangesAsync();
    }
}
