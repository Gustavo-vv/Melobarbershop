using Melobarbershop.Application.DTOs;

namespace Melobarbershop.Application.Servicos.Services;

public interface IPacoteService
{
    Task<PacoteDto?> ObterPorIdAsync(int id);
    Task<IEnumerable<PacoteDto>> ListarAtivosAsync();
    Task<IEnumerable<PacoteDto>> ListarTodosAsync();
    Task<PacoteDto> CriarAsync(CriarPacoteDto dto);
    Task<PacoteDto> AtualizarAsync(int id, AtualizarPacoteDto dto);
    Task DesativarAsync(int id);
    Task AtivarAsync(int id);
}
