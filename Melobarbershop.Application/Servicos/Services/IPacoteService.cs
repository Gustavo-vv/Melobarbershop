// ============================================================================
// Arquivo: IPacoteService.cs
// Camada: Melobarbershop.Application (Serviços - Contratos)
// Objetivo: Definir as operações de gerenciamento de combos/pacotes promocionais de serviços.
// Papel na Arquitetura:
//   - Interface que expõe métodos de CRUD e controle de ativação para pacotes comerciais.
// ============================================================================

using Melobarbershop.Application.DTOs;

namespace Melobarbershop.Application.Servicos.Services;

/// <summary>
/// Contrato do serviço de gestão de pacotes promocionais de serviços.
/// </summary>
public interface IPacoteService
{
    /// <summary>Obtém um pacote pelo ID com sua lista de serviços preenchida.</summary>
    Task<ApiResposta<PacoteDto>> ObterPorIdAsync(int id);

    /// <summary>Lista apenas os pacotes ativos para exibição comercial.</summary>
    Task<ApiResposta<IEnumerable<PacoteDto>>> ListarAtivosAsync();

    /// <summary>Lista todos os pacotes (ativos e inativos) para o painel administrativo.</summary>
    Task<ApiResposta<IEnumerable<PacoteDto>>> ListarTodosAsync();

    /// <summary>Cria um novo combo associando os IDs de serviços informados.</summary>
    Task<ApiResposta<PacoteDto>> CriarAsync(CriarPacoteDto dto);

    /// <summary>Atualiza nome, valor ou composição de serviços de um combo existente.</summary>
    Task<ApiResposta<PacoteDto>> AtualizarAsync(int id, AtualizarPacoteDto dto);

    /// <summary>Desativa um pacote, impedindo novas vendas/agendamentos do mesmo.</summary>
    Task<ApiResposta<PacoteDto>> DesativarAsync(int id);

    /// <summary>Reativa um pacote previamente desativado.</summary>
    Task<ApiResposta<PacoteDto>> AtivarAsync(int id);
}

