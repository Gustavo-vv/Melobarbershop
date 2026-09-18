// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Entidades/Venda.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem chama/usa:
//   * Melobarbershop.Application (VendaService, CaixaService, RelatoriosFinanceiros, DTOs de Venda)
//   * Melobarbershop.Infrastructure (BarbeariaDbContext, VendaConfiguration, VendaRepository)
//   * Melobarbershop.Desktop (Frente de Caixa / PDV, emissão de comanda e fechamento de conta)
// - Quem ele referencia:
//   * Agendamento (vinculo opcional 1:1 com a comanda de serviço)
//   * ApplicationUser (cliente opcional cadastrado)
//   * VendaItem (itens comercializados 1:N)
//   * Pagamento (parcelas/formas de quitação 1:N)
// ============================================================================

namespace Melobarbershop.Domain.Entidades;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Entidade raiz de agregação do subsistema financeiro e comercial (PDV / Caixa).
/// 
/// POR QUE EXISTE:
/// Consolida o fechamento financeiro de uma transação comercial, integrando serviços realizados
/// (vindos ou não de um Agendamento prévio), produtos físicos adquiridos e os pagamentos realizados.
/// 
/// O QUE QUEBRARIA SE NÃO EXISTISSE:
/// A barbearia não teria controle de faturamento, fluxo de caixa diário,
/// cálculo de ticket médio nem fechamento de comanda.
/// </summary>
public class Venda
{
    /// <summary>
    /// Identificador único da transação de venda / comanda (chave primária).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Chave estrangeira opcional do agendamento que deu origem a esta venda.
    /// É opcional porque uma venda pode ser puramente avulsa de produtos de balcão (sem agendamento).
    /// </summary>
    public int? AgendamentoId { get; set; }

    /// <summary>
    /// Propriedade de navegação para o Agendamento vinculado.
    /// </summary>
    public Agendamento? Agendamento { get; set; }

    /// <summary>
    /// Chave estrangeira opcional do cliente cadastrado.
    /// É opcional para permitir compras avulsas por clientes não cadastrados ("Consumidor Final").
    /// </summary>
    public string? ClienteId { get; set; }

    /// <summary>
    /// Propriedade de navegação para a conta do Cliente (quando identificado).
    /// </summary>
    public ApplicationUser? Cliente { get; set; }

    /// <summary>
    /// Data e hora exata do fechamento da venda (em UTC).
    /// </summary>
    public DateTime DataHora { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Soma bruta de todos os itens (serviços e produtos) antes de eventuais descontos.
    /// </summary>
    public decimal ValorSubtotal { get; set; }

    /// <summary>
    /// Valor monetário de desconto concedido na transação (promoção, cupom ou cortesia).
    /// </summary>
    public decimal ValorDesconto { get; set; }

    /// <summary>
    /// Valor líquido a ser pago pelo cliente (ValorFinal = ValorSubtotal - ValorDesconto).
    /// Regra de negócio: A soma dos Pagamentos associados deve igualar este valor para quitar a venda.
    /// </summary>
    public decimal ValorFinal { get; set; }

    /// <summary>
    /// Lista dos itens vendidos (serviços prestados e produtos entregues).
    /// </summary>
    public ICollection<VendaItem> Itens { get; set; } = new List<VendaItem>();

    /// <summary>
    /// Lista dos pagamentos efetuados para quitar esta venda (suporta pagamentos múltiplos / divididos).
    /// </summary>
    public ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
}
