using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Melobarbershop.Domain.Entidades
{
    public class CategoriaServico
    {
        public int Id { get; set; }

        [MaxLength(100, ErrorMessage = "O nome da categoria deve ter no maximo 100 caracteres.")]
        public required string Nome { get; set; }

        [MaxLength(300, ErrorMessage = "A descrição deve ter no maximo 300 caracteres.")]
        public string? Descricao { get; set; }

        public bool Ativo { get; set; } = true;

        public ICollection<Servico> Servicos { get; set; } = new List<Servico>();
    }
}