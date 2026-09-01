// Nome do arquivo: ICategoriaServico.cs
// Objetivo: Interface para o servico de categorias
// Camada: Application
// Como participa: Define as operacoes para manipulacao de categorias usando DTOs

using SenacFlix.Application.DTOs;

namespace SenacFlix.Application.Servicos.Interfaces
{
    public interface ICategoriaServico
    {
        Task<ApiResposta<IEnumerable<CategoriaDto>>> ObterTodasAsync(bool incluirInativas = false);
        Task<ApiResposta<CategoriaDto>> ObterPorIdAsync(int id);
        Task<ApiResposta<CategoriaDto>> CadastrarAsync(CriarCategoriaDto dto);
        Task<ApiResposta<CategoriaDto>> AtualizarAsync(int id, CriarCategoriaDto dto);
        Task<ApiResposta<bool>> DesativarAsync(int id);
        Task<ApiResposta<bool>> ReativarAsync(int id);
        Task<ApiResposta<bool>> ExcluirPermanentementeAsync(int id);
    }
}
