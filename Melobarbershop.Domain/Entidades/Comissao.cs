namespace Melobarbershop.Domain.Entidades
{
    public class Comissao
    {
        public int Id { get; set; }
        public int BarbeiroId { get; set; }
        public Barbeiro Barbeiro { get; set; } = null!;

        public int VendaItemId { get; set; }
        public VendaItem VendaItem { get; set; } = null!;

        public decimal ValorComissao { get; set; }
        public bool PagoAoBarbeiro { get; set; } = false;
        public DateTime? DataPagamentoComissao { get; set; }
    }
}
