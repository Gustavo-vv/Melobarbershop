// Nome do arquivo: IFavoritoServico.cs
// Objetivo: Interface para o servico de favoritos
// Camada: Application
// Como participa: Define as operacoes para favoritar filmes

using System.Collections.Generic;
using System.Threading.Tasks;
using SenacFlix.Application.DTOs;

namespace SenacFlix.Application.Servicos.Interfaces
{
    public interface IFavoritoServico
    {
        Task<ApiResposta<IEnumerable<FavoritoDto>>> ObterFavoritosDoUsuarioAsync(string usuarioId);
        Task<ApiResposta<FavoritoDto>> AdicionarFavoritoAsync(string usuarioId, AdicionarFavoritoDto dto);
        Task<ApiResposta<bool>> RemoverFavoritoAsync(string usuarioId, int filmeId);
        Task<ApiResposta<bool>> VerificarFavoritoAsync(string usuarioId, int filmeId);
    }
}
