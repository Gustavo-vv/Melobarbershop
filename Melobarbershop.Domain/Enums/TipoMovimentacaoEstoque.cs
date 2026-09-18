// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Enums/TipoMovimentacaoEstoque.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem usa:
//   * MovimentacaoEstoque (propriedade Tipo)
//   * Melobarbershop.Application (EstoqueService, VendaService, relatórios de inventário)
//   * Melobarbershop.Desktop (Módulo de controle de estoque / ajuste manual)
// ============================================================================

namespace Melobarbershop.Domain.Enums;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Enumeração tipada que classifica a natureza operacional de cada movimentação de estoque.
/// 
/// POR QUE EXISTE:
/// Diferencia entradas (reposição por compra) de saídas lucrativas (venda ao cliente),
/// consumo de insumos pelos próprios barbeiros (uso de bancada) e baixas por quebra/vencimento.
/// 
/// O QUE QUEBRARIA SE NÃO EXISTISSE:
/// Seria impossível apurar o Demonstrativo de Resultado (DRE) com clareza, pois produtos consumidos
/// pelos barbeiros no dia a dia seriam confundidos com vendas não pagas ou perdas.
/// </summary>
public enum TipoMovimentacaoEstoque
{
    /// <summary>
    /// Entrada de mercadorias no estoque decorrente de compra com fornecedor ou reposição.
    /// Operação matemática: Soma (+) no saldo de estoque.
    /// </summary>
    Entrada = 1,

    /// <summary>
    /// Saída decorrente de venda direta a um cliente via frente de caixa (PDV).
    /// Operação matemática: Subtração (-) no saldo de estoque com contrapartida financeira.
    /// </summary>
    SaidaVenda = 2,

    /// <summary>
    /// Consumo de produtos da barbearia pelos próprios profissionais durante os atendimentos
    /// (ex: lâminas descartáveis, loção pós-barba de uso comum, toalhas descartáveis).
    /// Operação matemática: Subtração (-) no saldo classificado como custo operacional.
    /// </summary>
    UsoInternoBancada = 3,

    /// <summary>
    /// Baixa manual por extravio, quebra de frasco, furto ou produto fora do prazo de validade.
    /// Operação matemática: Subtração (-) no saldo classificado como prejuízo/perda.
    /// </summary>
    AjustePerda = 4
}