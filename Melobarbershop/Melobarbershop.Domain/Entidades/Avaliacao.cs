namespace Melobarbershop.Domain.Entidades;

public class Avaliacao
{
    public int Id { get; set; }
    
    public int AgendamentoId { get; set; }
    public Agendamento Agendamento { get; set; } = null!;

    public string ClienteId { get; set; } = string.Empty;
    public ApplicationUser Cliente { get; set; } = null!;

    public string BarbeiroId { get; set; } = string.Empty;
    public ApplicationUser Barbeiro { get; set; } = null!;

    public int NotaEstrelas { get; set; } // 1 a 5
    public string? Comentario { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
}
