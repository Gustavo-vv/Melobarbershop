// ============================================================================
// Arquivo: VendaDtos.cs
// Camada: Melobarbershop.Application (Data Transfer Objects - DTOs)
// Objetivo: Definir os contratos de transferência de dados para o módulo de Frente de Caixa (PDV),
//           abrangendo fechamento de comanda, adição de itens e registro de pagamentos.
// Papel na Arquitetura:
//   - Isola as entidades Venda, VendaItem e Pagamento de chamadas diretas pela UI ou API.
//   - Fornece campos calculados (SaldoRestante, EstaTotalmentePaga) para orientar o operador
//     se a conta pode ser finalizada ou se ainda requer mais formas de pagamento.
// ============================================================================

using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Application.DTOs;

/// <summary>
/// DTO de leitura detalhado representando uma venda/comanda aberta ou concluída no PDV.
/// </summary>
public class VendaDto
{
    /// <summary>Identificador único da venda na base de dados.</summary>
    public int Id { get; set; }

    /// <summary>Identificador do agendamento de origem (se a venda foi iniciada a partir da agenda).</summary>
    public int? AgendamentoId { get; set; }

    /// <summary>Identificador do cliente (se identificado no momento da compra).</summary>
    public string? ClienteId { get; set; }

    /// <summary>Nome do cliente (obtido via join no repositório para evitar queries extras).</summary>
    public string? NomeCliente { get; set; }

    /// <summary>Data e hora do registro/abertura da venda.</summary>
    public DateTime DataHora { get; set; }

    /// <summary>Soma total dos itens da venda antes de descontos.</summary>
    public decimal ValorSubtotal { get; set; }

    /// <summary>Valor de desconto aplicado na comanda/venda.</summary>
    public decimal ValorDesconto { get; set; }

    /// <summary>Valor líquido a ser pago pelo cliente (ValorSubtotal - ValorDesconto).</summary>
    public decimal ValorFinal { get; set; }

    /// <summary>Valor total já recebido somando todos os pagamentos parciais ou totais.</summary>
    public decimal ValorPago { get; set; }

    /// <summary>
    /// Propriedade computada com o saldo ainda pendente de quitação.
    /// </summary>
    public decimal SaldoRestante => ValorFinal - ValorPago;

    /// <summary>
    /// Propriedade computada que indica se o total liquidado cobre o valor final da comanda.
    /// </summary>
    public bool EstaTotalmentePaga => ValorPago >= ValorFinal;

    /// <summary>Coleção dos itens da venda (serviços executados e produtos comprados).</summary>
    public ICollection<VendaItemDto> Itens { get; set; } = new List<VendaItemDto>();

    /// <summary>Coleção dos pagamentos registrados para esta comanda.</summary>
    public ICollection<PagamentoDto> Pagamentos { get; set; } = new List<PagamentoDto>();
}

/// <summary>
/// DTO de leitura de um item específico incluído na venda (seja serviço ou produto).
/// </summary>
public class VendaItemDto
{
    /// <summary>Identificador do item na comanda.</summary>
    public int Id { get; set; }

    /// <summary>Identificador do serviço prestado (se aplicável).</summary>
    public int? ServicoId { get; set; }

    /// <summary>Nome do serviço prestado.</summary>
    public string? NomeServico { get; set; }

    /// <summary>Identificador do produto vendido (se aplicável).</summary>
    public int? ProdutoId { get; set; }

    /// <summary>Nome do produto comercializado.</summary>
    public string? NomeProduto { get; set; }

    /// <summary>Identificador do barbeiro que prestou o serviço ou efetuou a indicação do produto.</summary>
    public string? BarbeiroId { get; set; }

    /// <summary>Nome do barbeiro responsável para cálculo de comissões.</summary>
    public string? NomeBarbeiro { get; set; }

    /// <summary>Quantidade de unidades/execuções do item.</summary>
    public int Quantidade { get; set; }

    /// <summary>Preço unitário cobrado no momento do registro da venda.</summary>
    public decimal PrecoUnitario { get; set; }

    /// <summary>Subtotal deste item (Quantidade * PrecoUnitario).</summary>
    public decimal ValorTotal { get; set; }
}

/// <summary>
/// DTO de leitura de um pagamento lançado para abater o valor da comanda.
/// </summary>
public class PagamentoDto
{
    /// <summary>Identificador do registro de pagamento.</summary>
    public int Id { get; set; }

    /// <summary>Identificador da comanda/venda a qual o pagamento pertence.</summary>
    public int VendaId { get; set; }

    /// <summary>Forma de pagamento utilizada (Dinheiro, Pix, Cartão de Crédito, Débito).</summary>
    public FormaPagamento Forma { get; set; }

    /// <summary>Valor monetário liquidado nesta transação.</summary>
    public decimal Valor { get; set; }

    /// <summary>Data e hora em que a transação foi aprovada e registrada.</summary>
    public DateTime DataHora { get; set; }
}

/// <summary>
/// DTO de comando para abertura de uma nova comanda/venda no PDV.
/// </summary>
public class IniciarVendaDto
{
    /// <summary>Identificador do cliente (opcional; comanda anônima/balcão pode ser nula).</summary>
    public string? ClienteId { get; set; }

    /// <summary>Identificador do agendamento vinculado, caso esteja finalizando uma sessão da agenda.</summary>
    public int? AgendamentoId { get; set; }
}

/// <summary>
/// DTO de comando para anexar um serviço executado a uma comanda aberta.
/// </summary>
public class AdicionarItemServicoDto
{
    /// <summary>Identificador do serviço a ser inserido.</summary>
    public int ServicoId { get; set; }

    /// <summary>Identificador do barbeiro responsável pelo serviço (para crédito de comissão).</summary>
    public string? BarbeiroId { get; set; }

    /// <summary>Preço customizado (caso haja negociação ou cortesia que divirja da tabela padrão).</summary>
    public decimal? PrecoCustomizado { get; set; }
}

/// <summary>
/// DTO de comando para anexar um produto físico do estoque a uma comanda aberta.
/// </summary>
public class AdicionarItemProdutoDto
{
    /// <summary>Identificador do produto vendido.</summary>
    public int ProdutoId { get; set; }

    /// <summary>Quantidade de itens a serem debitados do estoque (padrão 1).</summary>
    public int Quantidade { get; set; } = 1;

    /// <summary>Barbeiro/atendente comissionado pela venda.</summary>
    public string? BarbeiroId { get; set; }

    /// <summary>Preço negociado opcional.</summary>
    public decimal? PrecoCustomizado { get; set; }
}

/// <summary>
/// DTO de comando para lançar um pagamento parcial ou total em uma comanda aberta.
/// </summary>
public class RegistrarPagamentoDto
{
    /// <summary>Método selecionado pelo cliente (Dinheiro, Pix, etc.).</summary>
    public FormaPagamento Forma { get; set; }

    /// <summary>Quantia a ser abatida da venda.</summary>
    public decimal Valor { get; set; }
}

