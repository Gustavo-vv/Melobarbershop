// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Enums/FormaPagamento.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem usa:
//   * Pagamento (propriedade Forma na entidade de domínio)
//   * Melobarbershop.Application (DTOs de pagamento, relatórios por canal)
//   * Melobarbershop.Desktop (Dropdown de fechamento de venda no Caixa / PDV)
// ============================================================================

namespace Melobarbershop.Domain.Enums;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Enumeração tipada que delimita as modalidades financeiras aceitas pela barbearia.
/// 
/// POR QUE EXISTE:
/// Evita o uso de "magic strings" (como "cartao", "PIX", "dinheiro") sujeitas a erros de digitação,
/// garantindo consistência na persistência e nos filtros de relatórios de caixa diário.
/// 
/// O QUE QUEBRARIA SE NÃO EXISTISSE:
/// A separação do faturamento diário por taxa de operadora (Cartão de Crédito vs Débito vs Pix)
/// e o controle de troco para dinheiro físico ficariam vulneráveis a inconsistências.
/// </summary>
public enum FormaPagamento
{
    /// <summary>
    /// Pagamento em espécie (cédulas/moedas), exigindo conferência física de caixa e troco.
    /// </summary>
    Dinheiro = 1,

    /// <summary>
    /// Transferência instantânea PIX (comprovante bancário imediato).
    /// </summary>
    Pix = 2,

    /// <summary>
    /// Cartão de crédito à vista ou parcelado via máquina POS.
    /// </summary>
    CartaoCredito = 3,

    /// <summary>
    /// Cartão de débito à vista debitado em conta corrente.
    /// </summary>
    CartaoDebito = 4,

    /// <summary>
    /// Outras modalidades (ex: voucher corporativo, cortesia, permuta ou nota promissória).
    /// </summary>
    Outro = 5
}