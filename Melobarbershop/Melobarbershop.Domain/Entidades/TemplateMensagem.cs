using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Domain.Entidades;

public class TemplateMensagem
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public TipoGatilhoMensagem Gatilho { get; set; }
    public string ConteudoTemplate { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;
}
