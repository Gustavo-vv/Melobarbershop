// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Interfaces/Repositories/ITemplateMensagemRepository.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem define: Domain (Inversão de Dependência - DIP)
// - Quem implementa: Melobarbershop.Infrastructure (TemplateMensagemRepository via EF Core)
// - Quem consome: Melobarbershop.Application (NotificacaoService)
// ============================================================================

using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Domain.Interfaces.Repositories;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Contrato de persistência para os modelos textuais de mensagens e automações de CRM.
/// 
/// POR QUE EXISTE:
/// Fornece recuperação rápida do texto ativo configurado para um evento gatilho específico
/// (ex: buscar o template ativo para TipoGatilhoMensagem.ConfirmacaoAgendamento).
/// </summary>
public interface ITemplateMensagemRepository
{
    /// <summary>
    /// Busca um template por ID.
    /// </summary>
    Task<TemplateMensagem?> ObterPorIdAsync(int id);

    /// <summary>
    /// Localiza o modelo de mensagem ativo configurado para um determinado gatilho de negócio.
    /// </summary>
    Task<TemplateMensagem?> ObterPorGatilhoAsync(TipoGatilhoMensagem gatilho);

    /// <summary>
    /// Lista todos os templates cadastrados no sistema para o painel de configurações.
    /// </summary>
    Task<IEnumerable<TemplateMensagem>> ObterTodosAsync();

    /// <summary>
    /// Lista apenas templates com Ativo == true.
    /// </summary>
    Task<IEnumerable<TemplateMensagem>> ObterAtivosAsync();

    /// <summary>
    /// Cadastra um novo template de mensagem.
    /// </summary>
    Task AdicionarAsync(TemplateMensagem template);

    /// <summary>
    /// Atualiza o texto ou parâmetros de um modelo existente.
    /// </summary>
    Task AtualizarAsync(TemplateMensagem template);
}