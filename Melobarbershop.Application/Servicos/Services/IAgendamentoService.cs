using Melobarbershop.Application.DTOs;

namespace Melobarbershop.Application.Servicos.Services;

public interface IAgendamentoService
{
    Task<AgendamentoDto?> ObterPorIdAsync(int id);
    Task<IEnumerable<AgendamentoDto>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim, string? barbeiroId = null);
    Task<IEnumerable<AgendamentoDto>> ListarPorClienteAsync(string clienteId);
    Task<IEnumerable<DateTime>> ListarHorariosDisponiveisAsync(string barbeiroId, DateTime data, IEnumerable<int> servicoIds);
    Task<AgendamentoDto> CriarAsync(CriarAgendamentoDto dto);
    Task ConfirmarAsync(int agendamentoId);
    Task IniciarAtendimentoAsync(int agendamentoId);
    Task ConcluirAsync(int agendamentoId);
    Task CancelarAsync(int agendamentoId, string? motivo = null);
    Task RegistrarNaoComparecimentoAsync(int agendamentoId);
    Task<AgendamentoDto> ReagendarAsync(int agendamentoId, ReagendarAgendamentoDto dto);
}
