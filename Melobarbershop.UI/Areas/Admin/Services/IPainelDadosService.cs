using Melobarbershop.UI.Areas.Admin.Models;

namespace Melobarbershop.UI.Areas.Admin.Services;

public interface IPainelDadosService
{
    Task<PainelDadosPayloadViewModel> ObterDadosPainelAsync(
        string periodo, 
        string profissionalId, 
        string origem, 
        DateTime? inicioPersonalizado = null, 
        DateTime? fimPersonalizado = null);

    Task<bool> AlterarStatusAgendamentoAsync(int id, string acao, string? motivo);
}
