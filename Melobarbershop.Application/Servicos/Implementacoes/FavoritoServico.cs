// Nome do arquivo: FavoritoServico.cs
// Objetivo: Implementacao do servico de favoritos
// Camada: Application
// Como participa: Regras de negocio para manipulacao de filmes favoritos de cada usuario

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using SenacFlix.Application.DTOs;
using SenacFlix.Application.Servicos.Interfaces;
using SenacFlix.Domain.Entidades;
using SenacFlix.Domain.Interfaces;

namespace SenacFlix.Application.Servicos.Implementacoes
{
    public class FavoritoServico : IFavoritoServico
    {
        private readonly IFavoritoRepositorio _repositorio;
        private readonly IFilmeRepositorio _filmeRepositorio;
        private readonly IMapper _mapper;

        public FavoritoServico(IFavoritoRepositorio repositorio, IFilmeRepositorio filmeRepositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _filmeRepositorio = filmeRepositorio;
            _mapper = mapper;
        }

        public async Task<ApiResposta<IEnumerable<FavoritoDto>>> ObterFavoritosDoUsuarioAsync(string usuarioId)
        {
            try
            {
                var favoritos = await _repositorio.ObterPorUsuarioAsync(usuarioId);
                var dtos = _mapper.Map<IEnumerable<FavoritoDto>>(favoritos);
                return ApiResposta<IEnumerable<FavoritoDto>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                return ApiResposta<IEnumerable<FavoritoDto>>.Falha($"Erro ao obter favoritos: {ex.Message}");
            }
        }

        public async Task<ApiResposta<FavoritoDto>> AdicionarFavoritoAsync(string usuarioId, AdicionarFavoritoDto dto)
        {
            try
            {
                var filme = await _filmeRepositorio.ObterPorIdAsync(dto.FilmeId);
                if (filme == null)
                    return ApiResposta<FavoritoDto>.Falha("Filme nao encontrado.");

                bool jaFavorito = await _repositorio.ExisteAsync(usuarioId, dto.FilmeId);
                if (jaFavorito)
                    return ApiResposta<FavoritoDto>.Falha("Este filme ja esta nos seus favoritos.");

                var favorito = new Favorito
                {
                    UsuarioId = usuarioId,
                    FilmeId = dto.FilmeId,
                    DataFavorito = DateTime.UtcNow
                };

                var favoritoSalvo = await _repositorio.AdicionarAsync(favorito);
                
                // Busca completo para o DTO ter as informacoes do filme
                var favoritoCompleto = await _repositorio.ObterAsync(usuarioId, dto.FilmeId);
                var favoritoDto = _mapper.Map<FavoritoDto>(favoritoCompleto);
                
                return ApiResposta<FavoritoDto>.Ok(favoritoDto, "Filme adicionado aos favoritos.");
            }
            catch (Exception ex)
            {
                return ApiResposta<FavoritoDto>.Falha($"Erro ao adicionar favorito: {ex.Message}");
            }
        }

        public async Task<ApiResposta<bool>> RemoverFavoritoAsync(string usuarioId, int filmeId)
        {
            try
            {
                var favorito = await _repositorio.ObterAsync(usuarioId, filmeId);
                if (favorito == null)
                    return ApiResposta<bool>.Falha("Favorito nao encontrado.");

                await _repositorio.RemoverAsync(favorito);
                return ApiResposta<bool>.Ok(true, "Filme removido dos favoritos.");
            }
            catch (Exception ex)
            {
                return ApiResposta<bool>.Falha($"Erro ao remover favorito: {ex.Message}");
            }
        }

        public async Task<ApiResposta<bool>> VerificarFavoritoAsync(string usuarioId, int filmeId)
        {
            try
            {
                bool existe = await _repositorio.ExisteAsync(usuarioId, filmeId);
                return ApiResposta<bool>.Ok(existe);
            }
            catch (Exception ex)
            {
                return ApiResposta<bool>.Falha($"Erro ao verificar favorito: {ex.Message}");
            }
        }
    }
}
