using System;
using System.Linq;
using System.Threading.Tasks;
using SenacFlix.Application.DTOs;
using SenacFlix.Application.Servicos.Interfaces;
using SenacFlix.Domain.Interfaces;

namespace SenacFlix.Application.Servicos.Implementacoes
{
    public class EstatisticasServico : IEstatisticasServico
    {
        private readonly IEstatisticasRepositorio _repositorio;

        public EstatisticasServico(IEstatisticasRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<ApiResposta<DashboardEstatisticasDto>> ObterEstatisticasDashboardAsync()
        {
            try
            {
                var stats = await _repositorio.ObterEstatisticasDashboardAsync();
                
                var dto = new DashboardEstatisticasDto
                {
                    TotalFilmes = stats.TotalFilmes,
                    FilmesAtivos = stats.FilmesAtivos,
                    FilmesDesativados = stats.FilmesDesativados,
                    TotalCategorias = stats.TotalCategorias,
                    TotalUsuarios = stats.TotalUsuarios,
                    TotalAdministradores = stats.TotalAdministradores,
                    TotalOperadores = stats.TotalOperadores,
                    TotalClientes = stats.TotalClientes,
                    TotalFavoritos = stats.TotalFavoritos,
                    FilmesPorCategoria = stats.FilmesPorCategoria.Select(g => new GraficoItemDto { Label = g.Label, Valor = g.Valor }).ToList(),
                    FilmesPorClassificacao = stats.FilmesPorClassificacao.Select(g => new GraficoItemDto { Label = g.Label, Valor = g.Valor }).ToList(),
                    FilmesPorAno = stats.FilmesPorAno.Select(g => new GraficoItemDto { Label = g.Label, Valor = g.Valor }).ToList(),
                    UsuariosPorPerfil = stats.UsuariosPorPerfil.Select(g => new GraficoItemDto { Label = g.Label, Valor = g.Valor }).ToList(),
                    FavoritosPorCategoria = stats.FavoritosPorCategoria.Select(g => new GraficoItemDto { Label = g.Label, Valor = g.Valor }).ToList(),
                    Top10FilmesFavoritados = stats.Top10FilmesFavoritados.Select(g => new GraficoItemDto { Label = g.Label, Valor = g.Valor }).ToList()
                };

                return ApiResposta<DashboardEstatisticasDto>.Ok(dto);
            }
            catch (Exception ex)
            {
                return ApiResposta<DashboardEstatisticasDto>.Falha($"Erro ao gerar estatísticas: {ex.Message}");
            }
        }
    }
}