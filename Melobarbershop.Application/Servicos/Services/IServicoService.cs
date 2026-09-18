// ============================================================================
// Arquivo: IServicoService.cs
// Camada: Melobarbershop.Application (Serviços - Contratos)
// Objetivo: Definir as operações de negócio para o catálogo de procedimentos
//           e serviços executados pelos profissionais.
// Papel na Arquitetura:
//   - Interface que desacopla os controladores do gerenciamento de serviços.
// ============================================================================

using Melobarbershop.Application.DTOs;

namespace Melobarbershop.Application.Servicos.Services;

/// <summary>
/// Contrato do serviço de catálogo de serviços da barbearia.
/// </summary>
public interface IServicoService
{
    /// <summary>Lista serviços cadastrados, com opção de incluir os desativados para telas administrativas.</summary>
    Task<ApiResposta<IEnumerable<ServicoDto>>> ListarAsync(bool incluirInativos = false);

    /// <summary>Busca um serviço específico pelo ID.</summary>
    Task<ApiResposta<ServicoDto>> ObterPorIdAsync(int id);

    /// <summary>Cadastra um novo serviço com preço e duração padrão em minutos.</summary>
    Task<ApiResposta<ServicoDto>> CriarAsync(CriarServicoDto dto);

    /// <summary>Atualiza nome, descrição, preço ou duração de um serviço existente.</summary>
    Task<ApiResposta<ServicoDto>> AtualizarAsync(int id, AtualizarServicoDto dto);

    /// <summary>Desativa um serviço, ocultando-o de novos agendamentos.</summary>
    Task<ApiResposta<bool>> DesativarAsync(int id);

    /// <summary>Reativa um serviço inativo.</summary>
    Task<ApiResposta<bool>> AtivarAsync(int id);

    /// <summary>Exclui fisicamente o registro de um serviço da base de dados caso não tenha vínculos históricos.</summary>
    Task<ApiResposta<bool>> RemoverPermanentementeAsync(int id);
}

