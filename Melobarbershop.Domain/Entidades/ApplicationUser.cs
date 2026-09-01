using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Melobarbershop.Domain.Entidades
{
    public class ApplicationUser : IdentityUser
    {
        public required string NomeCompleto { get; set; }
        public string? FotoPerfilUrl { get; set; }
        public string? TelefoneWhatsApp { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
        public string? Especialidade { get; set; } // Especialidade do profissional (caso seja Barbeiro)

        // Relacionamentos com Agendamento
        public ICollection<Agendamento> AgendamentosComoCliente { get; set; } = new List<Agendamento>();
        public ICollection<Agendamento> AtendimentosComoBarbeiro { get; set; } = new List<Agendamento>();
    }
}