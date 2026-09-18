// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Entidades/NotificacaoLog.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem chama/usa:
//   * Melobarbershop.Application (Serviços de mensageria WhatsApp/SMS, NotificacaoService)
//   * Melobarbershop.Infrastructure (BarbeariaDbContext, NotificacaoLogConfiguration, NotificacaoLogRepository)
//   * Melobarbershop.Desktop / Admin (Auditoria e telemetria de disparos de mensagens)
// - Quem ele referencia:
//   * ApplicationUser (destinatário opcional cadastrado)
// ============================================================================

namespace Melobarbershop.Domain.Entidades;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Entidade de auditoria e rastreamento para comunicação externa (WhatsApp / SMS / E-mail).
/// 
/// POR QUE EXISTE:
/// Garante que toda tentativa de envio de lembrete de agendamento, confirmação ou mensagem promocional
/// seja registrada com seu status de entrega e a resposta retornada pelo gateway de mensageria (ex: Z-API / Twilio).
/// 
/// O QUE QUEBRARIA SE NÃO EXISTISSE:
/// Se um cliente alegar que não recebeu o lembrete de agendamento ou se a API de WhatsApp falhar por falta de saldo,
/// não haveria histórico para auditoria técnica e suporte ao cliente.
/// </summary>
public class NotificacaoLog
{
    /// <summary>
    /// Identificador único do log de envio (chave primária).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Chave estrangeira opcional do usuário cadastrado que recebeu o disparo.
    /// É opcional (nullable) para permitir envio a números não previamente cadastrados.
    /// </summary>
    public string? ClienteId { get; set; }

    /// <summary>
    /// Propriedade de navegação para o Cliente (quando existir).
    /// </summary>
    public ApplicationUser? Cliente { get; set; }

    /// <summary>
    /// Telefone de destino para o qual o disparo foi feito, no padrão E.164 ou DDI+DDD+Número.
    /// </summary>
    public string NumeroDestino { get; set; } = string.Empty;

    /// <summary>
    /// Texto final renderizado da mensagem (com variáveis já interpoladas, como nome e horário).
    /// </summary>
    public string MensagemEnviada { get; set; } = string.Empty;

    /// <summary>
    /// Momento exato em que a chamada de envio foi disparada (em UTC).
    /// </summary>
    public DateTime DataEnvio { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Indica se a API de mensageria aceitou e processou o envio com êxito (true) ou se houve rejeição (false).
    /// </summary>
    public bool Sucesso { get; set; }

    /// <summary>
    /// Payload de resposta da API externa ou mensagem de erro capturada (para fins de debug e diagnóstico).
    /// </summary>
    public string? DetalhesRespostaApi { get; set; }
}
