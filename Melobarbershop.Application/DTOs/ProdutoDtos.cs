// ============================================================================
// Arquivo: ProdutoDtos.cs
// Camada: Melobarbershop.Application (Data Transfer Objects - DTOs)
// Objetivo: Definir os contratos de transferência para produtos físicos vendidos
//           na barbearia (pomadas, shampoos, óleos) e suas movimentações de estoque.
// Papel na Arquitetura:
//   - Isola as regras de armazenamento das entidades Produto e MovimentacaoEstoque.
//   - Provê campos computados como EstoqueBaixo para facilitar renderização de badges de alerta na UI.
// ============================================================================

using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Application.DTOs;

/// <summary>
/// DTO de leitura contendo dados cadastrais do produto, preços e situação de estoque.
/// </summary>
public class ProdutoDto
{
    /// <summary>Identificador primário do produto.</summary>
    public int Id { get; set; }

    /// <summary>Código de barras (EAN-13 ou identificador de etiquetadora) para leitura ótica no PDV.</summary>
    public string CodigoBarras { get; set; } = string.Empty;

    /// <summary>Nome comercial do produto.</summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>Custo de aquisição pago ao fornecedor (usado para apuração de margem de lucro).</summary>
    public decimal PrecoCusto { get; set; }

    /// <summary>Preço de venda ao consumidor final no balcão/PDV.</summary>
    public decimal PrecoVenda { get; set; }

    /// <summary>Quantidade física atual disponível em prateleira/depósito.</summary>
    public int EstoqueAtual { get; set; }

    /// <summary>Limite mínimo de unidades antes de disparar alerta de reposição.</summary>
    public int EstoqueMinimoAlerta { get; set; }

    /// <summary>Indica se o produto está ativo para venda e catálogo.</summary>
    public bool Ativo { get; set; }

    /// <summary>
    /// Propriedade computada auxiliar: verdadeiro quando o estoque atual atinge ou fica abaixo do estoque mínimo.
    /// Útil para destaque visual imediato em tabelas e relatórios gerenciais.
    /// </summary>
    public bool EstoqueBaixo => EstoqueAtual <= EstoqueMinimoAlerta;
}

/// <summary>
/// DTO de entrada para cadastrar um novo item no estoque com saldo inicial.
/// </summary>
public class CriarProdutoDto
{
    /// <summary>Código de barras único do produto.</summary>
    public string CodigoBarras { get; set; } = string.Empty;

    /// <summary>Nome do produto.</summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>Preço de custo pago pelo estabelecimento.</summary>
    public decimal PrecoCusto { get; set; }

    /// <summary>Preço de venda ao consumidor.</summary>
    public decimal PrecoVenda { get; set; }

    /// <summary>Saldo físico com o qual o produto inicia no cadastro.</summary>
    public int EstoqueInicial { get; set; }

    /// <summary>Quantidade mínima limite para alerta de compras/reposição.</summary>
    public int EstoqueMinimoAlerta { get; set; }
}

/// <summary>
/// DTO de entrada para atualização de preços e alertas de um produto já cadastrado.
/// Nota de arquitetura: A alteração de quantidade em estoque não ocorre por aqui, mas via movimentações auditadas.
/// </summary>
public class AtualizarProdutoDto
{
    /// <summary>Código de barras atualizado.</summary>
    public string CodigoBarras { get; set; } = string.Empty;

    /// <summary>Nome do produto.</summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>Preço de custo atualizado.</summary>
    public decimal PrecoCusto { get; set; }

    /// <summary>Preço de venda atualizado.</summary>
    public decimal PrecoVenda { get; set; }

    /// <summary>Novo valor de estoque mínimo para disparo de alerta.</summary>
    public int EstoqueMinimoAlerta { get; set; }

    /// <summary>Status de disponibilidade do produto.</summary>
    public bool Ativo { get; set; } = true;
}

/// <summary>
/// DTO de comando para registrar entrada manual, perda, avaria ou ajuste de inventário.
/// </summary>
public class MovimentarEstoqueDto
{
    /// <summary>Identificador do produto movimentado.</summary>
    public int ProdutoId { get; set; }

    /// <summary>Quantidade de unidades adicionadas ou retiradas.</summary>
    public int Quantidade { get; set; }

    /// <summary>Tipo de movimentação (Entrada, Saida, Ajuste, Perda).</summary>
    public TipoMovimentacaoEstoque Tipo { get; set; }

    /// <summary>Justificativa ou nota fiscal referente ao ajuste.</summary>
    public string? Observacao { get; set; }
}

/// <summary>
/// DTO de leitura para histórico/log de auditoria de movimentações de estoque.
/// </summary>
public class MovimentacaoEstoqueDto
{
    /// <summary>Identificador do registro de movimentação.</summary>
    public int Id { get; set; }

    /// <summary>Identificador do produto.</summary>
    public int ProdutoId { get; set; }

    /// <summary>Nome do produto (preenchido via mapeamento/join para facilitar visualização sem roundtrips adicionais).</summary>
    public string NomeProduto { get; set; } = string.Empty;

    /// <summary>Tipo de operação realizada.</summary>
    public TipoMovimentacaoEstoque Tipo { get; set; }

    /// <summary>Quantidade movimentada no evento.</summary>
    public int Quantidade { get; set; }

    /// <summary>Data e hora do registro.</summary>
    public DateTime DataHora { get; set; }

    /// <summary>Observação ou justificativa anotada.</summary>
    public string? Observacao { get; set; }
}

