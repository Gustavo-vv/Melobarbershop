using Melobarbershop.Application.DTOs;

namespace Melobarbershop.Application.Servicos.Services;

public interface IPacoteService
{
    Task<ApiResposta<PacoteDto>> ObterPorIdAsync(int id);
    Task<ApiResposta<IEnumerable<PacoteDto>>> ListarAtivosAsync();
    Task<ApiResposta<IEnumerable<PacoteDto>>> ListarTodosAsync();
    Task<ApiResposta<PacoteDto>> CriarAsync(CriarPacoteDto dto);
    Task<ApiResposta<PacoteDto>> AtualizarAsync(int id, AtualizarPacoteDto dto);
    Task<ApiResposta<PacoteDto>> DesativarAsync(int id);
    Task<ApiResposta<PacoteDto>> AtivarAsync(int id);
}
