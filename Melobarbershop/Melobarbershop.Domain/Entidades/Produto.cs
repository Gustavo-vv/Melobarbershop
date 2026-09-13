namespace Melobarbershop.Domain.Entidades
{
    public class Produto
    {
        public int Id { get; set; }
        public string CodigoBarras { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty; // Ex: "Pomada Matte 100g"
        public decimal PrecoCusto { get; set; }
        public decimal PrecoVenda { get; set; }
        public int EstoqueAtual { get; set; }
        public int EstoqueMinimoAlerta { get; set; }
        public bool Ativo { get; set; } = true;
        public ICollection<MovimentacaoEstoque> Movimentacoes { get; set; } = new List<MovimentacaoEstoque>();
    }
}
