// ============================================================================
// Arquivo: UsuarioDtos.cs
// Camada: Melobarbershop.Application (Data Transfer Objects - DTOs)
// Objetivo: Definir os contratos de transferência de dados de usuários (clientes, barbeiros, administradores)
//           e bloqueios de agenda de profissionais.
// Papel na Arquitetura:
//   - Isola o modelo do ASP.NET Core Identity (ApplicationUser) das camadas de apresentação (API/Desktop/Web).
//   - Contém anotações de validação (DataAnnotations) como [Required], [EmailAddress], [Compare]
//     para validação declarativa de payloads de entrada antes da execução das regras de negócio.
// ============================================================================

using System.ComponentModel.DataAnnotations;

namespace Melobarbershop.Application.DTOs;

/// <summary>
/// DTO de saída com as informações de perfil, contato e papéis de um usuário cadastrado.
/// </summary>
public class UsuarioDto
{
    /// <summary>Identificador único do usuário no Identity (formato GUID em string).</summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>Nome civil completo do usuário.</summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>Endereço de e-mail cadastrado (utilizado para login e notificações).</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Número de telefone/celular do usuário.
    /// Nota: Mapeado da coluna PhoneNumber do ASP.NET Core Identity.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>Data de nascimento do cliente (utilizada para envio de promoções de aniversário).</summary>
    public DateTime? DataNascimento { get; set; }

    /// <summary>Anotações personalizadas de preferências de corte, estilo ou restrições.</summary>
    public string? PreferenciasNotas { get; set; }

    /// <summary>URL da fotografia de perfil do usuário.</summary>
    public string? FotoUrl { get; set; }

    /// <summary>
    /// Percentual de comissão acordado com o barbeiro (0 a 100).
    /// Preenchido apenas para usuários com perfil de Barbeiro/Profissional.
    /// </summary>
    public decimal? PercentualComissao { get; set; }

    /// <summary>Indica se a conta está ativa ou desativada no sistema.</summary>
    public bool Ativo { get; set; }

    /// <summary>Data e hora do cadastro do usuário na base de dados.</summary>
    public DateTime DataCadastro { get; set; }

    /// <summary>Coleção dos nomes dos papéis (roles) atribuídos ao usuário (ex: "Admin", "Barbeiro", "Cliente").</summary>
    public IList<string> Roles { get; set; } = new List<string>();
}

/// <summary>
/// DTO de entrada para registro/autocadastro de um novo cliente ou usuário no sistema.
/// </summary>
public class CriarUsuarioDto
{
    /// <summary>Nome completo do novo usuário.</summary>
    [Required(ErrorMessage = "O Nome Completo e obrigatorio.")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>E-mail de acesso único com validação de formato.</summary>
    [Required(ErrorMessage = "O E-mail e obrigatorio.")]
    [EmailAddress(ErrorMessage = "E-mail em formato invalido.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>Senha em texto plano enviada para hash seguro via Identity PasswordHasher.</summary>
    [Required(ErrorMessage = "A Senha e obrigatoria.")]
    [MinLength(6, ErrorMessage = "A senha deve ter no minimo 6 caracteres.")]
    public string Senha { get; set; } = string.Empty;

    /// <summary>Confirmação da senha para evitar erros de digitação no cadastro.</summary>
    [Required(ErrorMessage = "A confirmacao de senha e obrigatoria.")]
    [Compare("Senha", ErrorMessage = "As senhas nao coincidem.")]
    public string ConfirmarSenha { get; set; } = string.Empty;

    /// <summary>Telefone celular/WhatsApp informado pelo cliente no cadastro.</summary>
    [Required(ErrorMessage = "O Telefone é obrigatorio.")]
    public string TelefoneWhatsApp { get; set; } = string.Empty;

    /// <summary>Data de nascimento opcional.</summary>
    public DateTime? DataNascimento { get; set; }

    /// <summary>Observações de corte informadas opcionalmente pelo cliente.</summary>
    public string? PreferenciasNotas { get; set; }

    /// <summary>URL opcional de foto de perfil.</summary>
    public string? FotoUrl { get; set; }
}

/// <summary>
/// DTO de entrada para autenticação de credenciais básicas (e-mail e senha).
/// </summary>
public class LoginDto
{
    /// <summary>E-mail cadastrado da conta.</summary>
    [Required(ErrorMessage = "O Email é obrigatório!")]
    public string Email { get; set; } = string.Empty;

    /// <summary>Senha de acesso.</summary>
    [Required(ErrorMessage = "A Senha é obrigatória!")]
    public string Senha { get; set; } = string.Empty;
}

/// <summary>
/// DTO de entrada para edição dos dados cadastrais do perfil do usuário.
/// </summary>
public class AtualizarUsuarioDto
{
    /// <summary>Nome atualizado do usuário.</summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>Número de telefone atualizado.</summary>
    public string? TelefoneWhatsApp { get; set; }

    /// <summary>Data de nascimento corrigida/atualizada.</summary>
    public DateTime? DataNascimento { get; set; }

    /// <summary>Notas de preferência atualizadas.</summary>
    public string? PreferenciasNotas { get; set; }

    /// <summary>URL da foto de perfil atualizada.</summary>
    public string? FotoUrl { get; set; }

    /// <summary>Percentual de comissão atualizado (se for barbeiro).</summary>
    public decimal? PercentualComissao { get; set; }

    /// <summary>Situação cadastral do usuário.</summary>
    public bool Ativo { get; set; } = true;
}

/// <summary>
/// DTO de entrada para interdição de horários de um barbeiro (almoço, folga, consulta).
/// </summary>
public class CriarBloqueioAgendaDto
{
    /// <summary>Identificador do barbeiro que terá o horário bloqueado.</summary>
    public string BarbeiroId { get; set; } = string.Empty;

    /// <summary>Início do período indisponível.</summary>
    public DateTime DataHoraInicio { get; set; }

    /// <summary>Fim do período indisponível.</summary>
    public DateTime DataHoraFim { get; set; }

    /// <summary>Justificativa do bloqueio para exibição na grade de agendamentos.</summary>
    public string Motivo { get; set; } = string.Empty;
}

/// <summary>
/// DTO de leitura de um bloqueio de agenda cadastrado.
/// </summary>
public class BloqueioAgendaDto
{
    /// <summary>Identificador do bloqueio no banco de dados.</summary>
    public int Id { get; set; }

    /// <summary>Identificador do barbeiro associado.</summary>
    public string BarbeiroId { get; set; } = string.Empty;

    /// <summary>Nome do barbeiro (obtido via join para apresentação amigável).</summary>
    public string NomeBarbeiro { get; set; } = string.Empty;

    /// <summary>Data e hora inicial do bloqueio.</summary>
    public DateTime DataHoraInicio { get; set; }

    /// <summary>Data e hora final do bloqueio.</summary>
    public DateTime DataHoraFim { get; set; }

    /// <summary>Descrição do motivo do bloqueio.</summary>
    public string Motivo { get; set; } = string.Empty;
}

