using System.Collections.Generic;

namespace SenacFlix.Application.DTOs
{
    public class DashboardEstatisticasDto
    {
        public int TotalFilmes { get; set; }
        public int FilmesAtivos { get; set; }
        public int FilmesDesativados { get; set; }
        public int TotalCategorias { get; set; }
        public int TotalUsuarios { get; set; }
        public int TotalAdministradores { get; set; }
        public int TotalOperadores { get; set; }
        public int TotalClientes { get; set; }
        public int TotalFavoritos { get; set; }
        
        // Lista de tipo referêncial, criação de lista de GraficoItemDto, cada item  da lista contém Label e Valor
        public List<GraficoItemDto> FilmesPorCategoria { get; set; } = new();
        public List<GraficoItemDto> FilmesPorClassificacao { get; set; } = new();
        public List<GraficoItemDto> FilmesPorAno { get; set; } = new();
        public List<GraficoItemDto> UsuariosPorPerfil { get; set; } = new();
        public List<GraficoItemDto> FavoritosPorCategoria { get; set; } = new();
        public List<GraficoItemDto> Top10FilmesFavoritados { get; set; } = new();
    }

    public class GraficoItemDto
    {
        public string Label { get; set; } = string.Empty;
        public int Valor { get; set; }
    }
}