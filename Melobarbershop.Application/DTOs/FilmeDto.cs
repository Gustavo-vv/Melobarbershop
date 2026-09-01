// Nome do arquivo: FilmeDto.cs
// Objetivo: Objetos de Transferencia de Dados (DTOs) relacionados a entidade Filme.
// Camada: Application
// Como participa: Usado para receber dados da API (CriarFilmeDto, AtualizarFilmeDto) e enviar dados para os clientes (FilmeDto), escondendo a entidade original do banco de dados.

using System;
using System.ComponentModel.DataAnnotations;

namespace SenacFlix.Application.DTOs
{
    // DTO utilizado para enviar dados do filme aos clientes (Leitura)
    public class FilmeDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int AnoLancamento { get; set; }
        
        // Duracao em minutos (ex: 169). A formatacao para exibicao ocorre na View/ViewModel.
        public int Duracao { get; set; }
        
        public string? Diretor { get; set; }
        public string? Elenco { get; set; }
        public string? ImagemCapaUrl { get; set; }
        public string? ImagemBannerUrl { get; set; }
        public string? TrailerYoutubeUrl { get; set; }
        public string? VideoYoutubeUrl { get; set; }
        
        // Dados achatados da categoria
        public int CategoriaId { get; set; }
        public string CategoriaNome { get; set; } = string.Empty;
        
        // Dados achatados da classificacao indicativa
        public int ClassificacaoIndicativaId { get; set; }
        public string ClassificacaoNome { get; set; } = string.Empty;
        public string ClassificacaoCor { get; set; } = string.Empty;
        public int ClassificacaoIdadeMinima { get; set; }
        
        public bool Ativo { get; set; }
        public bool DestaqueHome { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public DateTime? DataExclusao { get; set; }

        /// <summary>
        /// Propriedade calculada para exibicao formatada da duracao. Nunca e armazenada nem enviada pela API.
        /// Exemplos: 169 minutos -> "2h 49min" | 90 minutos -> "1h 30min" | 45 minutos -> "45min"
        /// </summary>
        public string DuracaoFormatada
        {
            get
            {
                if (Duracao <= 0) return "Desconhecida";
                int horas = Duracao / 60;
                int mins = Duracao % 60;
                if (horas > 0 && mins > 0) return $"{horas}h {mins}min";
                if (horas > 0) return $"{horas}h";
                return $"{mins}min";
            }
        }
    }

    // DTO utilizado para receber dados de criacao de um novo filme (Escrita)
    public class CriarFilmeDto
    {
        [Required(ErrorMessage = "O Titulo e obrigatorio.")]
        [MaxLength(200, ErrorMessage = "O Titulo nao pode ter mais que 200 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Descricao e obrigatoria.")]
        public string Descricao { get; set; } = string.Empty;

        [Range(1900, 2100, ErrorMessage = "Ano de lancamento invalido.")]
        public int AnoLancamento { get; set; }

        [Range(1, 1000, ErrorMessage = "A duracao deve ser em minutos (maior que zero).")]
        public int Duracao { get; set; } // Em minutos

        [MaxLength(150, ErrorMessage = "O nome do Diretor nao pode exceder 150 caracteres.")]
        public string? Diretor { get; set; }

        public string? Elenco { get; set; }
        public string? ImagemCapaUrl { get; set; }
        public string? ImagemBannerUrl { get; set; }
        public string? TrailerYoutubeUrl { get; set; }
        public string? VideoYoutubeUrl { get; set; }

        [Required(ErrorMessage = "A Categoria e obrigatoria.")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "A Classificacao Indicativa e obrigatoria.")]
        public int ClassificacaoIndicativaId { get; set; }

        public bool DestaqueHome { get; set; } = false;
    }

    // DTO utilizado para receber dados de atualizacao de um filme existente (Escrita)
    // Herda de CriarFilmeDto pois a maioria das regras e campos sao iguais, adicionando apenas o Id.
    public class AtualizarFilmeDto : CriarFilmeDto
    {
        [Required(ErrorMessage = "O Id e obrigatorio para atualizar.")]
        public int Id { get; set; }
    }
}
