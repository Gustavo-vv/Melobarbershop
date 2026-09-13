// Arquivo: Melobarbershop.Domain/Entidades/PacoteItem.cs
// Namespace: Melobarbershop.Domain.Entidades
// Conteúdo: class PacoteItem
// Resumo: Item que compõe um Pacote, referenciando um serviço incluído no pacote.
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
