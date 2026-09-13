// Arquivo: Melobarbershop.Domain/Entidades/NotificacaoLog.cs
// Namespace: Melobarbershop.Domain.Entidades
// Conteúdo: class NotificacaoLog
// Resumo: Registro de tentativas de envio de notificações (mensagens) aos clientes.
namespace Melobarbershop.Domain.Entidades;

public class NotificacaoLog
{
    public int Id { get; set; }
    public string? ClienteId { get; set; }
    public ApplicationUser? Cliente { get; set; }

    public string NumeroDestino { get; set; } = string.Empty;
    public string MensagemEnviada { get; set; } = string.Empty;
    public DateTime DataEnvio { get; set; } = DateTime.UtcNow;
    public bool Sucesso { get; set; }
    public string? DetalhesRespostaApi { get; set; }
}
