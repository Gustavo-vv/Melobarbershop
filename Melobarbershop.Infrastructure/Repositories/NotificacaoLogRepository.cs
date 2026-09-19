// ============================================================================
// Arquivo: NotificacaoLogRepository.cs
// Camada: Melobarbershop.Infrastructure (Repositories)
// Objetivo: Repositório para auditoria e rastreamento de envio de notificações (WhatsApp, E-mail, SMS).
// Papel na Arquitetura:
//   - Grava logs de disparos de mensagens com status de sucesso ou erro.
//   - Permite consultar histórico de mensagens por cliente ou monitorar falhas operacionais.
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;
using Melobarbershop.Infrastructure.Data;

namespace Melobarbershop.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório para logs de notificações, grava e consulta envios.
/// </summary>
public class NotificacaoLogRepository : INotificacaoLogRepository
{
    private readonly BarbeariaDbContext _context;

    /// <summary>
    /// Construtor com injeção de dependência do contexto EF Core.
    /// </summary>
    public NotificacaoLogRepository(BarbeariaDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Busca um log de notificação pelo ID.
    /// </summary>
    public async Task<NotificacaoLog?> ObterPorIdAsync(int id)
    {
        return await _context.NotificacoesLog
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    /// <summary>
    /// Consulta todo o histórico de notificações enviadas para determinado cliente.
    /// </summary>
    public async Task<IEnumerable<NotificacaoLog>> ObterPorClienteAsync(string clienteId)
    {
        return await _context.NotificacoesLog
            .Where(n => n.ClienteId == clienteId)
            .OrderByDescending(n => n.DataEnvio)
            .ToListAsync();
    }

    /// <summary>
    /// Consulta os registros de notificações enviadas dentro de uma faixa de datas.
    /// </summary>
    public async Task<IEnumerable<NotificacaoLog>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim)
    {
        return await _context.NotificacoesLog
            .Where(n => n.DataEnvio >= inicio && n.DataEnvio <= fim)
            .OrderByDescending(n => n.DataEnvio)
            .ToListAsync();
    }

    /// <summary>
    /// Lista apenas notificações que falharam no envio dentro do período, permitindo reprocessamento ou alerta.
    /// </summary>
    public async Task<IEnumerable<NotificacaoLog>> ObterFalhasPorPeriodoAsync(DateTime inicio, DateTime fim)
    {
        return await _context.NotificacoesLog
            .Where(n => !n.Sucesso && n.DataEnvio >= inicio && n.DataEnvio <= fim)
            .OrderByDescending(n => n.DataEnvio)
            .ToListAsync();
    }

    /// <summary>
    /// Grava uma nova entrada de log de notificação no banco de dados.
    /// </summary>
    public async Task RegistrarAsync(NotificacaoLog log)
    {
        await _context.NotificacoesLog.AddAsync(log);
        await _context.SaveChangesAsync();
    }
}
