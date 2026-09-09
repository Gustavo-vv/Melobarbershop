using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Domain.Interfaces.Repositories;

public interface ITemplateMensagemRepository
{
    Task<TemplateMensagem?> ObterPorIdAsync(int id);
    Task<TemplateMensagem?> ObterPorGatilhoAsync(TipoGatilhoMensagem gatilho);
    Task<IEnumerable<TemplateMensagem>> ObterTodosAsync();
    Task<IEnumerable<TemplateMensagem>> ObterAtivosAsync();
    Task AdicionarAsync(TemplateMensagem template);
    Task AtualizarAsync(TemplateMensagem template);
}