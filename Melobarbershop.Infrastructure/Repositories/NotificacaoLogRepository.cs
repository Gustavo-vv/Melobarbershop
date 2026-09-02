using Microsoft.EntityFrameworkCore;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;
using Melobarbershop.Infrastructure.Data;

namespace Melobarbershop.Infrastructure.Repositories;

public class NotificacaoLogRepository : INotificacaoLogRepository
{
    private readonly BarbeariaDbContext _context;

    public NotificacaoLogRepository(BarbeariaDbContext context)
    {
        _context = context;
    }

    public async Task<NotificacaoLog?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.NotificacoesLog
            .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<NotificacaoLog>> ObterPorClienteAsync(string clienteId, CancellationToken cancellationToken = default)
    {
        return await _context.NotificacoesLog
            .Where(n => n.ClienteId == clienteId)
            .OrderByDescending(n => n.DataEnvio)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<NotificacaoLog>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
    {
        return await _context.NotificacoesLog
            .Where(n => n.DataEnvio >= inicio && n.DataEnvio <= fim)
            .OrderByDescending(n => n.DataEnvio)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<NotificacaoLog>> ObterFalhasPorPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
    {
        return await _context.NotificacoesLog
            .Where(n => !n.Sucesso && n.DataEnvio >= inicio && n.DataEnvio <= fim)
            .OrderByDescending(n => n.DataEnvio)
            .ToListAsync(cancellationToken);
    }

    public async Task RegistrarAsync(NotificacaoLog log, CancellationToken cancellationToken = default)
    {
        await _context.NotificacoesLog.AddAsync(log, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
