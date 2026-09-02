namespace Melobarbershop.Domain.Entidades
{
    public class Pacote
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty; // Ex: "Combo Barba + Cabelo"
        public decimal PrecoTotal { get; set; }
        public bool Ativo { get; set; } = true;
        public ICollection<PacoteItem> Itens { get; set; } = new List<PacoteItem>();
    }
}