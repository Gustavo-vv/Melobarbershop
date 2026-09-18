// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Enums/StatusAgendamento.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem usa:
//   * Agendamento (propriedade Status)
//   * Melobarbershop.Application (AgendamentoService para transições de estado)
//   * Melobarbershop.Infrastructure (Filtros de conflito de agenda: apenas agendamentos não-cancelados bloqueiam horário)
//   * Melobarbershop.UI e Desktop (Cores de badges na grade visual da agenda)
// ============================================================================

namespace Melobarbershop.Domain.Enums;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Máquina de estados oficial do ciclo de vida de um Agendamento.
/// 
/// POR QUE EXISTE:
/// Controla as transições válidas de um atendimento (ex: não é possível Concluir um agendamento já Cancelado),
/// além de orientar as regras de disponibilidade de horário e faturamento.
/// 
/// O QUE QUEBRARIA SE NÃO EXISTISSE:
/// Sem esses estados, horários cancelados continuariam ocupando a grade dos barbeiros,
/// e relatórios de no-show (não comparecimento) e taxa de ocupação seriam impossíveis de calcular.
/// </summary>
public enum StatusAgendamento
{
    /// <summary>
    /// Agendamento solicitado pelo cliente aguardando confirmação ou pagamento inicial.
    /// </summary>
    Pendente = 1,

    /// <summary>
    /// Agendamento aprovado e reservado com exclusividade na agenda do barbeiro.
    /// </summary>
    Confirmado = 2,

    /// <summary>
    /// Cliente já se encontra na cadeira da barbearia sendo atendido no momento.
    /// </summary>
    EmAtendimento = 3,

    /// <summary>
    /// Procedimento concluído com sucesso e pronto para envio de pesquisa de satisfação (Avaliação).
    /// </summary>
    Concluido = 4,

    /// <summary>
    /// Agendamento cancelado pelo cliente ou pela administração. O horário é liberado imediatamente para novas reservas.
    /// </summary>
    Cancelado = 5,

    /// <summary>
    /// Cliente não compareceu ao horário marcado (No-Show) sem aviso prévio.
    /// Importante para métricas de assiduidade de clientes.
    /// </summary>
    NaoCompareceu = 6
}
