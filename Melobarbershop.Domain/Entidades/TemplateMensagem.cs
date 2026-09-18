// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Entidades/TemplateMensagem.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem chama/usa:
//   * Melobarbershop.Application (NotificacaoService, MensageriaBackgroundService)
//   * Melobarbershop.Infrastructure (BarbeariaDbContext, TemplateMensagemConfiguration, TemplateMensagemRepository)
//   * Melobarbershop.Desktop (Configuração de réguas de comunicação e mensagens de WhatsApp)
// - Quem ele referencia:
//   * TipoGatilhoMensagem (Enum com os eventos disparadores do sistema)
// ============================================================================

using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Domain.Entidades;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Entidade de parametrização dinâmica de comunicação automatizada (CRM / WhatsApp).
/// 
/// POR QUE EXISTE:
/// Permite que a administração da barbearia customize o texto dos lembretes e avisos
/// sem precisar alterar código C# e recompilar a aplicação, utilizando placeholders/tags
/// como {NomeCliente}, {Horario}, {Barbeiro}.
/// 
/// O QUE QUEBRARIA SE NÃO EXISTISSE:
/// As mensagens enviadas ficariam 'hardcoded' em código-fonte, engessando qualquer ajuste de tom de voz,
/// promoções ou alterações de links enviados pelo WhatsApp aos clientes.
/// </summary>
public class TemplateMensagem
{
    /// <summary>
    /// Identificador único do template (chave primária).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nome identificador do modelo (ex: "Lembrete 2 Horas Antes", "Confirmação de Agendamento").
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Evento de negócio que dispara o envio automático deste template (ex: AgendamentoCriado, LembretePreAgendamento, Aniversario).
    /// </summary>
    public TipoGatilhoMensagem Gatilho { get; set; }

    /// <summary>
    /// Corpo do texto contendo variáveis a serem interpoladas
    /// (ex: "Olá {NomeCliente}, seu corte com {Barbeiro} está confirmado para {DataHora}!").
    /// </summary>
    public string ConteudoTemplate { get; set; } = string.Empty;

    /// <summary>
    /// Flag que liga ou desliga o disparo automático para este gatilho específico.
    /// </summary>
    public bool Ativo { get; set; } = true;
}
