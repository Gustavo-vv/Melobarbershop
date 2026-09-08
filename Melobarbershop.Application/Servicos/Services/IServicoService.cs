using Melobarbershop.Application.DTOs;

namespace Melobarbershop.Application.Servicos.Services;

public interface IServicoService
{
    Task<ApiResposta<IEnumerable<ServicoDto>>> ListarAsync(bool incluirInativos = false);
    Task<ApiResposta<IEnumerable<ServicoDto>>> ListarPorCategoriaAsync(int categoriaId, bool incluirInativos = false);
    Task<ApiResposta<ServicoDto>> ObterPorIdAsync(int id);
    Task<ApiResposta<ServicoDto>> CriarAsync(CriarServicoDto dto);
    Task<ApiResposta<ServicoDto>> AtualizarAsync(int id, AtualizarServicoDto dto);
    Task<ApiResposta<bool>> DesativarAsync(int id);
    Task<ApiResposta<bool>> AtivarAsync(int id);
    Task<ApiResposta<bool>> RemoverPermanentementeAsync(int id);
}
