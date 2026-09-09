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

    public async Task<NotificacaoLog?> ObterPorIdAsync(int id)
    {
        return await _context.NotificacoesLog
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task<IEnumerable<NotificacaoLog>> ObterPorClienteAsync(string clienteId)
    {
        return await _context.NotificacoesLog
            .Where(n => n.ClienteId == clienteId)
            .OrderByDescending(n => n.DataEnvio)
            .ToListAsync();
    }

    public async Task<IEnumerable<NotificacaoLog>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim)
    {
        return await _context.NotificacoesLog
            .Where(n => n.DataEnvio >= inicio && n.DataEnvio <= fim)
            .OrderByDescending(n => n.DataEnvio)
            .ToListAsync();
    }

    public async Task<IEnumerable<NotificacaoLog>> ObterFalhasPorPeriodoAsync(DateTime inicio, DateTime fim)
    {
        return await _context.NotificacoesLog
            .Where(n => !n.Sucesso && n.DataEnvio >= inicio && n.DataEnvio <= fim)
            .OrderByDescending(n => n.DataEnvio)
            .ToListAsync();
    }

    public async Task RegistrarAsync(NotificacaoLog log)
    {
        await _context.NotificacoesLog.AddAsync(log);
        await _context.SaveChangesAsync();
    }
}
