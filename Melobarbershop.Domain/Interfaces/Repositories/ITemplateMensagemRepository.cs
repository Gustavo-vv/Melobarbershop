using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Domain.Interfaces.Repositories;

public interface ITemplateMensagemRepository
{
    Task<TemplateMensagem?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TemplateMensagem?> ObterPorGatilhoAsync(TipoGatilhoMensagem gatilho, CancellationToken cancellationToken = default);
    Task<IEnumerable<TemplateMensagem>> ObterTodosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<TemplateMensagem>> ObterAtivosAsync(CancellationToken cancellationToken = default);
    Task AdicionarAsync(TemplateMensagem template, CancellationToken cancellationToken = default);
    Task AtualizarAsync(TemplateMensagem template, CancellationToken cancellationToken = default);
}