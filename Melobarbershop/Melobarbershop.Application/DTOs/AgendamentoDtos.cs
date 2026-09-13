using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Application.DTOs;

public class AgendamentoDto
{
    public int Id { get; set; }
    public string ClienteId { get; set; } = string.Empty;
    public string NomeCliente { get; set; } = string.Empty;
    public string? TelefoneCliente { get; set; }
    
    public string BarbeiroId { get; set; } = string.Empty;
    public string NomeBarbeiro { get; set; } = string.Empty;

    public DateTime DataHoraInicio { get; set; }
    public DateTime DataHoraFim { get; set; }
    public StatusAgendamento Status { get; set; }
    public OrigemAgendamento Origem { get; set; }
    public string? Observacoes { get; set; }
    public DateTime DataCriacao { get; set; }
    
    public decimal ValorTotal { get; set; }
    public ICollection<AgendamentoItemDto> Itens { get; set; } = new List<AgendamentoItemDto>();
}

public class AgendamentoItemDto
{
    public int Id { get; set; }
    public int ServicoId { get; set; }
    public string NomeServico { get; set; } = string.Empty;
    public decimal PrecoCobrado { get; set; }
}

public class CriarAgendamentoDto
{
    public string ClienteId { get; set; } = string.Empty;
    public string BarbeiroId { get; set; } = string.Empty;
    public DateTime DataHoraInicio { get; set; }
    public ICollection<int> ServicoIds { get; set; } = new List<int>();
    public OrigemAgendamento Origem { get; set; } = OrigemAgendamento.Site;
    public string? Observacoes { get; set; }
}

public class ReagendarAgendamentoDto
{
    public DateTime NovoDataHoraInicio { get; set; }
    public string? NovoBarbeiroId { get; set; }
}
