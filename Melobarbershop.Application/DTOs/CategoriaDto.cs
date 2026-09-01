// Nome do arquivo: CategoriaDto.cs
// Objetivo: Objetos de Transferencia de Dados (DTOs) relacionados a entidade Categoria.
// Camada: Application
// Como participa: Usado para trafegar os dados de categoria entre a API e as aplicacoes clientes (MVC, Desktop).

using System;
using System.ComponentModel.DataAnnotations;

namespace SenacFlix.Application.DTOs
{
    // DTO utilizado para retorno de dados (Leitura)
    public class CategoriaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        
        // Quantidade total de filmes vinculados a esta categoria
        public int TotalFilmes { get; set; }
    }

    // DTO utilizado para criacao e atualizacao de categorias (Escrita)
    public class CriarCategoriaDto
    {
        [Required(ErrorMessage = "O Nome da categoria e obrigatorio.")]
        [MaxLength(100, ErrorMessage = "O Nome nao pode exceder 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "A Descricao nao pode exceder 500 caracteres.")]
        public string? Descricao { get; set; }
    }
}
