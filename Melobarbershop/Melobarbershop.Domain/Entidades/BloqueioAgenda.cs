namespace Melobarbershop.Domain.Entidades;

public class BloqueioAgenda
{
    public int Id { get; set; }
    
    public string BarbeiroId { get; set; } = string.Empty;
    public ApplicationUser Barbeiro { get; set; } = null!;

    public DateTime DataHoraInicio { get; set; }
    public DateTime DataHoraFim { get; set; }
    public string Motivo { get; set; } = string.Empty;
}