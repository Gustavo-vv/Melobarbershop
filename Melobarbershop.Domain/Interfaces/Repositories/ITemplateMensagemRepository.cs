// Arquivo: Melobarbershop.Domain/Interfaces/Repositories/ITemplateMensagemRepository.cs
// Namespace: Melobarbershop.Domain.Interfaces.Repositories
// Conteúdo: interface ITemplateMensagemRepository
// Resumo: Contrato de repositório para gestão de templates de mensagem usados nas notificações.
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