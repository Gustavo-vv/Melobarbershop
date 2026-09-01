// Nome do arquivo: FilmeServico.cs
// Objetivo: Implementacao do servico de filmes
// Camada: Application
// Como participa: Regras de negocio e orquestracao entre repositorios e AutoMapper para Filmes

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
    public class FilmeServico : IFilmeServico
    {
        private readonly IFilmeRepositorio _repositorio;
        private readonly ICategoriaRepositorio _categoriaRepositorio;
        private readonly IMapper _mapper;

        public FilmeServico(IFilmeRepositorio repositorio, ICategoriaRepositorio categoriaRepositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _categoriaRepositorio = categoriaRepositorio;
            _mapper = mapper;
        }

        public async Task<ApiResposta<FilmeDto?>> ObterFilmeDestaqueAsync()
        {
            try
            {
                var todosFilmes = await _repositorio.ObterTodosAsync(false);
                var filmesDestaque = new List<Filme>();
                var filmesElegiveis = new List<Filme>();
                
                foreach (var f in todosFilmes)
                {
                    if (!string.IsNullOrEmpty(f.ImagemBannerUrl) && !string.IsNullOrEmpty(f.TrailerYoutubeUrl))
                    {
                        filmesElegiveis.Add(f);
                        if (f.DestaqueHome)
                        {
                            filmesDestaque.Add(f);
                        }
                    }
                }

                var listaParaSorteio = filmesDestaque.Count > 0 ? filmesDestaque : filmesElegiveis;

                if (listaParaSorteio.Count == 0)
                {
                    return ApiResposta<FilmeDto?>.Ok(null);
                }

                var random = new Random();
                var index = random.Next(listaParaSorteio.Count);
                var filmeEscolhido = listaParaSorteio[index];

                var dto = _mapper.Map<FilmeDto>(filmeEscolhido);
                return ApiResposta<FilmeDto?>.Ok(dto);
            }
            catch (Exception ex)
            {
                return ApiResposta<FilmeDto?>.Falha($"Erro ao obter filme destaque: {ex.Message}");
            }
        }

        public async Task<ApiResposta<IEnumerable<FilmeDto>>> ObterTodosAsync(bool incluirInativos = false)
        {
            try
            {
                var filmes = await _repositorio.ObterTodosAsync(incluirInativos);
                var dtos = _mapper.Map<IEnumerable<FilmeDto>>(filmes);
                return ApiResposta<IEnumerable<FilmeDto>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                return ApiResposta<IEnumerable<FilmeDto>>.Falha($"Erro ao obter filmes: {ex.Message}");
            }
        }

        public async Task<ApiResposta<FilmeDto>> ObterPorIdAsync(int id)
        {
            try
            {
                var filme = await _repositorio.ObterPorIdAsync(id);
                if (filme == null)
                    return ApiResposta<FilmeDto>.Falha("Filme nao encontrado.");

                var dto = _mapper.Map<FilmeDto>(filme);
                return ApiResposta<FilmeDto>.Ok(dto);
            }
            catch (Exception ex)
            {
                return ApiResposta<FilmeDto>.Falha($"Erro ao obter o filme: {ex.Message}");
            }
        }

        public async Task<ApiResposta<IEnumerable<FilmeDto>>> BuscarAsync(string? termo, int? categoriaId = null)
        {
            try
            {
                var filmes = await _repositorio.BuscarAsync(termo, categoriaId);
                var dtos = _mapper.Map<IEnumerable<FilmeDto>>(filmes);
                return ApiResposta<IEnumerable<FilmeDto>>.Ok(dtos, "Busca realizada com sucesso.");
            }
            catch (Exception ex)
            {
                return ApiResposta<IEnumerable<FilmeDto>>.Falha($"Erro ao buscar filmes: {ex.Message}");
            }
        }

        public async Task<ApiResposta<IEnumerable<FilmeDto>>> ObterPorCategoriaAsync(int categoriaId)
        {
            try
            {
                var filmes = await _repositorio.ObterPorCategoriaAsync(categoriaId);
                var dtos = _mapper.Map<IEnumerable<FilmeDto>>(filmes);
                return ApiResposta<IEnumerable<FilmeDto>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                return ApiResposta<IEnumerable<FilmeDto>>.Falha($"Erro ao obter filmes por categoria: {ex.Message}");
            }
        }

        public async Task<ApiResposta<FilmeDto>> CadastrarAsync(CriarFilmeDto dto)
        {
            try
            {
                // Verifica se a categoria informada existe
                var categoria = await _categoriaRepositorio.ObterPorIdAsync(dto.CategoriaId);
                if (categoria == null)
                    return ApiResposta<FilmeDto>.Falha("Categoria invalida.");

                var filme = _mapper.Map<Filme>(dto);
                var filmeCadastrado = await _repositorio.AdicionarAsync(filme);
                
                // Buscar novamente para carregar os relacionamentos corretamente para o DTO de retorno
                var filmeCompleto = await _repositorio.ObterPorIdAsync(filmeCadastrado.Id);
                var filmeDto = _mapper.Map<FilmeDto>(filmeCompleto);
                
                return ApiResposta<FilmeDto>.Ok(filmeDto, "Filme cadastrado com sucesso.");
            }
            catch (Exception ex)
            {
                return ApiResposta<FilmeDto>.Falha($"Erro ao cadastrar filme: {ex.Message}");
            }
        }

        public async Task<ApiResposta<FilmeDto>> AtualizarAsync(int id, AtualizarFilmeDto dto)
        {
            try
            {
                if (id != dto.Id)
                    return ApiResposta<FilmeDto>.Falha("O Id informado na URL e diferente do Id no corpo da requisicao.");

                var filmeExistente = await _repositorio.ObterPorIdAsync(id);
                if (filmeExistente == null)
                    return ApiResposta<FilmeDto>.Falha("Filme nao encontrado.");

                var categoria = await _categoriaRepositorio.ObterPorIdAsync(dto.CategoriaId);
                if (categoria == null)
                    return ApiResposta<FilmeDto>.Falha("Categoria invalida.");

                _mapper.Map(dto, filmeExistente);
                filmeExistente.DataAtualizacao = DateTime.UtcNow;

                await _repositorio.AtualizarAsync(filmeExistente);
                
                var filmeDto = _mapper.Map<FilmeDto>(filmeExistente);
                return ApiResposta<FilmeDto>.Ok(filmeDto, "Filme atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                return ApiResposta<FilmeDto>.Falha($"Erro ao atualizar filme: {ex.Message}");
            }
        }

        public async Task<ApiResposta<bool>> DesativarAsync(int id)
        {
            try
            {
                var filme = await _repositorio.ObterPorIdAsync(id);
                if (filme == null)
                    return ApiResposta<bool>.Falha("Filme nao encontrado.");

                await _repositorio.DesativarAsync(id);
                return ApiResposta<bool>.Ok(true, "Filme desativado com sucesso.");
            }
            catch (Exception ex)
            {
                return ApiResposta<bool>.Falha($"Erro ao desativar filme: {ex.Message}");
            }
        }

        public async Task<ApiResposta<bool>> ExcluirPermanentementeAsync(int id)
        {
            try
            {
                var filme = await _repositorio.ObterPorIdAsync(id);
                if (filme == null)
                    return ApiResposta<bool>.Falha("Filme nao encontrado.");

                await _repositorio.ExcluirPermanentementeAsync(id);
                return ApiResposta<bool>.Ok(true, "Filme excluido permanentemente com sucesso.");
            }
            catch (Exception ex)
            {
                return ApiResposta<bool>.Falha($"Erro ao excluir filme: {ex.Message}");
            }
        }

        public async Task<ApiResposta<bool>> ReativarAsync(int id)
        {
            try
            {
                var filme = await _repositorio.ObterPorIdAsync(id);
                if (filme == null && await _repositorio.ObterPorIdAsync(id) == null) // We actually need to include inativos to find it if ObterPorIdAsync only gets ativos. 
                {
                    // Wait, ObterPorIdAsync currently doesn't have `incluirInativos` parameter in the repository. Let's fix that too or just call ReativarAsync.
                }
                
                // Em FilmeRepositorio, ObterPorIdAsync traz o filme mesmo inativo porque não há filtro lá.
                if (filme == null)
                    return ApiResposta<bool>.Falha("Filme nao encontrado.");

                await _repositorio.ReativarAsync(id);
                return ApiResposta<bool>.Ok(true, "Filme reativado com sucesso.");
            }
            catch (Exception ex)
            {
                return ApiResposta<bool>.Falha($"Erro ao reativar filme: {ex.Message}");
            }
        }
    }
}
