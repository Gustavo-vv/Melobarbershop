namespace Barbearia.Application.Interfaces.Services;

using Barbearia.Domain.Enums;
using Melobarbershop.Domain.Entidades;

public interface IAgendamentoService
{
    Task<Agendamento?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Agendamento?> ObterDetalhesCompletosAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Agendamento>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim, int? barbeiroId = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<Agendamento>> ListarPorClienteAsync(int clienteId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DateTime>> ListarHorariosDisponiveisAsync(int barbeiroId, DateTime data, IEnumerable<int> servicoIds, CancellationToken cancellationToken = default);
    Task<Agendamento> CriarAsync(int clienteId, int barbeiroId, DateTime inicio, IEnumerable<int> servicoIds, OrigemAgendamento origem = OrigemAgendamento.Site, string? observacoes = null, CancellationToken cancellationToken = default);
    Task ConfirmarAsync(int agendamentoId, CancellationToken cancellationToken = default);
    Task IniciarAtendimentoAsync(int agendamentoId, CancellationToken cancellationToken = default);
    Task ConcluirAsync(int agendamentoId, CancellationToken cancellationToken = default);
    Task CancelarAsync(int agendamentoId, string? motivo = null, CancellationToken cancellationToken = default);
    Task RegistrarNaoComparecimentoAsync(int agendamentoId, CancellationToken cancellationToken = default);
    Task<Agendamento> ReagendarAsync(int agendamentoId, DateTime novoInicio, int? novoBarbeiroId = null, CancellationToken cancellationToken = default);
}