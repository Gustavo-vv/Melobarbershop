namespace Melobarbershop.Domain.Entidades
{
    public class PacoteItem
    {
        public int Id { get; set; }
        public int PacoteId { get; set; }
        public Pacote Pacote { get; set; } = null!;
        public int ServicoId { get; set; }
        public Servico Servico { get; set; } = null!;
    }
}
