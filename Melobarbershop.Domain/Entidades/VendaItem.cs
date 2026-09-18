// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Entidades/VendaItem.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem chama/usa:
//   * Venda (agregação pai 1:N)
//   * Melobarbershop.Application (ComissaoService, VendaService, EstoqueService)
//   * Melobarbershop.Infrastructure (BarbeariaDbContext, VendaItemConfiguration)
//   * Melobarbershop.Desktop (Grid de itens do cupom fiscal / Frente de Caixa)
// - Quem ele referencia:
//   * Venda (venda pai)
//   * Servico (procedimento prestado, se for serviço)
//   * Produto (mercadoria entregue, se for produto físico)
//   * ApplicationUser (barbeiro que executou ou vendeu, para fins de comissão)
// ============================================================================

namespace Melobarbershop.Domain.Entidades;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Entidade polimórfica/híbrida de linha de cupom fiscal/venda.
/// 
/// POR QUE EXISTE:
/// Permite faturar tanto Serviços quanto Produtos em um único carrinho de compras / comanda,
/// registrando qual Barbeiro foi o responsável pela execução ou venda daquele item específico
/// para cálculo de comissões individualizadas.
/// 
/// O QUE QUEBRARIA SE NÃO EXISTISSE:
/// O cálculo de comissão de barbeiros sobre serviços específicos e produtos vendidos
/// não teria como discriminar quem realizou qual item da comanda.
/// </summary>
public class VendaItem
{
    /// <summary>
    /// Identificador único da linha de item da venda (chave primária).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Chave estrangeira da Venda pai.
    /// </summary>
    public int VendaId { get; set; }

    /// <summary>
    /// Propriedade de navegação para a Venda correspondente.
    /// </summary>
    public Venda Venda { get; set; } = null!;

    /// <summary>
    /// Chave estrangeira opcional do serviço faturado (preenchido se o item for um procedimento).
    /// </summary>
    public int? ServicoId { get; set; }

    /// <summary>
    /// Propriedade de navegação para o Serviço correspondente.
    /// </summary>
    public Servico? Servico { get; set; }

    /// <summary>
    /// Chave estrangeira opcional do produto físico (preenchido se o item for mercadoria de revenda).
    /// </summary>
    public int? ProdutoId { get; set; }

    /// <summary>
    /// Propriedade de navegação para o Produto correspondente.
    /// </summary>
    public Produto? Produto { get; set; }

    /// <summary>
    /// Chave estrangeira opcional do barbeiro responsável por este item.
    /// Regra de negócio: Fundamental para o repasse de comissão individual daquele profissional.
    /// </summary>
    public string? BarbeiroId { get; set; }

    /// <summary>
    /// Propriedade de navegação para o Barbeiro comissionado.
    /// </summary>
    public ApplicationUser? Barbeiro { get; set; }

    /// <summary>
    /// Quantidade de unidades vendidas ou execuções do serviço.
    /// Padrão: 1 unidade.
    /// </summary>
    public int Quantidade { get; set; } = 1;

    /// <summary>
    /// Preço unitário cobrado no momento do fechamento da venda.
    /// Regra de negócio: Snapshot financeiro imutável para não sofrer impacto de alterações futuras na tabela de preços.
    /// </summary>
    public decimal PrecoUnitario { get; set; }

    /// <summary>
    /// Propriedade calculada em memória que representa o subtotal da linha (Quantidade * PrecoUnitario).
    /// Nota: Não persistida diretamente como coluna de tabela, avaliada sob demanda pelo modelo C#.
    /// </summary>
    public decimal ValorTotal => Quantidade * PrecoUnitario;
}
