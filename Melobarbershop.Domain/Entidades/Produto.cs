// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Entidades/Produto.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem chama/usa:
//   * Melobarbershop.Application (ProdutoService, VendaService, DTOs de Produto)
//   * Melobarbershop.Infrastructure (BarbeariaDbContext, ProdutoConfiguration, ProdutoRepository)
//   * Melobarbershop.Desktop (Controle de Estoque, PDV de produtos físicos)
// - Quem ele referencia:
//   * MovimentacaoEstoque (coleção de auditoria de estoque 1:N)
// ============================================================================

namespace Melobarbershop.Domain.Entidades
{
    /// <summary>
    /// PAPEL ARQUITETURAL:
    /// Entidade raiz de agregação que representa mercadorias físicas comercializadas pela barbearia.
    /// 
    /// POR QUE EXISTE:
    /// Controla produtos de revenda (ex: pomadas modeladoras, óleos de barba, shampoos especiais, bebidas),
    /// seus custos de aquisição, margens de lucro de venda e saldo em estoque.
    /// 
    /// O QUE QUEBRARIA SE NÃO EXISTISSE:
    /// O módulo de Frente de Caixa (PDV) não permitiria venda cruzada de produtos durante o atendimento,
    /// e a barbearia não teria gestão automatizada de inventário.
    /// </summary>
    public class Produto
    {
        /// <summary>
        /// Identificador único do produto (chave primária).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Código de barras EAN-13 ou código interno alfanumérico para leitura ótica no PDV.
        /// </summary>
        public string CodigoBarras { get; set; } = string.Empty;

        /// <summary>
        /// Nome comercial e apresentação da mercadoria (ex: "Pomada Efeito Matte 100g").
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Custo unitário de compra pago ao fornecedor.
        /// Utilizado no cálculo de margem de contribuição e lucratividade líquida da barbearia.
        /// </summary>
        public decimal PrecoCusto { get; set; }

        /// <summary>
        /// Preço de venda ao consumidor final no balcão da barbearia.
        /// </summary>
        public decimal PrecoVenda { get; set; }

        /// <summary>
        /// Saldo físico disponível atualmente nas prateleiras e depósito.
        /// Regra de negócio: Atualizado a cada venda (baixa) ou entrada de nota fiscal (reposição).
        /// </summary>
        public int EstoqueAtual { get; set; }

        /// <summary>
        /// Ponto de pedido / gatilho de ressuprimento.
        /// Regra de negócio: Quando EstoqueAtual <= EstoqueMinimoAlerta, o sistema emite alerta visual de estoque baixo.
        /// </summary>
        public int EstoqueMinimoAlerta { get; set; }

        /// <summary>
        /// Flag de exclusão lógica (soft-delete).
        /// Mercadorias inativas não podem ser selecionadas em novas vendas.
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// Histórico cronológico de todas as entradas, saídas, perdas e ajustes deste produto.
        /// </summary>
        public ICollection<MovimentacaoEstoque> Movimentacoes { get; set; } = new List<MovimentacaoEstoque>();
    }
}
