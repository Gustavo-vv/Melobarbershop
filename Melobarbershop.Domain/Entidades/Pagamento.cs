// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Entidades/Pagamento.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem chama/usa:
//   * Melobarbershop.Application (VendaService, CaixaService, RelatoriosFinanceiros)
//   * Melobarbershop.Infrastructure (BarbeariaDbContext, PagamentoConfiguration, PagamentoRepository)
//   * Melobarbershop.Desktop (Frente de Caixa / PDV, fechamento de comanda)
// - Quem ele referencia:
//   * Venda (agregação pai 1:N)
//   * FormaPagamento (Enum: Dinheiro, CartaoCredito, CartaoDebito, Pix, etc.)
// ============================================================================

using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Domain.Entidades;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Entidade de movimentação financeira que registra a quitação total ou parcial de uma Venda.
/// 
/// POR QUE EXISTE:
/// Suporta pagamentos divididos (split payment) em uma mesma transação
/// (ex: R$ 50,00 no PIX e R$ 25,00 em Dinheiro para uma conta de R$ 75,00).
/// 
/// O QUE QUEBRARIA SE NÃO EXISTISSE:
/// O sistema só aceitaria uma única forma de pagamento por venda, limitando a flexibilidade do caixa da barbearia
/// e distorcendo a conciliação de faturamento por método de recebimento.
/// </summary>
public class Pagamento
{
    /// <summary>
    /// Identificador único do registro de pagamento (chave primária).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Chave estrangeira da Venda correspondente.
    /// </summary>
    public int VendaId { get; set; }

    /// <summary>
    /// Propriedade de navegação para a entidade Venda associada.
    /// </summary>
    public Venda Venda { get; set; } = null!;

    /// <summary>
    /// Método financeiro utilizado (Dinheiro, Pix, CartaoCredito, CartaoDebito, Outro).
    /// </summary>
    public FormaPagamento Forma { get; set; }

    /// <summary>
    /// Valor monetário líquido amortizado por esta forma de pagamento.
    /// </summary>
    public decimal Valor { get; set; }

    /// <summary>
    /// Timestamp do recebimento financeiro (em UTC).
    /// </summary>
    public DateTime DataHora { get; set; } = DateTime.UtcNow;
}
