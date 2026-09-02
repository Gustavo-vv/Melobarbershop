namespace Melobarbershop.Domain.Entidades;

public class VendaItem
{
    public int Id { get; set; }
    public int VendaId { get; set; }
    public Venda Venda { get; set; } = null!;

    public int? ServicoId { get; set; }
    public Servico? Servico { get; set; }

    public int? ProdutoId { get; set; }
    public Produto? Produto { get; set; }

    public string? BarbeiroId { get; set; } // Barbeiro (ApplicationUser) que executou o serviço ou vendeu o produto
    public ApplicationUser? Barbeiro { get; set; }

    public int Quantidade { get; set; } = 1;
    public decimal PrecoUnitario { get; set; }

    public decimal ValorTotal => Quantidade * PrecoUnitario;
}
