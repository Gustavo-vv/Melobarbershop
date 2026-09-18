// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Entidades/ApplicationUser.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem chama/usa:
//   * Melobarbershop.Application (UsuarioService, AuthService, DTOs, Mapeamentos)
//   * Melobarbershop.Infrastructure (IdentityDbContext, repositórios de usuário e agendamento)
//   * Melobarbershop.API e UI (Controladores de autenticação, perfil e administração)
//   * Melobarbershop.Desktop (Gestão de usuários e comissões de barbeiros)
// - Quem ele referencia:
//   * IdentityUser (classe base do ASP.NET Core Identity)
//   * Agendamento, BloqueioAgenda, Venda, Avaliacao (coleções de navegação)
// ============================================================================

using Microsoft.AspNetCore.Identity;

namespace Melobarbershop.Domain.Entidades;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Entidade unificada de ator do sistema, estendendo a infraestrutura de segurança do ASP.NET Core Identity.
/// 
/// POR QUE EXISTE:
/// Centraliza todos os perfis de usuários (Administrador, Barbeiro, Cliente) em uma única tabela ("AspNetUsers"),
/// diferenciando-os por Roles (Perfis) e campos específicos (como PercentualComissao para Barbeiros).
/// 
/// O QUE QUEBRARIA SE NÃO EXISTISSE:
/// Todo o subsistema de autenticação, login com JWT/Cookies, autorização baseada em cargos (RBAC)
/// e os vínculos de autoria de agendamentos e vendas deixariam de compilar.
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// Nome completo do usuário exibido em interfaces, mensagens e recibos.
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Telefone formatado com DDD utilizado preferencialmente para disparos de WhatsApp.
    /// Nota: O IdentityUser já possui a propriedade PhoneNumber herdada; este campo é usado para reforçar a integração via WhatsApp.
    /// </summary>
    public string? TelefoneWhatsApp { get; set; }

    /// <summary>
    /// Data de nascimento do cliente/barbeiro.
    /// Utilizada para felicitações de aniversário e eventuais campanhas promocionais.
    /// </summary>
    public DateTime? DataNascimento { get; set; }

    /// <summary>
    /// Anotações internas sobre preferências do cliente (ex: 'alérgico a pós-barba', 'corte militar')
    /// ou observações de equipe visíveis para o barbeiro.
    /// </summary>
    public string? PreferenciasNotas { get; set; }

    /// <summary>
    /// Caminho relativo ou URL da foto de perfil do usuário.
    /// </summary>
    public string? FotoUrl { get; set; }

    /// <summary>
    /// Percentual de comissão acordado (ex: 50.00 para 50%).
    /// Regra de negócio: Aplicável especificamente para usuários com Role 'Barbeiro' no cálculo de repasse financeiro de serviços.
    /// </summary>
    public decimal? PercentualComissao { get; set; }

    /// <summary>
    /// Data e hora do cadastro do usuário no sistema (em UTC).
    /// </summary>
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Flag de exclusão lógica (soft-delete) / suspensão de acesso.
    /// Regra de negócio: Usuários inativos não podem logar nem receber novos agendamentos na plataforma.
    /// </summary>
    public bool Ativo { get; set; } = true;

    // ========================================================================
    // PROPRIEDADES DE NAVEGAÇÃO (RELACIONAMENTOS REVERSOS EF CORE)
    // ========================================================================

    /// <summary>
    /// Histórico de agendamentos onde este usuário figura como o Cliente atendido.
    /// </summary>
    public ICollection<Agendamento> AgendamentosComoCliente { get; set; } = new List<Agendamento>();

    /// <summary>
    /// Agenda de atendimentos onde este usuário é o Barbeiro prestador do serviço.
    /// </summary>
    public ICollection<Agendamento> AgendamentosComoBarbeiro { get; set; } = new List<Agendamento>();

    /// <summary>
    /// Intervalos de bloqueio manual de agenda configurados pelo barbeiro (folgas, almoço, férias).
    /// </summary>
    public ICollection<BloqueioAgenda> BloqueiosAgenda { get; set; } = new List<BloqueioAgenda>();

    /// <summary>
    /// Vendas comerciais faturadas para este cliente ou registradas pelo usuário.
    /// </summary>
    public ICollection<Venda> Vendas { get; set; } = new List<Venda>();

    /// <summary>
    /// Avaliações emitidas por este cliente sobre os serviços recebidos.
    /// </summary>
    public ICollection<Avaliacao> AvaliacoesComoCliente { get; set; } = new List<Avaliacao>();

    /// <summary>
    /// Avaliações recebidas por este profissional acerca do seu atendimento.
    /// </summary>
    public ICollection<Avaliacao> AvaliacoesComoBarbeiro { get; set; } = new List<Avaliacao>();
}