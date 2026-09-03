using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Melobarbershop.Domain.Entidades;

namespace Melobarbershop.Infrastructure.Data.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("Usuarios");

        builder.Property(u => u.Nome)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(u => u.PreferenciasNotas)
            .HasMaxLength(500);

        builder.Property(u => u.FotoUrl)
            .HasMaxLength(500);

        builder.Property(u => u.PercentualComissao)
            .HasPrecision(5, 2);
    }
}

public class ServicoConfiguration : IEntityTypeConfiguration<Servico>
{
    public void Configure(EntityTypeBuilder<Servico> builder)
    {
        builder.ToTable("Servicos");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Nome)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.Descricao)
            .HasMaxLength(500);

        builder.Property(s => s.Preco)
            .HasPrecision(18, 2)
            .IsRequired();
    }
}

public class PacoteConfiguration : IEntityTypeConfiguration<Pacote>
{
    public void Configure(EntityTypeBuilder<Pacote> builder)
    {
        builder.ToTable("Pacotes");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nome)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.PrecoTotal)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasMany(p => p.Itens)
            .WithOne(i => i.Pacote)
            .HasForeignKey(i => i.PacoteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class PacoteItemConfiguration : IEntityTypeConfiguration<PacoteItem>
{
    public void Configure(EntityTypeBuilder<PacoteItem> builder)
    {
        builder.ToTable("PacoteItens");

        builder.HasKey(pi => pi.Id);

        builder.HasOne(pi => pi.Servico)
            .WithMany()
            .HasForeignKey(pi => pi.ServicoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.ToTable("Produtos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.CodigoBarras)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(p => p.CodigoBarras)
            .IsUnique();

        builder.Property(p => p.Nome)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(p => p.PrecoCusto)
            .HasPrecision(18, 2);

        builder.Property(p => p.PrecoVenda)
            .HasPrecision(18, 2);

        builder.HasMany(p => p.Movimentacoes)
            .WithOne(m => m.Produto)
            .HasForeignKey(m => m.ProdutoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class MovimentacaoEstoqueConfiguration : IEntityTypeConfiguration<MovimentacaoEstoque>
{
    public void Configure(EntityTypeBuilder<MovimentacaoEstoque> builder)
    {
        builder.ToTable("MovimentacoesEstoque");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Observacao)
            .HasMaxLength(250);
    }
}

public class AgendamentoConfiguration : IEntityTypeConfiguration<Agendamento>
{
    public void Configure(EntityTypeBuilder<Agendamento> builder)
    {
        builder.ToTable("Agendamentos");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Observacoes)
            .HasMaxLength(500);

        builder.HasOne(a => a.Cliente)
            .WithMany(u => u.AgendamentosComoCliente)
            .HasForeignKey(a => a.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Barbeiro)
            .WithMany(u => u.AgendamentosComoBarbeiro)
            .HasForeignKey(a => a.BarbeiroId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.Itens)
            .WithOne(i => i.Agendamento)
            .HasForeignKey(i => i.AgendamentoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Avaliacao)
            .WithOne(av => av.Agendamento)
            .HasForeignKey<Avaliacao>(av => av.AgendamentoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class AgendamentoItemConfiguration : IEntityTypeConfiguration<AgendamentoItem>
{
    public void Configure(EntityTypeBuilder<AgendamentoItem> builder)
    {
        builder.ToTable("AgendamentoItens");

        builder.HasKey(ai => ai.Id);

        builder.Property(ai => ai.PrecoCobrado)
            .HasPrecision(18, 2);

        builder.HasOne(ai => ai.Servico)
            .WithMany()
            .HasForeignKey(ai => ai.ServicoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class BloqueioAgendaConfiguration : IEntityTypeConfiguration<BloqueioAgenda>
{
    public void Configure(EntityTypeBuilder<BloqueioAgenda> builder)
    {
        builder.ToTable("BloqueiosAgenda");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Motivo)
            .HasMaxLength(250)
            .IsRequired();

        builder.HasOne(b => b.Barbeiro)
            .WithMany(u => u.BloqueiosAgenda)
            .HasForeignKey(b => b.BarbeiroId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class VendaConfiguration : IEntityTypeConfiguration<Venda>
{
    public void Configure(EntityTypeBuilder<Venda> builder)
    {
        builder.ToTable("Vendas");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.ValorSubtotal)
            .HasPrecision(18, 2);

        builder.Property(v => v.ValorDesconto)
            .HasPrecision(18, 2);

        builder.Property(v => v.ValorFinal)
            .HasPrecision(18, 2);

        builder.HasOne(v => v.Cliente)
            .WithMany(u => u.Vendas)
            .HasForeignKey(v => v.ClienteId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(v => v.Agendamento)
            .WithOne()
            .HasForeignKey<Venda>(v => v.AgendamentoId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(v => v.Itens)
            .WithOne(i => i.Venda)
            .HasForeignKey(i => i.VendaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(v => v.Pagamentos)
            .WithOne(p => p.Venda)
            .HasForeignKey(p => p.VendaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class VendaItemConfiguration : IEntityTypeConfiguration<VendaItem>
{
    public void Configure(EntityTypeBuilder<VendaItem> builder)
    {
        builder.ToTable("VendaItens");

        builder.HasKey(vi => vi.Id);

        builder.Property(vi => vi.PrecoUnitario)
            .HasPrecision(18, 2);

        builder.HasOne(vi => vi.Servico)
            .WithMany()
            .HasForeignKey(vi => vi.ServicoId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(vi => vi.Produto)
            .WithMany()
            .HasForeignKey(vi => vi.ProdutoId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(vi => vi.Barbeiro)
            .WithMany()
            .HasForeignKey(vi => vi.BarbeiroId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class PagamentoConfiguration : IEntityTypeConfiguration<Pagamento>
{
    public void Configure(EntityTypeBuilder<Pagamento> builder)
    {
        builder.ToTable("Pagamentos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Valor)
            .HasPrecision(18, 2);
    }
}

public class AvaliacaoConfiguration : IEntityTypeConfiguration<Avaliacao>
{
    public void Configure(EntityTypeBuilder<Avaliacao> builder)
    {
        builder.ToTable("Avaliacoes");

        builder.HasKey(av => av.Id);

        builder.Property(av => av.Comentario)
            .HasMaxLength(500);

        builder.HasOne(av => av.Cliente)
            .WithMany(u => u.AvaliacoesComoCliente)
            .HasForeignKey(av => av.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(av => av.Barbeiro)
            .WithMany(u => u.AvaliacoesComoBarbeiro)
            .HasForeignKey(av => av.BarbeiroId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class TemplateMensagemConfiguration : IEntityTypeConfiguration<TemplateMensagem>
{
    public void Configure(EntityTypeBuilder<TemplateMensagem> builder)
    {
        builder.ToTable("TemplatesMensagem");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Nome)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.ConteudoTemplate)
            .HasMaxLength(1000)
            .IsRequired();
    }
}

public class NotificacaoLogConfiguration : IEntityTypeConfiguration<NotificacaoLog>
{
    public void Configure(EntityTypeBuilder<NotificacaoLog> builder)
    {
        builder.ToTable("NotificacoesLog");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.NumeroDestino)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(n => n.MensagemEnviada)
            .HasMaxLength(1000)
            .IsRequired();

        builder.HasOne(n => n.Cliente)
            .WithMany()
            .HasForeignKey(n => n.ClienteId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
