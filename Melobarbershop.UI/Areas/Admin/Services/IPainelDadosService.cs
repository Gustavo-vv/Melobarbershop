using Melobarbershop.UI.Areas.Admin.Models;

namespace Melobarbershop.UI.Areas.Admin.Services;

public interface IPainelDadosService
{
    Task<PainelDadosPayloadViewModel> ObterDadosPainelAsync(string periodo, string profissionalId, string origem);
    Task<bool> AlterarStatusAgendamentoAsync(int id, string acao, string? motivo);
}
