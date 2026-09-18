// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Enums/TipoGatilhoMensagem.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem usa:
//   * TemplateMensagem (propriedade Gatilho)
//   * Melobarbershop.Application (NotificacaoService, rotinas agendadas de lembretes e pós-venda)
//   * Melobarbershop.Desktop (Configuração das réguas de automação do WhatsApp)
// ============================================================================

namespace Melobarbershop.Domain.Enums;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Enumeração tipada que classifica os eventos de ciclo de vida do cliente que disparam mensagens automáticas.
/// 
/// POR QUE EXISTE:
/// Vincula os eventos do sistema aos modelos de mensagens correspondentes no CRM,
/// permitindo a automação do fluxo de relacionamento sem intervenção manual do atendente.
/// 
/// O QUE QUEBRARIA SE NÃO EXISTISSE:
/// As rotinas automáticas de background (como envio de lembrete 2h antes) não teriam como saber
/// qual template de texto deve ser carregado e interpolado para cada situação.
/// </summary>
public enum TipoGatilhoMensagem
{
    /// <summary>
    /// Disparado imediatamente após a criação e reserva de um novo agendamento com sucesso.
    /// </summary>
    ConfirmacaoAgendamento = 1,

    /// <summary>
    /// Disparado por serviço em segundo plano com antecedência programada (ex: 2h antes do atendimento).
    /// Reduz drasticamente a taxa de esquecimento e faltas (No-Show).
    /// </summary>
    LembreteHorario = 2,

    /// <summary>
    /// Disparado após a conclusão do serviço convidando o cliente a avaliar o corte e agradecendo a preferência.
    /// </summary>
    AgradecimentoAposServico = 3,

    /// <summary>
    /// Disparado para clientes que não agendam há mais de 30 ou 60 dias (estratégia de retenção e reativação).
    /// </summary>
    ReativacaoClienteInativo = 4,

    /// <summary>
    /// Disparado em massa para comunicados de feriados, novidades ou promoções sazonais da barbearia.
    /// </summary>
    CampanhaMarketing = 5
}