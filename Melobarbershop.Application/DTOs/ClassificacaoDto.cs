// Nome do arquivo: ClassificacaoDto.cs
// Objetivo: DTO de leitura para Classificacao Indicativa
// Camada: Application
// Como participa: Usado para fornecer a lista de classificacoes disponiveis (Livre, 10, 12, etc.)

namespace SenacFlix.Application.DTOs
{
    public class ClassificacaoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int IdadeMinima { get; set; }
        public string? Descricao { get; set; }
        public string Cor { get; set; } = string.Empty;
    }
}
