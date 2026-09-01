using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Domain.Entidades
{
    public class Agendamento
    {
        public int Id { get; set; }

        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal ValorCobrado { get; set; }

        public StatusAgendamento Status { get; set; } = StatusAgendamento.Pendente;

        [MaxLength(500)]
        public string? Observacoes { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        // Cliente
        public required string ClienteId { get; set; }
        public ApplicationUser Cliente { get; set; } = null!;

        // Barbeiro
        public required string BarbeiroId { get; set; }
        public ApplicationUser Barbeiro { get; set; } = null!;

        // Serviço
        public required int ServicoId { get; set; }
        public Servico Servico { get; set; } = null!;
    }
}