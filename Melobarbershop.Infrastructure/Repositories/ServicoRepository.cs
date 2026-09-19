// ============================================================================
// Arquivo: ServicoRepository.cs
// Camada: Melobarbershop.Infrastructure (Repositories)
// Objetivo: Repositório para persistência e recuperação de serviços oferecidos pela barbearia.
// Papel na Arquitetura:
//   - Executa consultas otimizadas ao banco de dados usando Entity Framework Core.
//   - Filtra serviços por status de ativação e visibilidade no catálogo do site de agendamento.
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;
using Melobarbershop.Infrastructure.Data;

namespace Melobarbershop.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório para Serviços (CRUD e disponibilidade).
/// </summary>
public class ServicoRepository : IServicoRepository
{
    private readonly BarbeariaDbContext _context;

    /// <summary>
    /// Construtor com injeção de dependência do contexto EF Core.
    /// </summary>
    public ServicoRepository(BarbeariaDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Busca um serviço por seu identificador único.
    /// </summary>
    public async Task<Servico?> ObterPorIdAsync(int id)
    {
        return await _context.Servicos
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    /// <summary>
    /// Retorna múltiplos serviços a partir de uma coleção de identificadores.
    /// Utilizado para orçamentos ou múltiplos serviços em um agendamento.
    /// </summary>
    public async Task<IEnumerable<Servico>> ObterPorIdsAsync(IEnumerable<int> ids)
    {
        return await _context.Servicos
            .Where(s => ids.Contains(s.Id))
            .ToListAsync();
    }

    /// <summary>
    /// Retorna todos os serviços cadastrados, ordenados por nome.
    /// </summary>
    public async Task<IEnumerable<Servico>> ObterTodosAsync()
    {
        return await _context.Servicos
            .OrderBy(s => s.Nome)
            .ToListAsync();
    }

    /// <summary>
    /// Retorna apenas os serviços ativos para agendamentos internos e rotinas de atendimento.
    /// </summary>
    public async Task<IEnumerable<Servico>> ObterAtivosAsync()
    {
        return await _context.Servicos
            .Where(s => s.Ativo)
            .OrderBy(s => s.Nome)
            .ToListAsync();
    }

    /// <summary>
    /// Retorna apenas os serviços que estão ativos e marcados para exibição pública no site de agendamento.
    /// </summary>
    public async Task<IEnumerable<Servico>> ObterExibidosNoSiteAsync()
    {
        return await _context.Servicos
            .Where(s => s.Ativo && s.ExibirNoSite)
            .OrderBy(s => s.Nome)
            .ToListAsync();
    }

    /// <summary>
    /// Insere um novo serviço no banco de dados.
    /// </summary>
    public async Task AdicionarAsync(Servico servico)
    {
        await _context.Servicos.AddAsync(servico);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Atualiza as informações de um serviço existente (ex: nome, preço, duração).
    /// </summary>
    public async Task AtualizarAsync(Servico servico)
    {
        _context.Servicos.Update(servico);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Remove um serviço permanentemente do banco de dados.
    /// </summary>
    public async Task RemoverAsync(Servico servico)
    {
        _context.Servicos.Remove(servico);
        await _context.SaveChangesAsync();
    }
}