// Nome do arquivo: CategoriaServico.cs
// Objetivo: Implementacao do servico de categorias
// Camada: Application
// Como participa: Regras de negocio e orquestracao entre repositorios e AutoMapper para Categorias

using AutoMapper;
using SenacFlix.Application.DTOs;
using SenacFlix.Application.Servicos.Interfaces;
using SenacFlix.Domain.Entidades;
using SenacFlix.Domain.Interfaces;

namespace SenacFlix.Application.Servicos.Implementacoes
{
    public class CategoriaServico : ICategoriaServico
    {
        private readonly ICategoriaRepositorio _repositorio;
        private readonly IMapper _mapper;

        public CategoriaServico(ICategoriaRepositorio repositorio, IMapper mapper)
        {
            // DI - Dependency Injection
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ApiResposta<IEnumerable<CategoriaDto>>> ObterTodasAsync(bool incluirInativas = true)
        {
            try
            {
                var categorias = await _repositorio.ObterTodasAsync(incluirInativas);
                var dtos = _mapper.Map<IEnumerable<CategoriaDto>>(categorias);
                return ApiResposta<IEnumerable<CategoriaDto>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                return ApiResposta<IEnumerable<CategoriaDto>>.Falha($"Erro ao obter categorias: {ex.Message}");
            }
        }

        public async Task<ApiResposta<CategoriaDto>> ObterPorIdAsync(int id)
        {
            try
            {
                var categoria = await _repositorio.ObterPorIdAsync(id);
                if (categoria == null) return ApiResposta<CategoriaDto>.Falha("Categoria não encontrada");

                var dto = _mapper.Map<CategoriaDto>(categoria);
                return ApiResposta<CategoriaDto>.Ok(dto);
                
            }
            catch (Exception ex)
            {
                return ApiResposta<CategoriaDto>.Falha($"Erro ao obter a categoria: {ex.Message}");
            }
        }

        public async Task<ApiResposta<CategoriaDto>> CadastrarAsync(CriarCategoriaDto dto)
        {
            try
            {
                var todasCategorias = await _repositorio.ObterTodasAsync(true);
                foreach (var categoria in todasCategorias)
                {
                    // Compara se o nome que está percorrendo no foreach é igual a algum nome já existente nas Categorias.
                    if (categoria.Nome.Equals(dto.Nome, StringComparison.OrdinalIgnoreCase))
                        return ApiResposta<CategoriaDto>.Falha("Já existe uma categoria com este nome.");
                }

                var categoriaMapeada = _mapper.Map<Categoria>(dto);
                var categoriaCadastrada = await _repositorio.AdicionarAsync(categoriaMapeada);
                var categoriaDto = _mapper.Map<CategoriaDto>(categoriaCadastrada);

                return ApiResposta<CategoriaDto>.Ok(categoriaDto, "Categoria cadastrada com sucesso!");
            }
            catch (Exception ex)
            {
                return ApiResposta<CategoriaDto>.Falha($"Erro ao cadastrar categoria: {ex.Message}");
            }
        }

        public async Task<ApiResposta<CategoriaDto>> AtualizarAsync(int id, CriarCategoriaDto dto)
        {
            try
            {
                var categoriaExistente = await _repositorio.ObterPorIdAsync(id);
                if (categoriaExistente == null)
                    return ApiResposta<CategoriaDto>.Falha("Categoria não encontrada.");

                var todasCategorias = await _repositorio.ObterTodasAsync(true);
                foreach (var categoria in todasCategorias)
                {
                    if (categoria.Id != id && categoria.Nome.Equals(dto.Nome, StringComparison.OrdinalIgnoreCase))
                        return ApiResposta<CategoriaDto>.Falha("Já existe outra categoria com este nome.");
                }

                _mapper.Map(dto, categoriaExistente);
                categoriaExistente.DataAtualizacao = DateTime.UtcNow;

                await _repositorio.AtualizarAsync(categoriaExistente);
                var categoriaDto = _mapper.Map<CategoriaDto>(categoriaExistente);

                return ApiResposta<CategoriaDto>.Ok(categoriaDto, "Categoria atualizada com sucesso!");

            }
            catch (Exception ex)
            {
                return ApiResposta<CategoriaDto>.Falha($"Erro ao atualizar a categoria: {ex.Message}");
            }
        }

        public async Task<ApiResposta<bool>> DesativarAsync(int id)
        {
            try
            {
                var categoria = await _repositorio.ObterPorIdAsync(id);

                if (categoria == null)
                    return ApiResposta<bool>.Falha("Categoria não encontrada");

                await _repositorio.DesativarAsync(id);
                return ApiResposta<bool>.Ok(true, "Categoria desativada com sucesso");
            }
            catch (Exception ex)
            {
                return ApiResposta<bool>.Falha($"Erro ao desativar a categoria: {ex.Message}");
            }
        }
        public async Task<ApiResposta<bool>> ReativarAsync(int id)
        {
            try
            {
                var categoria = await _repositorio.ObterPorIdAsync(id);

                if (categoria == null)
                    return ApiResposta<bool>.Falha("Categoria não encontrada");

                await _repositorio.ReativarAsync(id);
                return ApiResposta<bool>.Ok(true, "Categoria desativada com sucesso");
            }
            catch (Exception ex)
            {
                return ApiResposta<bool>.Falha($"Erro ao reativar a categoria: {ex.Message}");
            }
        }

        public async Task<ApiResposta<bool>> ExcluirPermanentementeAsync(int id)
        {
            try
            {
                var categoria = await _repositorio.ObterPorIdAsync(id);

                if (categoria == null)
                    return ApiResposta<bool>.Falha("Categoria não encontrada");

                if (categoria.Filmes != null && categoria.Filmes.Any(f => !f.Ativo || f.Ativo))
                    return ApiResposta<bool>.Falha("Não é possível excluir uma categoria que contenha algum filme.");

                await _repositorio.ExcluirPermanentementeAsync(id);
                return ApiResposta<bool>.Ok(true, "Categoria excluída com sucesso.");
            }
            catch (Exception ex)
            {
                return ApiResposta<bool>.Falha($"Erro ao excluir a categoria: {ex.Message}");
            }
        }
    }
}
