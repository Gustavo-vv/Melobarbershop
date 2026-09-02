using Microsoft.EntityFrameworkCore;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Enums;
using Melobarbershop.Domain.Interfaces.Repositories;
using Melobarbershop.Infrastructure.Data;

namespace Melobarbershop.Infrastructure.Repositories;

public class AgendamentoRepository : IAgendamentoRepository
{
    private readonly BarbeariaDbContext _context;

    public AgendamentoRepository(BarbeariaDbContext context)
    {
        _context = context;
    }

    public async Task<Agendamento?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Agendamentos
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<Agendamento?> ObterPorIdCompletoAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Agendamentos
            .Include(a => a.Cliente)
            .Include(a => a.Barbeiro)
            .Include(a => a.Itens)
                .ThenInclude(i => i.Servico)
            .Include(a => a.Avaliacao)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Agendamento>> ObterPorClienteAsync(string clienteId, CancellationToken cancellationToken = default)
    {
        return await _context.Agendamentos
            .Include(a => a.Barbeiro)
            .Include(a => a.Itens)
                .ThenInclude(i => i.Servico)
            .Where(a => a.ClienteId == clienteId)
            .OrderByDescending(a => a.DataHoraInicio)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Agendamento>> ObterPorBarbeiroEPeriodoAsync(string barbeiroId, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
    {
        return await _context.Agendamentos
            .Include(a => a.Cliente)
            .Include(a => a.Itens)
                .ThenInclude(i => i.Servico)
            .Where(a => a.BarbeiroId == barbeiroId && a.DataHoraInicio >= inicio && a.DataHoraInicio <= fim)
            .OrderBy(a => a.DataHoraInicio)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Agendamento>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
    {
        return await _context.Agendamentos
            .Include(a => a.Cliente)
            .Include(a => a.Barbeiro)
            .Include(a => a.Itens)
                .ThenInclude(i => i.Servico)
            .Where(a => a.DataHoraInicio >= inicio && a.DataHoraInicio <= fim)
            .OrderBy(a => a.DataHoraInicio)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Agendamento>> ObterProximosAgendamentosAsync(DateTime aPartirDe, string? barbeiroId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Agendamentos
            .Include(a => a.Cliente)
            .Include(a => a.Barbeiro)
            .Include(a => a.Itens)
                .ThenInclude(i => i.Servico)
            .Where(a => a.DataHoraInicio >= aPartirDe && a.Status != StatusAgendamento.Cancelado);

        if (!string.IsNullOrEmpty(barbeiroId))
            query = query.Where(a => a.BarbeiroId == barbeiroId);

        return await query
            .OrderBy(a => a.DataHoraInicio)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Agendamento>> ObterAgendamentosParaLembreteAsync(DateTime janelaInicio, DateTime janelaFim, CancellationToken cancellationToken = default)
    {
        return await _context.Agendamentos
            .Include(a => a.Cliente)
            .Include(a => a.Barbeiro)
            .Include(a => a.Itens)
                .ThenInclude(i => i.Servico)
            .Where(a => a.DataHoraInicio >= janelaInicio && a.DataHoraInicio <= janelaFim && a.Status == StatusAgendamento.Confirmado)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExisteConflitoDeHorarioAsync(string barbeiroId, DateTime inicio, DateTime fim, int? agendamentoIdIgnorar = null, CancellationToken cancellationToken = default)
    {
        return await _context.Agendamentos
            .AnyAsync(a => a.BarbeiroId == barbeiroId
                        && a.Status != StatusAgendamento.Cancelado
                        && a.Status != StatusAgendamento.NaoCompareceu
                        && (agendamentoIdIgnorar == null || a.Id != agendamentoIdIgnorar)
                        && a.DataHoraInicio < fim
                        && a.DataHoraFim > inicio,
                      cancellationToken);
    }

    public async Task AdicionarAsync(Agendamento agendamento, CancellationToken cancellationToken = default)
    {
        await _context.Agendamentos.AddAsync(agendamento, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AtualizarAsync(Agendamento agendamento, CancellationToken cancellationToken = default)
    {
        _context.Agendamentos.Update(agendamento);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AtualizarStatusAsync(int id, StatusAgendamento status, CancellationToken cancellationToken = default)
    {
        var agendamento = await _context.Agendamentos.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        if (agendamento != null)
        {
            agendamento.Status = status;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
