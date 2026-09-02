using Barbearia.Domain.Entities;

namespace Melobarbershop.Domain.Entidades
{
    public class Cliente
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; } = string.Empty;
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
        public ICollection<Venda> Vendas { get; set; } = new List<Venda>();
    }
}
