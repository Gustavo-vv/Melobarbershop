// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Interfaces/Repositories/IPagamentoRepository.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem define: Domain (Inversão de Dependência - DIP)
// - Quem implementa: Melobarbershop.Infrastructure (PagamentoRepository via EF Core)
// - Quem consome: Melobarbershop.Application (VendaService, CaixaService, RelatoriosFinanceiros)
// ============================================================================

using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Domain.Interfaces.Repositories;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Contrato de persistência para as transações de pagamento financeiro.
/// 
/// POR QUE EXISTE:
/// Centraliza o acesso aos lançamentos de recebíveis, fornecendo métodos otimizados para totalizações
/// e conciliações de caixa por data e por forma de pagamento.
/// </summary>
public interface IPagamentoRepository
{
    /// <summary>
    /// Busca um lançamento financeiro pelo ID primário.
    /// </summary>
    Task<Pagamento?> ObterPorIdAsync(int id);

    /// <summary>
    /// Lista todos os pagamentos vinculados a uma venda/comanda específica.
    /// </summary>
    Task<IEnumerable<Pagamento>> ObterPorVendaIdAsync(int vendaId);

    /// <summary>
    /// Lista todos os pagamentos efetuados dentro de um intervalo de datas (para fechamento de turno/dia).
    /// </summary>
    Task<IEnumerable<Pagamento>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim);

    /// <summary>
    /// Filtra os pagamentos de um período por uma modalidade específica (ex: listar somente recebimentos em Pix).
    /// </summary>
    Task<IEnumerable<Pagamento>> ObterPorFormaPagamentoAsync(FormaPagamento formaPagamento, DateTime inicio, DateTime fim);

    /// <summary>
    /// Executa uma consulta agregada de soma (SUM) diretamente no banco de dados para totalizar
    /// o valor financeiro recebido em um período, com filtro opcional por forma de pagamento.
    /// </summary>
    Task<decimal> ObterTotalRecebidoPorPeriodoAsync(DateTime inicio, DateTime fim, FormaPagamento? forma = null);

    /// <summary>
    /// Registra um novo pagamento no banco de dados.
    /// </summary>
    Task AdicionarAsync(Pagamento pagamento);

    /// <summary>
    /// Estorna ou remove um pagamento registrado.
    /// </summary>
    Task RemoverAsync(Pagamento pagamento);
}