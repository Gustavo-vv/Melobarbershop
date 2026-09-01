// Nome do arquivo: IFilmeServico.cs
// Objetivo: Interface para o servico de filmes
// Camada: Application
// Como participa: Define as operacoes disponiveis para manipulacao de filmes, usando DTOs

using System.Collections.Generic;
using System.Threading.Tasks;
using SenacFlix.Application.DTOs;

namespace SenacFlix.Application.Servicos.Interfaces
{
    public interface IFilmeServico
    {
        Task<ApiResposta<FilmeDto?>> ObterFilmeDestaqueAsync();
        Task<ApiResposta<IEnumerable<FilmeDto>>> ObterTodosAsync(bool incluirInativos = false);
        Task<ApiResposta<FilmeDto>> ObterPorIdAsync(int id);
        Task<ApiResposta<IEnumerable<FilmeDto>>> BuscarAsync(string? termo, int? categoriaId = null);
        Task<ApiResposta<IEnumerable<FilmeDto>>> ObterPorCategoriaAsync(int categoriaId);
        Task<ApiResposta<FilmeDto>> CadastrarAsync(CriarFilmeDto dto);
        Task<ApiResposta<FilmeDto>> AtualizarAsync(int id, AtualizarFilmeDto dto);
        Task<ApiResposta<bool>> DesativarAsync(int id);
        Task<ApiResposta<bool>> ExcluirPermanentementeAsync(int id);
        Task<ApiResposta<bool>> ReativarAsync(int id);
    }
}
