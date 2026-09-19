// ============================================================================
// Arquivo: TemplateMensagemRepository.cs
// Camada: Melobarbershop.Infrastructure (Repositories)
// Objetivo: Repositório para consulta e manutenção dos modelos de mensagens de notificação do sistema.
// Papel na Arquitetura:
//   - Recupera os templates ativos mapeados para gatilhos específicos (ex: Confirmação de Agendamento, Lembrete 2h antes).
//   - Permite personalização textual pelo painel administrativo.
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Enums;
using Melobarbershop.Domain.Interfaces.Repositories;
using Melobarbershop.Infrastructure.Data;

namespace Melobarbershop.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório para gerenciar templates de mensagem usados nas notificações automáticas.
/// </summary>
public class TemplateMensagemRepository : ITemplateMensagemRepository
{
    private readonly BarbeariaDbContext _context;

    /// <summary>
    /// Construtor com injeção de dependência do contexto EF Core.
    /// </summary>
    public TemplateMensagemRepository(BarbeariaDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Busca um template pelo seu identificador primário.
    /// </summary>
    public async Task<TemplateMensagem?> ObterPorIdAsync(int id)
    {
        return await _context.TemplatesMensagem
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <summary>
    /// Busca o template ativo associado a um tipo específico de gatilho de notificação.
    /// </summary>
    public async Task<TemplateMensagem?> ObterPorGatilhoAsync(TipoGatilhoMensagem gatilho)
    {
        return await _context.TemplatesMensagem
            .FirstOrDefaultAsync(t => t.Gatilho == gatilho && t.Ativo);
    }

    /// <summary>
    /// Lista todos os templates cadastrados no sistema, ordenados por nome.
    /// </summary>
    public async Task<IEnumerable<TemplateMensagem>> ObterTodosAsync()
    {
        return await _context.TemplatesMensagem
            .OrderBy(t => t.Nome)
            .ToListAsync();
    }

    /// <summary>
    /// Lista apenas os templates que estão ativos para disparos automáticos.
    /// </summary>
    public async Task<IEnumerable<TemplateMensagem>> ObterAtivosAsync()
    {
        return await _context.TemplatesMensagem
            .Where(t => t.Ativo)
            .OrderBy(t => t.Nome)
            .ToListAsync();
    }

    /// <summary>
    /// Adiciona um novo template de mensagem no banco de dados.
    /// </summary>
    public async Task AdicionarAsync(TemplateMensagem template)
    {
        await _context.TemplatesMensagem.AddAsync(template);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Atualiza o texto, canal ou variáveis de um template de mensagem existente.
    /// </summary>
    public async Task AtualizarAsync(TemplateMensagem template)
    {
        _context.TemplatesMensagem.Update(template);
        await _context.SaveChangesAsync();
    }
}
