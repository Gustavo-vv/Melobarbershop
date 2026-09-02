using Melobarbershop.Domain.Entidades;
using Microsoft.AspNetCore.Identity;

namespace Barbearia.Domain.Entities;

// --- CLIENTE E PROFISSIONAL ---
public class ApplicationUser : IdentityUser
{
    public string Nome { get; set; } = string.Empty;

    public DateTime? DataNascimento { get; set; }
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    public bool Ativo { get; set; } = true;
    
    
    
    // Navegação
    public Cliente? Cliente { get; set; }
    public Barbeiro? Barbeiro { get; set; }
    public ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
    public ICollection<Venda> Vendas { get; set; } = new List<Venda>();
};