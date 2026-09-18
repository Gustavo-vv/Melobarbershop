// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Entidades/MovimentacaoEstoque.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem chama/usa:
//   * Melobarbershop.Application (ProdutoService, VendaService para baixa e reposição)
//   * Melobarbershop.Infrastructure (BarbeariaDbContext, MovimentacaoEstoqueConfiguration)
//   * Melobarbershop.Desktop (Módulo de controle de estoque e inventário)
// - Quem ele referencia:
//   * Produto (chave estrangeira e navegação)
//   * TipoMovimentacaoEstoque (Enum que categoriza Entrada, Saída, Ajuste, etc.)
// ============================================================================

using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Domain.Entidades;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Entidade de auditoria contábil de estoque (ledger de inventário).
/// 
/// POR QUE EXISTE:
/// Garante que nenhuma alteração na quantidade física de um produto ocorra sem um registro histórico
/// de justificativa (quem movimentou, quando, qual a quantidade e o tipo).
/// 
/// O QUE QUEBRARIA SE NÃO EXISTISSE:
/// O estoque seria uma coluna que apenas sobreescreve valores, impedindo a rastreabilidade
/// de perdas, desvios, compras de fornecedores ou saídas por venda.
/// </summary>
public class MovimentacaoEstoque
{
    /// <summary>
    /// Identificador único da movimentação (chave primária).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Chave estrangeira do produto movimentado.
    /// </summary>
    public int ProdutoId { get; set; }

    /// <summary>
    /// Propriedade de navegação para o Produto afetado.
    /// </summary>
    public Produto Produto { get; set; } = null!;

    /// <summary>
    /// Classificação da movimentação (Entrada, SaidaVenda, Perda, AjusteInventario).
    /// </summary>
    public TipoMovimentacaoEstoque Tipo { get; set; }

    /// <summary>
    /// Quantidade de unidades adicionadas ou subtraídas.
    /// </summary>
    public int Quantidade { get; set; }

    /// <summary>
    /// Timestamp exato do registro da movimentação (sempre em UTC).
    /// </summary>
    public DateTime DataHora { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Descrição detalhada ou motivo da movimentação (ex: 'Venda balcão #1042', 'Nota fiscal 4589', 'Produto vencido descartado').
    /// </summary>
    public string? Observacao { get; set; }
}
