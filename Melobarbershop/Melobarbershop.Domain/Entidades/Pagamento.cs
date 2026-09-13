using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Domain.Entidades;

public class Pagamento
{
    public int Id { get; set; }
    public int VendaId { get; set; }
    public Venda Venda { get; set; } = null!;

    public FormaPagamento Forma { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataHora { get; set; } = DateTime.UtcNow;
}
