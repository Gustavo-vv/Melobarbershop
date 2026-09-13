// Arquivo: Melobarbershop.Infrastructure/Repositories/TemplateMensagemRepository.cs
// Namespace: Melobarbershop.Infrastructure.Repositories
// Conteúdo: class TemplateMensagemRepository : ITemplateMensagemRepository
// Resumo: Implementação do repositório para gerenciar templates de mensagem usados nas notificações.
using Microsoft.EntityFrameworkCore;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Enums;
using Melobarbershop.Domain.Interfaces.Repositories;
using Melobarbershop.Infrastructure.Data;

namespace Melobarbershop.Infrastructure.Repositories;

public class TemplateMensagemRepository : ITemplateMensagemRepository
{
    private readonly BarbeariaDbContext _context;

    public TemplateMensagemRepository(BarbeariaDbContext context)
    {
        _context = context;
    }

    public async Task<TemplateMensagem?> ObterPorIdAsync(int id)
    {
        return await _context.TemplatesMensagem
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<TemplateMensagem?> ObterPorGatilhoAsync(TipoGatilhoMensagem gatilho)
    {
        return await _context.TemplatesMensagem
            .FirstOrDefaultAsync(t => t.Gatilho == gatilho && t.Ativo);
    }

    public async Task<IEnumerable<TemplateMensagem>> ObterTodosAsync()
    {
        return await _context.TemplatesMensagem
            .OrderBy(t => t.Nome)
            .ToListAsync();
    }

    public async Task<IEnumerable<TemplateMensagem>> ObterAtivosAsync()
    {
        return await _context.TemplatesMensagem
            .Where(t => t.Ativo)
            .OrderBy(t => t.Nome)
            .ToListAsync();
    }

    public async Task AdicionarAsync(TemplateMensagem template)
    {
        await _context.TemplatesMensagem.AddAsync(template);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(TemplateMensagem template)
    {
        _context.TemplatesMensagem.Update(template);
        await _context.SaveChangesAsync();
    }
}
