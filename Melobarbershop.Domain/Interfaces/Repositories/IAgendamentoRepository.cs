// Arquivo: Melobarbershop.Domain/Interfaces/Repositories/IAgendamentoRepository.cs
// Namespace: Melobarbershop.Domain.Interfaces.Repositories
// Conteúdo: interface IAgendamentoRepository
// Resumo: Contrato de repositório para operações CRUD e consultas específicas sobre Agendamento.
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Domain.Interfaces.Repositories;

public interface IAgendamentoRepository
{
    Task<Agendamento?> ObterPorIdAsync(int id);
    Task<Agendamento?> ObterPorIdCompletoAsync(int id);
    Task<IEnumerable<Agendamento>> ObterPorClienteAsync(string clienteId);
    Task<IEnumerable<Agendamento>> ObterPorBarbeiroEPeriodoAsync(string barbeiroId, DateTime inicio, DateTime fim);
    Task<IEnumerable<Agendamento>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim);
    Task<IEnumerable<Agendamento>> ObterProximosAgendamentosAsync(DateTime aPartirDe, string? barbeiroId = null);
    Task<IEnumerable<Agendamento>> ObterAgendamentosParaLembreteAsync(DateTime janelaInicio, DateTime janelaFim);
    Task<bool> ExisteConflitoDeHorarioAsync(string barbeiroId, DateTime inicio, DateTime fim, int? agendamentoIdIgnorar = null);
    Task AdicionarAsync(Agendamento agendamento);
    Task AtualizarAsync(Agendamento agendamento);
    Task AtualizarStatusAsync(int id, StatusAgendamento status);
}