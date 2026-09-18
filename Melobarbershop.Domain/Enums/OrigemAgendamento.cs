// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Enums/OrigemAgendamento.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem usa:
//   * Agendamento (propriedade Origem na entidade de domínio)
//   * Melobarbershop.Application (DTOs e estatísticas de captação de clientes)
//   * Melobarbershop.UI (envia Site como origem padrão)
//   * Melobarbershop.Desktop (envia PresencialBalcao ou WhatsApp)
// ============================================================================

namespace Melobarbershop.Domain.Enums;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Enumeração tipada que mapeia o ponto de contato inicial (canal) pelo qual o agendamento ingressou no sistema.
/// 
/// POR QUE EXISTE:
/// Permite mensurar a eficácia de cada canal de aquisição de clientes (marketing digital vs conveniência do WhatsApp vs agendamento de balcão).
/// 
/// O QUE QUEBRARIA SE NÃO EXISTISSE:
/// Métricas gerenciais de conversão e relatórios de distribuição de canais não teriam precisão.
/// </summary>
public enum OrigemAgendamento
{
    /// <summary>
    /// Criado pelo próprio cliente através do portal web público (Landing Page / Web App).
    /// </summary>
    Site = 1,

    /// <summary>
    /// Criado através de integração com chatbot ou atendimento manual via WhatsApp.
    /// </summary>
    WhatsApp = 2,

    /// <summary>
    /// Criado por aplicativo mobile dedicado do cliente (iOS/Android).
    /// </summary>
    Aplicativo = 3,

    /// <summary>
    /// Agendado presencialmente ou por telefone diretamente na recepção da barbearia (Desktop).
    /// </summary>
    PresencialBalcao = 4
}