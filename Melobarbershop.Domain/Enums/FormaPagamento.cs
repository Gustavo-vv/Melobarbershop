namespace Melobarbershop.Domain.Enums;

/// <summary>
/// Enumeração que define as formas de pagamento suportadas.
/// </summary>
public enum FormaPagamento
{
    Dinheiro = 1,
    Pix = 2,
    CartaoCredito = 3,
    CartaoDebito = 4,
    Outro = 5
}