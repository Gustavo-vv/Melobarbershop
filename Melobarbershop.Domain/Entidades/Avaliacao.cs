namespace Melobarbershop.Domain.Entidades
{
    public class Avaliacao
    {
        public int Id { get; set; }
        public int AgendamentoId { get; set; }
        public Agendamento Agendamento { get; set; } = null!;

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;

        public int BarbeiroId { get; set; }
        public Barbeiro Barbeiro { get; set; } = null!;

        public int NotaEstrelas { get; set; } // 1 a 5
        public string? Comentario { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}
