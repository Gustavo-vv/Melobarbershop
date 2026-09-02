namespace Melobarbershop.Domain.Entidades
{
    public class MovimentacaoEstoque
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; } = null!;

        public int Quantidade { get; set; }
        public DateTime DataHora { get; set; } = DateTime.UtcNow;
        public string? Observacao { get; set; }
    }
}
