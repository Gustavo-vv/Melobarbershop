// ============================================================================
// ARQUIVO: Melobarbershop.Application/DTOs/AgendamentoDtos.cs
// CAMADA: Application (Contratos e DTOs)
// CONEXÕES ARQUITETURAIS:
// - Quem consome: Melobarbershop.API (AgendamentosController), Melobarbershop.UI, Melobarbershop.Desktop
// - Quem mapeia: MappingProfile.cs (AutoMapper transforma Agendamento <-> AgendamentoDto)
// ============================================================================

using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Application.DTOs;

/// <summary>
/// PAPEL ARQUITETURAL:
/// DTO de saída detalhado (Response Model) para exibição de agendamentos.
/// 
/// POR QUE EXISTE:
/// Desacopla as entidades de banco (Agendamento) das camadas de apresentação, já entregando campos desnormalizados
/// convenientes (ex: NomeCliente, TelefoneCliente, NomeBarbeiro, ValorTotal somado) sem que a UI precise
/// navegar em árvores relacionais profundas do EF Core.
/// </summary>
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
    
    /// <summary>
    /// Soma calculada de todos os PrecoCobrado dos itens do agendamento.
    /// </summary>
    public decimal ValorTotal { get; set; }

    /// <summary>
    /// Lista dos serviços contratados neste horário.
    /// </summary>
    public ICollection<AgendamentoItemDto> Itens { get; set; } = new List<AgendamentoItemDto>();
}

/// <summary>
/// DTO de exibição de um serviço individual contido no agendamento.
/// </summary>
public class AgendamentoItemDto
{
    public int Id { get; set; }
    public int ServicoId { get; set; }
    public string NomeServico { get; set; } = string.Empty;
    public decimal PrecoCobrado { get; set; }
}

/// <summary>
/// DTO de entrada (Request Model) para solicitação de uma nova reserva de horário.
/// Recebe apenas os identificadores e a data de início desejada; o cálculo de término
/// e o congelamento dos preços são resolvidos pelo AgendamentoService no backend.
/// </summary>
public class CriarAgendamentoDto
{
    public string ClienteId { get; set; } = string.Empty;
    public string BarbeiroId { get; set; } = string.Empty;
    public DateTime DataHoraInicio { get; set; }
    public ICollection<int> ServicoIds { get; set; } = new List<int>();
    public OrigemAgendamento Origem { get; set; } = OrigemAgendamento.Site;
    public string? Observacoes { get; set; }
}

/// <summary>
/// DTO de entrada para reagendamento de um horário já existente.
/// </summary>
public class ReagendarAgendamentoDto
{
    public DateTime NovoDataHoraInicio { get; set; }
    public string? NovoBarbeiroId { get; set; }
}

/// <summary>
/// DTO de resposta que representa uma fração de horário na grade (slot)
/// e sua respectiva disponibilidade para marcação pelo cliente.
/// </summary>
public class HorarioSlotDto
{
    public DateTime Horario { get; set; }
    public bool Disponivel { get; set; }
}
