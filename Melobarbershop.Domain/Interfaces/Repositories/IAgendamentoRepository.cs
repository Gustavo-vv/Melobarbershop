namespace Melobarbershop.Domain.Interfaces.Repositories;

using Barbearia.Domain.Enums;
using Melobarbershop.Domain.Entidades;

public interface IAgendamentoRepository
{
    Task<Agendamento?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Agendamento?> ObterPorIdCompletoAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Agendamento>> ObterPorClienteAsync(int clienteId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Agendamento>> ObterPorBarbeiroEPeriodoAsync(int barbeiroId, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
    Task<IEnumerable<Agendamento>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
    Task<IEnumerable<Agendamento>> ObterProximosAgendamentosAsync(DateTime aPartirDe, int? barbeiroId = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<Agendamento>> ObterAgendamentosParaLembreteAsync(DateTime janelaInicio, DateTime janelaFim, CancellationToken cancellationToken = default);
    Task<bool> ExisteConflitoDeHorarioAsync(int barbeiroId, DateTime inicio, DateTime fim, int? agendamentoIdIgnorar = null, CancellationToken cancellationToken = default);
    Task AdicionarAsync(Agendamento agendamento, CancellationToken cancellationToken = default);
    Task AtualizarAsync(Agendamento agendamento, CancellationToken cancellationToken = default);
    Task AtualizarStatusAsync(int id, StatusAgendamento status, CancellationToken cancellationToken = default);
}