

using Barbearia.Domain.Enums;

namespace Melobarbershop.Domain.Entidades;

public class Agendamento
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;

    public int BarbeiroId { get; set; }
    public Barbeiro Barbeiro { get; set; } = null!;

    public DateTime DataHoraInicio { get; set; }
    public DateTime DataHoraFim { get; set; }

    public StatusAgendamento Status { get; set; } = StatusAgendamento.Pendente;
    public OrigemAgendamento Origem { get; set; } = OrigemAgendamento.Site;
    public string? Observacoes { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public ICollection<AgendamentoItem> Itens { get; set; } = new List<AgendamentoItem>();
    public Avaliacao? Avaliacao { get; set; }
}
