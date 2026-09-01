using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Domain.Interfaces
{
    public interface IAgendamentoRepositorio
    {
        Task<IEnumerable<Agendamento>> ObterTodosAsync();
        Task<Agendamento?> ObterPorIdAsync(int id);
        Task<IEnumerable<Agendamento>> ObterPorClienteAsync(string clienteId);
        Task<IEnumerable<Agendamento>> ObterPorBarbeiroAsync(string barbeiroId, DateTime? data = null);
        Task<IEnumerable<Agendamento>> ObterAgendamentosDoDiaAsync(DateTime data);
        Task<Agendamento> AdicionarAsync(Agendamento agendamento);
        Task AtualizarAsync(Agendamento agendamento);
        Task AtualizarStatusAsync(int id, StatusAgendamento novoStatus);
        Task<bool> ExisteConflitoDeHorarioAsync(string barbeiroId, DateTime inicio, DateTime fim, int? agendamentoIdIgnorar = null);
    }
}