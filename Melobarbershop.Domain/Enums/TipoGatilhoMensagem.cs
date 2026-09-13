namespace Melobarbershop.Domain.Enums;

/// <summary>
/// Enumeração que define os tipos de gatilho para envio automático de mensagens.
/// </summary>
public enum TipoGatilhoMensagem
{
    ConfirmacaoAgendamento = 1,
    LembreteHorario = 2,
    AgradecimentoAposServico = 3,
    ReativacaoClienteInativo = 4,
    CampanhaMarketing = 5
}