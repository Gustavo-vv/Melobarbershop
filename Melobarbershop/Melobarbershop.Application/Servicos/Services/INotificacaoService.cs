namespace Melobarbershop.Application.Servicos.Services;

public interface INotificacaoService
{
    Task<bool> EnviarAsync(string numeroDestino, string mensagem, string? clienteId = null);
    Task EnviarConfirmacaoAgendamentoAsync(int agendamentoId);
    Task EnviarLembreteAgendamentoAsync(int agendamentoId);
    Task EnviarCancelamentoAgendamentoAsync(int agendamentoId, string? motivo = null);
    Task EnviarSolicitacaoAvaliacaoAsync(int agendamentoId);
    Task<int> EnviarCampanhaMarketingAsync(IEnumerable<string> clienteIds, string mensagem);
}
