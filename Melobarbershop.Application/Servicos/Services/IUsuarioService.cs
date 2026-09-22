// ============================================================================
// Arquivo: IUsuarioService.cs
// Camada: Melobarbershop.Application (Serviços - Contratos)
// Objetivo: Definir os contratos de negócio para gestão de usuários,
//           controle de papéis (Roles) e gerenciamento de bloqueios de agenda.
// Papel na Arquitetura:
//   - Interface que abstrai o ASP.NET Core Identity (UserManager, RoleManager).
//   - Fornece busca inteligente por telefone/e-mail e validação de janelas de disponibilidade de barbeiros.
// ============================================================================

using Melobarbershop.Application.DTOs;

namespace Melobarbershop.Application.Servicos.Services;

/// <summary>
/// Contrato do serviço de usuários, perfis de acesso e agenda de trabalho dos barbeiros.
/// </summary>
public interface IUsuarioService
{
    /// <summary>Obtém os dados completos de um usuário a partir do seu ID (GUID em string).</summary>
    Task<UsuarioDto?> ObterPorIdAsync(string id);

    /// <summary>Localiza um usuário pelo número de telefone cadastrado.</summary>
    Task<UsuarioDto?> ObterPorTelefoneAsync(string telefone);

    /// <summary>Localiza um usuário pelo e-mail cadastrado.</summary>
    Task<UsuarioDto?> ObterPorEmailAsync(string email);

    /// <summary>Lista usuários filtrando pelo perfil (ex: "Barbeiro", "Cliente", "Admin"), com opção de apenas ativos.</summary>
    Task<IEnumerable<UsuarioDto>> ListarPorRoleAsync(string roleName, bool apenasAtivos = true);

    /// <summary>Cria um novo usuário com senha criptografada via Identity e associa ao perfil padrão de Cliente.</summary>
    Task<UsuarioDto> CriarAsync(CriarUsuarioDto dto);

    /// <summary>Atualiza os dados cadastrais (nome, telefone, comissão, preferências) de um usuário existente.</summary>
    Task<UsuarioDto> AtualizarAsync(string id, AtualizarUsuarioDto dto);

    /// <summary>Inativa a conta do usuário no sistema.</summary>
    Task DesativarAsync(string id);

    /// <summary>Reativa a conta de um usuário desativado.</summary>
    Task AtivarAsync(string id);

    /// <summary>Registra um bloqueio na agenda do barbeiro para impedir agendamentos no intervalo.</summary>
    Task<BloqueioAgendaDto> AdicionarBloqueioAgendaAsync(CriarBloqueioAgendaDto dto);

    /// <summary>Remove um bloqueio de agenda previamente cadastrado.</summary>
    Task RemoverBloqueioAgendaAsync(int bloqueioId);

    /// <summary>Lista os períodos bloqueados de um barbeiro dentro de um intervalo de datas.</summary>
    Task<IEnumerable<BloqueioAgendaDto>> ListarBloqueiosBarbeiroAsync(string barbeiroId, DateTime inicio, DateTime fim);

    /// <summary>Lista todos os períodos bloqueados de todos os barbeiros ou opcionalmente filtrado por barbeiro.</summary>
    Task<IEnumerable<BloqueioAgendaDto>> ListarBloqueiosAsync(string? barbeiroId, DateTime inicio, DateTime fim);

    /// <summary>Verifica se o barbeiro está livre (sem bloqueios nem agendamentos concomitantes) no intervalo solicitado.</summary>
    Task<bool> VerificarDisponibilidadeBarbeiroAsync(string barbeiroId, DateTime inicio, DateTime fim);
}

