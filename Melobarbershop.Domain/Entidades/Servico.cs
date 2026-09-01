using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Melobarbershop.Domain.Entidades
{
    public class Servico
    {
        public int Id { get; set; }

        [MaxLength(150, ErrorMessage = "O nome do serviço deve ter no maximo 150 caracteres.")]
        public required string Nome { get; set; }

        public required string Descricao { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Preco { get; set; }

        public int DuracaoMinutos { get; set; } // Duração em minutos (ex: 30, 45, 60)

        public string? ImagemUrl { get; set; }

        public bool Ativo { get; set; } = true;
        public bool DestaqueHome { get; set; } = false;

        // Chave estrangeira para Categoria
        public int CategoriaServicoId { get; set; }
        public CategoriaServico CategoriaServico { get; set; } = null!;

        public ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
    }
}