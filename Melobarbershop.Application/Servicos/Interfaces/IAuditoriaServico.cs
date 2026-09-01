// Nome do arquivo: IAuditoriaServico.cs
// Objetivo: Interface para serviços de logs de auditoria
// Camada: Application
// Como participa: Fornece metodos para registrar ações criticas no sistema e consulta-las

using SenacFlix.Application.DTOs;

namespace SenacFlix.Application.Servicos.Interfaces
{
    public interface IAuditoriaServico
    {
        Task<ApiResposta<IEnumerable<AuditoriaDto>>> ObterTodasAsync();
        Task RegistrarAsync(string usuarioId, string nomeUsuario, string acao, string tabela, string? detalhes = null);
    }
}
