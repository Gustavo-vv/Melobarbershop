namespace Melobarbershop.Domain.Entidades
{
    public class Venda
    {
        public int Id { get; set; }
        public int? AgendamentoId { get; set; }
        public Agendamento? Agendamento { get; set; }

        public int? ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        public DateTime DataHora { get; set; } = DateTime.UtcNow;
        public decimal ValorSubtotal { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorFinal { get; set; }
        public ICollection<VendaItem> Itens { get; set; } = new List<VendaItem>();
        public ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
    }
}
