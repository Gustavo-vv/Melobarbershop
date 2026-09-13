using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Melobarbershop.Domain.Entidades;

namespace Melobarbershop.Infrastructure.Data;

public class BarbeariaDbContext : IdentityDbContext<ApplicationUser>
{
    public BarbeariaDbContext(DbContextOptions<BarbeariaDbContext> options) : base(options)
    {
    }

    public DbSet<Servico> Servicos => Set<Servico>();
    public DbSet<Pacote> Pacotes => Set<Pacote>();
    public DbSet<PacoteItem> PacoteItens => Set<PacoteItem>();
    public DbSet<Agendamento> Agendamentos => Set<Agendamento>();
    public DbSet<AgendamentoItem> AgendamentoItens => Set<AgendamentoItem>();
    public DbSet<BloqueioAgenda> BloqueiosAgenda => Set<BloqueioAgenda>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<MovimentacaoEstoque> MovimentacoesEstoque => Set<MovimentacaoEstoque>();
    public DbSet<Venda> Vendas => Set<Venda>();
    public DbSet<VendaItem> VendaItens => Set<VendaItem>();
    public DbSet<Pagamento> Pagamentos => Set<Pagamento>();
    public DbSet<Avaliacao> Avaliacoes => Set<Avaliacao>();
    public DbSet<TemplateMensagem> TemplatesMensagem => Set<TemplateMensagem>();
    public DbSet<NotificacaoLog> NotificacoesLog => Set<NotificacaoLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Mapeamento das tabelas padrão do ASP.NET Identity para português
        modelBuilder.Entity<ApplicationUser>(b => b.ToTable("Usuarios"));
        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityRole>(b => b.ToTable("Perfis"));
        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<string>>(b => b.ToTable("UsuarioPerfis"));
        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserClaim<string>>(b => b.ToTable("UsuarioClaims"));
        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>>(b => b.ToTable("PerfilClaims"));
        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserLogin<string>>(b => b.ToTable("UsuarioLogins"));
        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserToken<string>>(b => b.ToTable("UsuarioTokens"));

        // Aplica todas as classes IEntityTypeConfiguration<T> da camada de Infrastructure
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BarbeariaDbContext).Assembly);
    }
}
