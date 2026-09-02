using Melobarbershop.Domain.Entidades;

namespace Melobarbershop.Domain.Interfaces.Repositories;

public interface INotificacaoLogRepository
{
    Task<NotificacaoLog?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<NotificacaoLog>> ObterPorClienteAsync(string clienteId, CancellationToken cancellationToken = default);
    Task<IEnumerable<NotificacaoLog>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
    Task<IEnumerable<NotificacaoLog>> ObterFalhasPorPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
    Task RegistrarAsync(NotificacaoLog log, CancellationToken cancellationToken = default);
}