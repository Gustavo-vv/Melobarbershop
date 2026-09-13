using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Application.DTOs;

public class ProdutoDto
{
    public int Id { get; set; }
    public string CodigoBarras { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public decimal PrecoCusto { get; set; }
    public decimal PrecoVenda { get; set; }
    public int EstoqueAtual { get; set; }
    public int EstoqueMinimoAlerta { get; set; }
    public bool Ativo { get; set; }
    public bool EstoqueBaixo => EstoqueAtual <= EstoqueMinimoAlerta;
}

public class CriarProdutoDto
{
    public string CodigoBarras { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public decimal PrecoCusto { get; set; }
    public decimal PrecoVenda { get; set; }
    public int EstoqueInicial { get; set; }
    public int EstoqueMinimoAlerta { get; set; }
}

public class AtualizarProdutoDto
{
    public string CodigoBarras { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public decimal PrecoCusto { get; set; }
    public decimal PrecoVenda { get; set; }
    public int EstoqueMinimoAlerta { get; set; }
    public bool Ativo { get; set; } = true;
}

public class MovimentarEstoqueDto
{
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public TipoMovimentacaoEstoque Tipo { get; set; }
    public string? Observacao { get; set; }
}

public class MovimentacaoEstoqueDto
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public TipoMovimentacaoEstoque Tipo { get; set; }
    public int Quantidade { get; set; }
    public DateTime DataHora { get; set; }
    public string? Observacao { get; set; }
}
