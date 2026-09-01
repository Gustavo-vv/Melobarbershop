// Nome do arquivo: PerfilMapeamento.cs
// Objetivo: Centralizar as configuracoes de mapeamento entre Entidades e DTOs usando AutoMapper.
// Camada: Application
// Como participa: O AutoMapper utiliza essa classe para saber como transformar um Filme em FilmeDto, por exemplo.

using AutoMapper;
using SenacFlix.Application.DTOs;
using SenacFlix.Domain.Entidades;
using System;

namespace SenacFlix.Application.Mapeamentos
{
    public class PerfilMapeamento : Profile
    {
        public PerfilMapeamento()
        {
            // =========================================================================
            // MAPEAMENTOS DE FILME
            // =========================================================================
            
            // Entidade -> DTO (Leitura)
            CreateMap<Filme, FilmeDto>()
                // Mapeia o nome da categoria vindo do relacionamento
                .ForMember(dest => dest.CategoriaNome, opt => opt.MapFrom(src => src.Categoria != null ? src.Categoria.Nome : string.Empty))
                // Mapeia os dados da classificacao indicativa
                .ForMember(dest => dest.ClassificacaoNome, opt => opt.MapFrom(src => src.ClassificacaoIndicativa != null ? src.ClassificacaoIndicativa.Nome : string.Empty))
                .ForMember(dest => dest.ClassificacaoCor, opt => opt.MapFrom(src => src.ClassificacaoIndicativa != null ? src.ClassificacaoIndicativa.Cor : string.Empty))
                .ForMember(dest => dest.ClassificacaoIdadeMinima, opt => opt.MapFrom(src => src.ClassificacaoIndicativa != null ? src.ClassificacaoIndicativa.IdadeMinima : 0))
                // DataAtualizacao e DataExclusao sao mapeados diretamente (mesmos nomes)
                .ForMember(dest => dest.Duracao, opt => opt.MapFrom(src => src.Duracao)); // int -> int, formatacao feita na ViewModel/View

            // DTO -> Entidade (Criacao)
            CreateMap<CriarFilmeDto, Filme>()
                .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => true))
                // Ignora campos que nao vem do DTO e sao gerados/gerenciados automaticamente
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Categoria, opt => opt.Ignore())
                .ForMember(dest => dest.ClassificacaoIndicativa, opt => opt.Ignore())
                .ForMember(dest => dest.Favoritos, opt => opt.Ignore())
                .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
                .ForMember(dest => dest.DataExclusao, opt => opt.Ignore());

            // DTO -> Entidade (Atualizacao)
            CreateMap<AtualizarFilmeDto, Filme>()
                .ForMember(dest => dest.Categoria, opt => opt.Ignore())
                .ForMember(dest => dest.ClassificacaoIndicativa, opt => opt.Ignore())
                .ForMember(dest => dest.Favoritos, opt => opt.Ignore())
                .ForMember(dest => dest.DataCadastro, opt => opt.Ignore())
                .ForMember(dest => dest.DataExclusao, opt => opt.Ignore())
                .ForMember(dest => dest.Ativo, opt => opt.Ignore());

            // =========================================================================
            // MAPEAMENTOS DE CATEGORIA
            // =========================================================================
            
            CreateMap<Categoria, CategoriaDto>()
                // Conta o numero de filmes ativos vinculados a esta categoria
                .ForMember(dest => dest.TotalFilmes, opt => opt.MapFrom(src => src.Filmes != null ? src.Filmes.Count : 0));

            CreateMap<CriarCategoriaDto, Categoria>()
                .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Filmes, opt => opt.Ignore())
                .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
                .ForMember(dest => dest.DataExclusao, opt => opt.Ignore());

            // =========================================================================
            // MAPEAMENTOS DE USUARIO
            // =========================================================================
            
            CreateMap<ApplicationUser, UsuarioDto>()
                // Perfis deverao ser preenchidos manualmente pelo servico, pois o Identity gerencia as roles de forma complexa
                .ForMember(dest => dest.Perfis, opt => opt.Ignore());

            CreateMap<RegistrarUsuarioDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email)) // UserName no Identity sera o Email
                .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => true));

            // =========================================================================
            // MAPEAMENTOS GERAIS
            // =========================================================================
            
            CreateMap<Favorito, FavoritoDto>()
                .ForMember(dest => dest.FilmeTitulo, opt => opt.MapFrom(src => src.Filme != null ? src.Filme.Titulo : string.Empty))
                .ForMember(dest => dest.FilmeImagemCapaUrl, opt => opt.MapFrom(src => src.Filme != null ? src.Filme.ImagemCapaUrl : string.Empty))
                .ForMember(dest => dest.FilmeCategoriaNome, opt => opt.MapFrom(src => (src.Filme != null && src.Filme.Categoria != null) ? src.Filme.Categoria.Nome : string.Empty));

            CreateMap<ClassificacaoIndicativa, ClassificacaoDto>();
            
            CreateMap<Auditoria, AuditoriaDto>();
        }

    }
}
