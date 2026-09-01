// Nome do arquivo: AuditoriaDto.cs
// Objetivo: DTO para retorno de dados de log de auditoria
// Camada: Application
// Como participa: Usado pelo painel de admin para exibir as alteracoes no sistema

using System;

namespace SenacFlix.Application.DTOs
{
    public class AuditoriaDto
    {
        public int Id { get; set; }
        public string? UsuarioId { get; set; }
        public string? NomeUsuario { get; set; }
        public string Acao { get; set; } = string.Empty;
        public string TabelaAfetada { get; set; } = string.Empty;
        public string? Detalhes { get; set; }
        public DateTime DataHora { get; set; }
    }
}
