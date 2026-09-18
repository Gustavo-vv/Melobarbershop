// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Interfaces/Repositories/INotificacaoLogRepository.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem define: Domain (Inversão de Dependência - DIP)
// - Quem implementa: Melobarbershop.Infrastructure (NotificacaoLogRepository via EF Core)
// - Quem consome: Melobarbershop.Application (NotificacaoService, gateways de WhatsApp/SMS)
// ============================================================================

using Melobarbershop.Domain.Entidades;

namespace Melobarbershop.Domain.Interfaces.Repositories;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Contrato de persistência para os logs de telemetria de disparos de mensagens.
/// 
/// POR QUE EXISTE:
/// Permite que a camada de aplicação registre o resultado de envios e audite falhas
/// sem se acoplar a uma tecnologia específica de banco de dados.
/// </summary>
public interface INotificacaoLogRepository
{
    /// <summary>
    /// Recupera um registro de log individual por ID.
    /// </summary>
    Task<NotificacaoLog?> ObterPorIdAsync(int id);

    /// <summary>
    /// Consulta o histórico de todas as mensagens já disparadas para um determinado cliente.
    /// </summary>
    Task<IEnumerable<NotificacaoLog>> ObterPorClienteAsync(string clienteId);

    /// <summary>
    /// Consulta todas as mensagens enviadas em um intervalo de datas (para relatórios de custo de mensageria).
    /// </summary>
    Task<IEnumerable<NotificacaoLog>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim);

    /// <summary>
    /// Filtra exclusivamente as notificações que falharam (Sucesso == false) em um período.
    /// Utilizado pelo dashboard administrativo para detectar instabilidade na API de WhatsApp.
    /// </summary>
    Task<IEnumerable<NotificacaoLog>> ObterFalhasPorPeriodoAsync(DateTime inicio, DateTime fim);

    /// <summary>
    /// Grava uma nova entrada de log após a execução da chamada HTTP para o gateway externo.
    /// </summary>
    Task RegistrarAsync(NotificacaoLog log);
}