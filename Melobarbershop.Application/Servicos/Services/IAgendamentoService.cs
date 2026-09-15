using Melobarbershop.Application.DTOs;

public interface IAgendamentoService
{
    Task<ApiResposta<AgendamentoDto>> ObterPorIdAsync(int id);
    Task<ApiResposta<IEnumerable<AgendamentoDto>>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim, string? barbeiroId = null);
    Task<ApiResposta<IEnumerable<AgendamentoDto>>> ListarPorClienteAsync(string clienteId);
    Task<ApiResposta<IEnumerable<DateTime>>> ListarHorariosDisponiveisAsync(string barbeiroId, DateTime data, IEnumerable<int> servicoIds);
    Task<ApiResposta<IEnumerable<HorarioSlotDto>>> ListarTodosHorariosDoDiaAsync(string barbeiroId, DateTime data, IEnumerable<int> servicoIds);
    Task<ApiResposta<AgendamentoDto>> CriarAsync(CriarAgendamentoDto dto);
    Task<ApiResposta<AgendamentoDto>> ConfirmarAsync(int agendamentoId);
    Task<ApiResposta<AgendamentoDto>> IniciarAtendimentoAsync(int agendamentoId);
    Task<ApiResposta<AgendamentoDto>> ConcluirAsync(int agendamentoId);
    Task<ApiResposta<AgendamentoDto>> CancelarAsync(int agendamentoId, string? motivo = null);
    Task<ApiResposta<AgendamentoDto>> RegistrarNaoComparecimentoAsync(int agendamentoId);
    Task<ApiResposta<AgendamentoDto>> ReagendarAsync(int agendamentoId, ReagendarAgendamentoDto dto);
}