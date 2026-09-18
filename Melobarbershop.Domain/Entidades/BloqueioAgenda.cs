// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Entidades/BloqueioAgenda.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem chama/usa:
//   * Melobarbershop.Application (AgendamentoService, HorarioService para validação de disponibilidade)
//   * Melobarbershop.Infrastructure (BarbeariaDbContext, BloqueioAgendaConfiguration)
//   * Melobarbershop.Desktop / UI (Tela de bloqueio de folga/almoço pelo barbeiro ou gerente)
// - Quem ele referencia:
//   * ApplicationUser (o barbeiro titular da agenda bloqueada)
// ============================================================================

namespace Melobarbershop.Domain.Entidades;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Entidade de domínio que reserva um intervalo de tempo em que o barbeiro não pode receber atendimentos.
/// 
/// POR QUE EXISTE:
/// Permite o gerenciamento de intervalos de almoço, folgas, compromissos pessoais, atestados ou férias,
/// impedindo que a grade pública de agendamento ofereça esses horários aos clientes.
/// 
/// O QUE QUEBRARIA SE NÃO EXISTISSE:
/// Clientes conseguiriam marcar horários durante períodos de ausência do barbeiro,
/// gerando conflitos graves de agenda e cancelamentos indesejados.
/// </summary>
public class BloqueioAgenda
{
    /// <summary>
    /// Identificador único do bloqueio (chave primária).
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Chave estrangeira do profissional (ApplicationUser com Role 'Barbeiro').
    /// </summary>
    public string BarbeiroId { get; set; } = string.Empty;

    /// <summary>
    /// Propriedade de navegação para a entidade do Barbeiro.
    /// </summary>
    public ApplicationUser Barbeiro { get; set; } = null!;

    /// <summary>
    /// Início do intervalo de indisponibilidade.
    /// </summary>
    public DateTime DataHoraInicio { get; set; }

    /// <summary>
    /// Fim do intervalo de indisponibilidade.
    /// </summary>
    public DateTime DataHoraFim { get; set; }

    /// <summary>
    /// Justificativa do bloqueio (ex: 'Horário de Almoço', 'Treinamento', 'Folga Semanal').
    /// </summary>
    public string Motivo { get; set; } = string.Empty;
}