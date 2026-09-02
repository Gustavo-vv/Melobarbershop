using Microsoft.AspNetCore.Identity;

namespace Melobarbershop.Domain.Entidades;

public class ApplicationUser : IdentityUser
{
    public string Nome { get; set; } = string.Empty;
    public DateTime? DataNascimento { get; set; }
    public string? PreferenciasNotas { get; set; } // Preferências do cliente ou notas
    public string? FotoUrl { get; set; }           // Foto de perfil do barbeiro/cliente
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    public bool Ativo { get; set; } = true;

    // Navegações
    public ICollection<Agendamento> AgendamentosComoCliente { get; set; } = new List<Agendamento>();
    public ICollection<Agendamento> AgendamentosComoBarbeiro { get; set; } = new List<Agendamento>();
    public ICollection<BloqueioAgenda> BloqueiosAgenda { get; set; } = new List<BloqueioAgenda>();
    public ICollection<Venda> Vendas { get; set; } = new List<Venda>();
    public ICollection<Avaliacao> AvaliacoesComoCliente { get; set; } = new List<Avaliacao>();
    public ICollection<Avaliacao> AvaliacoesComoBarbeiro { get; set; } = new List<Avaliacao>();
}