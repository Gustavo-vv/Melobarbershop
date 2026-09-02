using Barbearia.Domain.Enums;

namespace Melobarbershop.Domain.Entidades
{
    public class TemplateMensagem
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public TipoGatilhoMensagem Gatilho { get; set; }
        // Conteúdo com tags: "Olá {NomeCliente}, seu horário está confirmado para {DataHora} com {NomeBarbeiro}."
        public string ConteudoTemplate { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
    }
}
