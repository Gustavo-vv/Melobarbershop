// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Interfaces/Repositories/IVendaRepository.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem define: Domain (Inversão de Dependência - DIP)
// - Quem implementa: Melobarbershop.Infrastructure (VendaRepository via EF Core)
// - Quem consome: Melobarbershop.Application (VendaService, CaixaService, RelatoriosFinanceiros)
// ============================================================================

using Melobarbershop.Domain.Entidades;

namespace Melobarbershop.Domain.Interfaces.Repositories;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Contrato de persistência para as transações de venda comercial e faturamento de caixa.
/// 
/// POR QUE EXISTE:
/// Centraliza a persistência das comandas e recibos, além de disponibilizar consultas agregadas
/// para relatórios de faturamento e fechamento de turno da barbearia.
/// </summary>
public interface IVendaRepository
{
    /// <summary>
    /// Busca dados básicos de uma venda pelo ID primário.
    /// </summary>
    Task<Venda?> ObterPorIdAsync(int id);

    /// <summary>
    /// Busca a venda completa com Eager Loading (Include) de Cliente, Agendamento, Itens (Serviços e Produtos) e Pagamentos.
    /// Essencial para impressão de cupom não fiscal e visualização de detalhes da comanda.
    /// </summary>
    Task<Venda?> ObterPorIdCompletoAsync(int id);

    /// <summary>
    /// Lista o histórico de compras e consumos de um determinado cliente.
    /// </summary>
    Task<IEnumerable<Venda>> ObterPorClienteAsync(string clienteId);

    /// <summary>
    /// Localiza a comanda gerada a partir de um agendamento específico (relação 1:1).
    /// </summary>
    Task<Venda?> ObterPorAgendamentoIdAsync(int agendamentoId);

    /// <summary>
    /// Lista todas as vendas finalizadas dentro de uma janela de datas (para fechamento mensal ou diário).
    /// </summary>
    Task<IEnumerable<Venda>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim);

    /// <summary>
    /// Executa o cálculo agregado de soma (SUM) do ValorFinal faturado no período diretamente no banco de dados.
    /// </summary>
    Task<decimal> ObterTotalFaturadoPorPeriodoAsync(DateTime inicio, DateTime fim);

    /// <summary>
    /// Insere uma nova venda com seus itens e pagamentos em transação única.
    /// </summary>
    Task AdicionarAsync(Venda venda);

    /// <summary>
    /// Atualiza os dados de uma comanda aberta ou fechamento de venda.
    /// </summary>
    Task AtualizarAsync(Venda venda);
}