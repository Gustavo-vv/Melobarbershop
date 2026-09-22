// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Interfaces/Repositories/IUsuarioRepository.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem define: Domain (Inversão de Dependência - DIP)
// - Quem implementa: Melobarbershop.Infrastructure (UsuarioRepository via EF Core / UserManager)
// - Quem consome: Melobarbershop.Application (UsuarioService, AuthService, AgendamentoService)
// ============================================================================

using Melobarbershop.Domain.Entidades;

namespace Melobarbershop.Domain.Interfaces.Repositories;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Contrato de persistência e consultas especializadas sobre atores do sistema (ApplicationUser)
/// e bloqueios operacionais de agenda (BloqueioAgenda).
/// 
/// POR QUE EXISTE:
/// Centraliza verificações de unicidade de dados sensíveis (e-mail e telefone),
/// filtragens de usuários por cargo (ex: listar apenas profissionais com role 'Barbeiro')
/// e validações de indisponibilidade de horário por bloqueio.
/// </summary>
public interface IUsuarioRepository
{
    /// <summary>
    /// Busca um usuário pelo identificador primário GUID (string).
    /// </summary>
    Task<ApplicationUser?> ObterPorIdAsync(string id);

    /// <summary>
    /// Localiza um usuário pelo seu número de telefone cadastrado (WhatsApp/PhoneNumber).
    /// Utilizado na identificação rápida de clientes em chamadas da API de mensageria.
    /// </summary>
    Task<ApplicationUser?> ObterPorTelefoneAsync(string telefone);

    /// <summary>
    /// Localiza um usuário pelo endereço de e-mail normalizado.
    /// </summary>
    Task<ApplicationUser?> ObterPorEmailAsync(string email);

    /// <summary>
    /// Lista todos os usuários associados a um perfil específico (ex: "Barbeiro", "Cliente", "Admin").
    /// </summary>
    Task<IEnumerable<ApplicationUser>> ObterPorRoleAsync(string roleName);

    /// <summary>
    /// Lista apenas os usuários ativos vinculados ao perfil informado.
    /// Utilizado para preencher a listagem de barbeiros disponíveis para agendamento no site.
    /// </summary>
    Task<IEnumerable<ApplicationUser>> ObterAtivosPorRoleAsync(string roleName);

    /// <summary>
    /// Valida se um determinado número de telefone já está em uso por outro usuário.
    /// Permite ignorar um ID específico no caso de atualização cadastral do próprio usuário.
    /// </summary>
    Task<bool> ExisteTelefoneAsync(string telefone, string? usuarioIdIgnorar = null);

    /// <summary>
    /// Valida se um determinado e-mail já está em uso na base de dados.
    /// Permite ignorar um ID específico na edição do próprio perfil.
    /// </summary>
    Task<bool> ExisteEmailAsync(string email, string? usuarioIdIgnorar = null);

    /// <summary>
    /// Lista os bloqueios de horário registrados para um barbeiro em um período específico.
    /// </summary>
    Task<IEnumerable<BloqueioAgenda>> ObterBloqueiosPorPeriodoAsync(string barbeiroId, DateTime inicio, DateTime fim);

    /// <summary>
    /// Lista todos os bloqueios de horário de todos os barbeiros dentro do período especificado.
    /// </summary>
    Task<IEnumerable<BloqueioAgenda>> ObterBloqueiosGeraisPorPeriodoAsync(DateTime inicio, DateTime fim);

    /// <summary>
    /// Validação rápida: verifica se existe qualquer bloqueio ativo para o barbeiro que colida com [inicio, fim].
    /// </summary>
    Task<bool> ExisteBloqueioNoPeriodoAsync(string barbeiroId, DateTime inicio, DateTime fim);

    /// <summary>
    /// Busca um bloqueio específico pelo seu ID primário.
    /// </summary>
    Task<BloqueioAgenda?> ObterBloqueioPorIdAsync(int bloqueioId);

    /// <summary>
    /// Insere um novo bloqueio de agenda no banco de dados.
    /// </summary>
    Task AdicionarBloqueioAsync(BloqueioAgenda bloqueio);

    /// <summary>
    /// Exclui um bloqueio de agenda, liberando o horário para agendamento.
    /// </summary>
    Task RemoverBloqueioAsync(BloqueioAgenda bloqueio);

    /// <summary>
    /// Atualiza os dados de cadastro e preferências de um usuário no banco.
    /// </summary>
    Task AtualizarAsync(ApplicationUser usuario);
}
