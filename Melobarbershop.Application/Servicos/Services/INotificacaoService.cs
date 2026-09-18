// ============================================================================
// Arquivo: INotificacaoService.cs
// Camada: Melobarbershop.Application (Serviços - Contratos)
// Objetivo: Definir os contratos para envio de mensagens, lembretes de agenda
//           e campanhas promocionais via canais externos (WhatsApp/SMS).
// Papel na Arquitetura:
//   - Interface que abstrai o provedor de mensageria externa (ex: Z-API, Twilio, Evolution API).
//   - Dispara gatilhos automáticos a partir de eventos do ciclo de vida de agendamentos.
// ============================================================================

namespace Melobarbershop.Application.Servicos.Services;

/// <summary>
/// Contrato do serviço de mensageria e notificações aos clientes e barbeiros.
/// </summary>
public interface INotificacaoService
{
    /// <summary>Envia uma mensagem de texto direta para um número de telefone informado.</summary>
    Task<bool> EnviarAsync(string numeroDestino, string mensagem, string? clienteId = null);

    /// <summary>Dispara a notificação de confirmação e detalhes de um novo agendamento criado.</summary>
    Task EnviarConfirmacaoAgendamentoAsync(int agendamentoId);

    /// <summary>Dispara o lembrete preventivo de horário marcado (ex: 2h ou 24h antes).</summary>
    Task EnviarLembreteAgendamentoAsync(int agendamentoId);

    /// <summary>Notifica o cliente ou profissional a respeito do cancelamento de um horário agendado.</summary>
    Task EnviarCancelamentoAgendamentoAsync(int agendamentoId, string? motivo = null);

    /// <summary>Envia convite com link após a conclusão do corte para que o cliente avalie o barbeiro.</summary>
    Task EnviarSolicitacaoAvaliacaoAsync(int agendamentoId);

    /// <summary>Envia mensagem em massa para uma lista de clientes (marketing/promoções), retornando o total disparado.</summary>
    Task<int> EnviarCampanhaMarketingAsync(IEnumerable<string> clienteIds, string mensagem);
}

