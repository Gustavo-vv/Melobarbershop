namespace Melobarbershop.Domain.Entidades
{
    public class BloqueioAgenda
    {
        public int Id { get; set; }
        public int BarbeiroId { get; set; }
        public Barbeiro Barbeiro { get; set; } = null!;

        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public string Motivo { get; set; } = string.Empty; // Ex: "Férias", "Consulta médica"
    }
}