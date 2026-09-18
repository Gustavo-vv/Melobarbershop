// ============================================================================
// Arquivo: AgendamentoRepository.cs
// Camada: Melobarbershop.Infrastructure (Repositories)
// Objetivo: Implementação do repositório de dados para agendamentos via Entity Framework Core.
// Papel na Arquitetura:
//   - Isola as consultas LINQ e operações de persistência da entidade Agendamento.
//   - Executa buscas com Includes otimizados para clientes, barbeiros, serviços e avaliações.
//   - Realiza verificação eficiente de sobreposição e conflito de horários no banco.
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Enums;
using Melobarbershop.Domain.Interfaces.Repositories;
using Melobarbershop.Infrastructure.Data;

namespace Melobarbershop.Infrastructure.Repositories;

/// <summary>
/// Repositório para persistência e consultas de agendamentos no banco de dados.
/// </summary>
public class AgendamentoRepository : IAgendamentoRepository
{
    private readonly BarbeariaDbContext _context;

    /// <summary>
    /// Construtor que recebe o contexto do banco de dados por injeção de dependência.
    /// </summary>
    public AgendamentoRepository(BarbeariaDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtém um agendamento apenas pelos dados da tabela principal.
    /// </summary>
    public async Task<Agendamento?> ObterPorIdAsync(int id)
    {
        return await _context.Agendamentos
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    /// <summary>
    /// Obtém um agendamento carregando todos os seus relacionamentos (Cliente, Barbeiro, Itens/Serviços e Avaliação).
    /// </summary>
    public async Task<Agendamento?> ObterPorIdCompletoAsync(int id)
    {
        return await _context.Agendamentos
            .Include(a => a.Cliente)
            .Include(a => a.Barbeiro)
            .Include(a => a.Itens)
                .ThenInclude(i => i.Servico)
            .Include(a => a.Avaliacao)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    /// <summary>
    /// Retorna o histórico de agendamentos de um cliente específico ordenado do mais recente para o mais antigo.
    /// </summary>
    public async Task<IEnumerable<Agendamento>> ObterPorClienteAsync(string clienteId)
    {
        return await _context.Agendamentos
            .Include(a => a.Barbeiro)
            .Include(a => a.Itens)
                .ThenInclude(i => i.Servico)
            .Where(a => a.ClienteId == clienteId)
            .OrderByDescending(a => a.DataHoraInicio)
            .ToListAsync();
    }

    /// <summary>
    /// Retorna todos os agendamentos de um barbeiro específico dentro de uma faixa de datas.
    /// </summary>
    public async Task<IEnumerable<Agendamento>> ObterPorBarbeiroEPeriodoAsync(string barbeiroId, DateTime inicio, DateTime fim)
    {
        return await _context.Agendamentos
            .Include(a => a.Cliente)
            .Include(a => a.Itens)
                .ThenInclude(i => i.Servico)
            .Where(a => a.BarbeiroId == barbeiroId && a.DataHoraInicio >= inicio && a.DataHoraInicio <= fim)
            .OrderBy(a => a.DataHoraInicio)
            .ToListAsync();
    }

    /// <summary>
    /// Retorna os agendamentos de toda a barbearia dentro de um intervalo de datas.
    /// </summary>
    public async Task<IEnumerable<Agendamento>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim)
    {
        return await _context.Agendamentos
            .Include(a => a.Cliente)
            .Include(a => a.Barbeiro)
            .Include(a => a.Itens)
                .ThenInclude(i => i.Servico)
            .Where(a => a.DataHoraInicio >= inicio && a.DataHoraInicio <= fim)
            .OrderBy(a => a.DataHoraInicio)
            .ToListAsync();
    }

    /// <summary>
    /// Busca os próximos agendamentos não cancelados a partir de um determinado horário.
    /// </summary>
    public async Task<IEnumerable<Agendamento>> ObterProximosAgendamentosAsync(DateTime aPartirDe, string? barbeiroId = null)
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
            .ToListAsync();
    }

    /// <summary>
    /// Localiza agendamentos confirmados que se encontram na janela de envio de lembrete.
    /// </summary>
    public async Task<IEnumerable<Agendamento>> ObterAgendamentosParaLembreteAsync(DateTime janelaInicio, DateTime janelaFim)
    {
        return await _context.Agendamentos
            .Include(a => a.Cliente)
            .Include(a => a.Barbeiro)
            .Include(a => a.Itens)
                .ThenInclude(i => i.Servico)
            .Where(a => a.DataHoraInicio >= janelaInicio && a.DataHoraInicio <= janelaFim && a.Status == StatusAgendamento.Confirmado)
            .ToListAsync();
    }

    /// <summary>
    /// Verifica se existe colisão de horário no banco para um determinado barbeiro, desconsiderando agendamentos cancelados ou no-show.
    /// </summary>
    public async Task<bool> ExisteConflitoDeHorarioAsync(string barbeiroId, DateTime inicio, DateTime fim, int? agendamentoIdIgnorar = null)
    {
        return await _context.Agendamentos
            .AnyAsync(a => a.BarbeiroId == barbeiroId
                        && a.Status != StatusAgendamento.Cancelado
                        && a.Status != StatusAgendamento.NaoCompareceu
                        && (agendamentoIdIgnorar == null || a.Id != agendamentoIdIgnorar)
                        && a.DataHoraInicio < fim
                        && a.DataHoraFim > inicio);
    }

    /// <summary>
    /// Adiciona um novo agendamento e persiste no banco de dados.
    /// </summary>
    public async Task AdicionarAsync(Agendamento agendamento)
    {
        await _context.Agendamentos.AddAsync(agendamento);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Atualiza as informações do agendamento e salva as alterações.
    /// </summary>
    public async Task AtualizarAsync(Agendamento agendamento)
    {
        _context.Agendamentos.Update(agendamento);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Atualiza de forma direcionada o status de um agendamento.
    /// </summary>
    public async Task AtualizarStatusAsync(int id, StatusAgendamento status)
    {
        var agendamento = await _context.Agendamentos.FirstOrDefaultAsync(a => a.Id == id);
        if (agendamento != null)
        {
            agendamento.Status = status;
            await _context.SaveChangesAsync();
        }
    }
}
