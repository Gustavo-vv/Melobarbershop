namespace Melobarbershop.Domain.Entidades
{
    public class NotificacaoLog
    {
        public int Id { get; set; }
        public int? ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        public string NumeroDestino { get; set; } = string.Empty;
        public string MensagemEnviada { get; set; } = string.Empty;
        public DateTime DataEnvio { get; set; } = DateTime.UtcNow;
        public bool Sucesso { get; set; }
        public string? DetalhesRespostaApi { get; set; }
    }
}
