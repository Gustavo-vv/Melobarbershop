namespace Melobarbershop.Application.Servicos.Services;

public interface INotificacaoService
{
    Task<bool> EnviarAsync(string numeroDestino, string mensagem, string? clienteId = null, CancellationToken cancellationToken = default);
    Task EnviarConfirmacaoAgendamentoAsync(int agendamentoId, CancellationToken cancellationToken = default);
    Task EnviarLembreteAgendamentoAsync(int agendamentoId, CancellationToken cancellationToken = default);
    Task EnviarCancelamentoAgendamentoAsync(int agendamentoId, string? motivo = null, CancellationToken cancellationToken = default);
    Task EnviarSolicitacaoAvaliacaoAsync(int agendamentoId, CancellationToken cancellationToken = default);
    Task<int> EnviarCampanhaMarketingAsync(IEnumerable<string> clienteIds, string mensagem, CancellationToken cancellationToken = default);
}
