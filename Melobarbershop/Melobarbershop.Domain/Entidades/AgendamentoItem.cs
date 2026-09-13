namespace Melobarbershop.Domain.Entidades
{
    public class AgendamentoItem
    {
        public int Id { get; set; }
        public int AgendamentoId { get; set; }
        public Agendamento Agendamento { get; set; } = null!;

        public int ServicoId { get; set; }
        public Servico Servico { get; set; } = null!;

        public decimal PrecoCobrado { get; set; }
    }
}
