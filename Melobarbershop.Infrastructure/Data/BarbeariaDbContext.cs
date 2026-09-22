// ============================================================================
// Arquivo: BarbeariaDbContext.cs
// Camada: Melobarbershop.Infrastructure (Data)
// Objetivo: Representar o contexto principal do Entity Framework Core integrado
//           ao ASP.NET Core Identity para persistência relacional da barbearia.
// Papel na Arquitetura:
//   - Mapeia entidades de domínio (Servicos, Pacotes, Agendamentos, Vendas, etc.) para tabelas do SQL Server.
//   - Configura os nomes em português para as tabelas de segurança do Identity.
//   - Carrega automaticamente as classes Fluent API (IEntityTypeConfiguration) do assembly.
// ============================================================================

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Melobarbershop.Domain.Entidades;

namespace Melobarbershop.Infrastructure.Data;

/// <summary>
/// Contexto do Entity Framework Core que gerencia a conexão e mapeamento de dados da barbearia.
/// </summary>
public class BarbeariaDbContext : IdentityDbContext<ApplicationUser>
{
    /// <summary>
    /// Construtor que recebe as opções de configuração do DbContext via injeção de dependência.
    /// </summary>
    public BarbeariaDbContext(DbContextOptions<BarbeariaDbContext> options) : base(options)
    {
    }

    /// <summary>Conjunto de dados de serviços da barbearia.</summary>
    public DbSet<Servico> Servicos => Set<Servico>();

    /// <summary>Conjunto de dados de pacotes promocionais de serviços.</summary>
    public DbSet<Pacote> Pacotes => Set<Pacote>();

    /// <summary>Tabela associativa entre pacotes e serviços.</summary>
    public DbSet<PacoteItem> PacoteItens => Set<PacoteItem>();

    /// <summary>Conjunto de dados de agendamentos de clientes.</summary>
    public DbSet<Agendamento> Agendamentos => Set<Agendamento>();

    /// <summary>Tabela associativa de serviços contratados no agendamento.</summary>
    public DbSet<AgendamentoItem> AgendamentoItens => Set<AgendamentoItem>();

    /// <summary>Bloqueios e pausas na agenda dos barbeiros.</summary>
    public DbSet<BloqueioAgenda> BloqueiosAgenda => Set<BloqueioAgenda>();

    /// <summary>Conjunto de dados de produtos para venda e estoque.</summary>
    public DbSet<Produto> Produtos => Set<Produto>();

    /// <summary>Histórico de movimentações de estoque (entradas, saídas, perdas).</summary>
    public DbSet<MovimentacaoEstoque> MovimentacoesEstoque => Set<MovimentacaoEstoque>();

    /// <summary>Registro de vendas e comandas de atendimento.</summary>
    public DbSet<Venda> Vendas => Set<Venda>();

    /// <summary>Itens individuais (serviços ou produtos) associados à venda.</summary>
    public DbSet<VendaItem> VendaItens => Set<VendaItem>();

    /// <summary>Pagamentos efetuados nas vendas (PIX, Cartão, Dinheiro).</summary>
    public DbSet<Pagamento> Pagamentos => Set<Pagamento>();

    /// <summary>Avaliações de satisfação e notas de clientes.</summary>
    public DbSet<Avaliacao> Avaliacoes => Set<Avaliacao>();

    /// <summary>Modelos de mensagens e notificações automáticas.</summary>
    public DbSet<TemplateMensagem> TemplatesMensagem => Set<TemplateMensagem>();

    /// <summary>Logs de disparo e entrega de notificações.</summary>
    public DbSet<NotificacaoLog> NotificacoesLog => Set<NotificacaoLog>();

    /// <summary>Configuração de horários de funcionamento padrão por dia da semana.</summary>
    public DbSet<HorarioFuncionamento> HorariosFuncionamento => Set<HorarioFuncionamento>();

    /// <summary>Exceções e horários de funcionamento especiais/feriados por data.</summary>
    public DbSet<HorarioEspecial> HorariosEspeciais => Set<HorarioEspecial>();

    /// <summary>
    /// Configurações avançadas do modelo relacional, nomes de tabelas Identity e carregamento de configurações de entidades.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Mapeamento semântico das tabelas padrão do ASP.NET Identity para português
        modelBuilder.Entity<ApplicationUser>(b => b.ToTable("Usuarios"));
        modelBuilder.Entity<IdentityRole>(b => b.ToTable("Perfis"));
        modelBuilder.Entity<IdentityUserRole<string>>(b => b.ToTable("UsuarioPerfis"));
        modelBuilder.Entity<IdentityUserClaim<string>>(b => b.ToTable("UsuarioClaims"));
        modelBuilder.Entity<IdentityRoleClaim<string>>(b => b.ToTable("PerfilClaims"));
        modelBuilder.Entity<IdentityUserLogin<string>>(b => b.ToTable("UsuarioLogins"));
        modelBuilder.Entity<IdentityUserToken<string>>(b => b.ToTable("UsuarioTokens"));

        // Aplica automaticamente todas as classes que implementam IEntityTypeConfiguration<T> deste assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BarbeariaDbContext).Assembly);
    }
}
