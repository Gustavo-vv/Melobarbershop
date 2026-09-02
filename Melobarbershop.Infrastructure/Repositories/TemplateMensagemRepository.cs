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

    public async Task<TemplateMensagem?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.TemplatesMensagem
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<TemplateMensagem?> ObterPorGatilhoAsync(TipoGatilhoMensagem gatilho, CancellationToken cancellationToken = default)
    {
        return await _context.TemplatesMensagem
            .FirstOrDefaultAsync(t => t.Gatilho == gatilho && t.Ativo, cancellationToken);
    }

    public async Task<IEnumerable<TemplateMensagem>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TemplatesMensagem
            .OrderBy(t => t.Nome)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TemplateMensagem>> ObterAtivosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TemplatesMensagem
            .Where(t => t.Ativo)
            .OrderBy(t => t.Nome)
            .ToListAsync(cancellationToken);
    }

    public async Task AdicionarAsync(TemplateMensagem template, CancellationToken cancellationToken = default)
    {
        await _context.TemplatesMensagem.AddAsync(template, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AtualizarAsync(TemplateMensagem template, CancellationToken cancellationToken = default)
    {
        _context.TemplatesMensagem.Update(template);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
