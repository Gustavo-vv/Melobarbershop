namespace Melobarbershop.Domain.Interfaces.Repositories;

using Barbearia.Domain.Enums;
using Melobarbershop.Domain.Entidades;

public interface ITemplateMensagemRepository
{
    Task<TemplateMensagem?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TemplateMensagem?> ObterPorGatilhoAsync(TipoGatilhoMensagem gatilho, CancellationToken cancellationToken = default);
    Task<IEnumerable<TemplateMensagem>> ObterTodosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<TemplateMensagem>> ObterAtivosAsync(CancellationToken cancellationToken = default);
    Task AdicionarAsync(TemplateMensagem template, CancellationToken cancellationToken = default);
    Task AtualizarAsync(TemplateMensagem template, CancellationToken cancellationToken = default);
}