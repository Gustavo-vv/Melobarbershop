// Arquivo: Melobarbershop.Domain/Interfaces/Repositories/INotificacaoLogRepository.cs
// Namespace: Melobarbershop.Domain.Interfaces.Repositories
// Conteúdo: interface INotificacaoLogRepository
// Resumo: Contrato de repositório para persistência e consulta de logs de notificações.
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