using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Domain.Interfaces.Repositories;

public interface IAgendamentoRepository
{
    Task<Agendamento?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Agendamento?> ObterPorIdCompletoAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Agendamento>> ObterPorClienteAsync(string clienteId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Agendamento>> ObterPorBarbeiroEPeriodoAsync(string barbeiroId, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
    Task<IEnumerable<Agendamento>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
    Task<IEnumerable<Agendamento>> ObterProximosAgendamentosAsync(DateTime aPartirDe, string? barbeiroId = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<Agendamento>> ObterAgendamentosParaLembreteAsync(DateTime janelaInicio, DateTime janelaFim, CancellationToken cancellationToken = default);
    Task<bool> ExisteConflitoDeHorarioAsync(string barbeiroId, DateTime inicio, DateTime fim, int? agendamentoIdIgnorar = null, CancellationToken cancellationToken = default);
    Task AdicionarAsync(Agendamento agendamento, CancellationToken cancellationToken = default);
    Task AtualizarAsync(Agendamento agendamento, CancellationToken cancellationToken = default);
    Task AtualizarStatusAsync(int id, StatusAgendamento status, CancellationToken cancellationToken = default);
}