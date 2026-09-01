// Nome do arquivo: FavoritoDto.cs
// Objetivo: DTOs relacionados aos favoritos do usuario
// Camada: Application
// Como participa: Trafega dados de favoritos entre API e cliente

using System;
using System.ComponentModel.DataAnnotations;

namespace SenacFlix.Application.DTOs
{
    public class FavoritoDto
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; } = string.Empty;
        public int FilmeId { get; set; }
        public string FilmeTitulo { get; set; } = string.Empty;
        public string? FilmeImagemCapaUrl { get; set; }
        public string FilmeCategoriaNome { get; set; } = string.Empty;
        public DateTime DataFavorito { get; set; }
    }

    public class AdicionarFavoritoDto
    {
        [Required(ErrorMessage = "O Filme e obrigatorio.")]
        public int FilmeId { get; set; }
    }
}
