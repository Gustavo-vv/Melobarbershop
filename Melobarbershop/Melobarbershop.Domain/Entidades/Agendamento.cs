using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Domain.Entidades;

public class Agendamento
{
    public int Id { get; set; }
    
    public string ClienteId { get; set; } = string.Empty;
    public ApplicationUser Cliente { get; set; } = null!;

    public string BarbeiroId { get; set; } = string.Empty;
    public ApplicationUser Barbeiro { get; set; } = null!;

    public DateTime DataHoraInicio { get; set; }
    public DateTime DataHoraFim { get; set; }

    public StatusAgendamento Status { get; set; } = StatusAgendamento.Pendente;
    public OrigemAgendamento Origem { get; set; } = OrigemAgendamento.Site;
    public string? Observacoes { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    public ICollection<AgendamentoItem> Itens { get; set; } = new List<AgendamentoItem>();
    public Avaliacao? Avaliacao { get; set; }
}
