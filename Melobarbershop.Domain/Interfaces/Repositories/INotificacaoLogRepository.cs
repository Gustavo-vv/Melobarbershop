using Melobarbershop.Domain.Entidades;

namespace Melobarbershop.Domain.Interfaces.Repositories;

public interface INotificacaoLogRepository
{
    Task<NotificacaoLog?> ObterPorIdAsync(int id);
    Task<IEnumerable<NotificacaoLog>> ObterPorClienteAsync(string clienteId);
    Task<IEnumerable<NotificacaoLog>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim);
    Task<IEnumerable<NotificacaoLog>> ObterFalhasPorPeriodoAsync(DateTime inicio, DateTime fim);
    Task RegistrarAsync(NotificacaoLog log);
}