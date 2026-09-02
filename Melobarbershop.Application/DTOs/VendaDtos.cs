using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Application.DTOs;

public class VendaDto
{
    public int Id { get; set; }
    public int? AgendamentoId { get; set; }
    public string? ClienteId { get; set; }
    public string? NomeCliente { get; set; }
    public DateTime DataHora { get; set; }
    public decimal ValorSubtotal { get; set; }
    public decimal ValorDesconto { get; set; }
    public decimal ValorFinal { get; set; }
    public decimal ValorPago { get; set; }
    public decimal SaldoRestante => ValorFinal - ValorPago;
    public bool EstaTotalmentePaga => ValorPago >= ValorFinal;

    public ICollection<VendaItemDto> Itens { get; set; } = new List<VendaItemDto>();
    public ICollection<PagamentoDto> Pagamentos { get; set; } = new List<PagamentoDto>();
}

public class VendaItemDto
{
    public int Id { get; set; }
    public int? ServicoId { get; set; }
    public string? NomeServico { get; set; }
    public int? ProdutoId { get; set; }
    public string? NomeProduto { get; set; }
    public string? BarbeiroId { get; set; }
    public string? NomeBarbeiro { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal ValorTotal { get; set; }
}

public class PagamentoDto
{
    public int Id { get; set; }
    public int VendaId { get; set; }
    public FormaPagamento Forma { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataHora { get; set; }
}

public class IniciarVendaDto
{
    public string? ClienteId { get; set; }
    public int? AgendamentoId { get; set; }
}

public class AdicionarItemServicoDto
{
    public int ServicoId { get; set; }
    public string? BarbeiroId { get; set; }
    public decimal? PrecoCustomizado { get; set; }
}

public class AdicionarItemProdutoDto
{
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; } = 1;
    public string? BarbeiroId { get; set; }
    public decimal? PrecoCustomizado { get; set; }
}

public class RegistrarPagamentoDto
{
    public FormaPagamento Forma { get; set; }
    public decimal Valor { get; set; }
}
