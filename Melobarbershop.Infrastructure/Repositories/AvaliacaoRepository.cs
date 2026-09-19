// ============================================================================
// Arquivo: AvaliacaoRepository.cs
// Camada: Melobarbershop.Infrastructure (Repositories)
// Objetivo: Repositório para persistência de avaliações e cálculo de reputação de barbeiros.
// Papel na Arquitetura:
//   - Recupera avaliações com dados de cliente e barbeiro carregados para visualização.
//   - Realiza cálculo agregado da média de notas (estrelas) por profissional.
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;
using Melobarbershop.Infrastructure.Data;

namespace Melobarbershop.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório para Avaliacao, encapsulando operações de persistência e estatísticas.
/// </summary>
public class AvaliacaoRepository : IAvaliacaoRepository
{
    private readonly BarbeariaDbContext _context;

    /// <summary>
    /// Construtor com injeção de dependência do contexto EF Core.
    /// </summary>
    public AvaliacaoRepository(BarbeariaDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Busca uma avaliação pelo seu identificador primário.
    /// </summary>
    public async Task<Avaliacao?> ObterPorIdAsync(int id)
    {
        return await _context.Avaliacoes
            .FirstOrDefaultAsync(av => av.Id == id);
    }

    /// <summary>
    /// Recupera a avaliação vinculada a um agendamento específico, incluindo dados de cliente e barbeiro.
    /// </summary>
    public async Task<Avaliacao?> ObterPorAgendamentoIdAsync(int agendamentoId)
    {
        return await _context.Avaliacoes
            .Include(av => av.Cliente)
            .Include(av => av.Barbeiro)
            .FirstOrDefaultAsync(av => av.AgendamentoId == agendamentoId);
    }

    /// <summary>
    /// Retorna a lista de todas as avaliações recebidas por um barbeiro, da mais recente para a mais antiga.
    /// </summary>
    public async Task<IEnumerable<Avaliacao>> ObterPorBarbeiroAsync(string barbeiroId)
    {
        return await _context.Avaliacoes
            .Include(av => av.Cliente)
            .Where(av => av.BarbeiroId == barbeiroId)
            .OrderByDescending(av => av.DataCriacao)
            .ToListAsync();
    }

    /// <summary>
    /// Retorna a lista de todas as avaliações feitas por um cliente específico, com dados do barbeiro.
    /// </summary>
    public async Task<IEnumerable<Avaliacao>> ObterPorClienteAsync(string clienteId)
    {
        return await _context.Avaliacoes
            .Include(av => av.Barbeiro)
            .Where(av => av.ClienteId == clienteId)
            .OrderByDescending(av => av.DataCriacao)
            .ToListAsync();
    }

    /// <summary>
    /// Calcula a média aritmética de pontuação (1 a 5 estrelas) de um barbeiro.
    /// </summary>
    public async Task<double> CalcularMediaAvaliacoesBarbeiroAsync(string barbeiroId)
    {
        var avaliacoes = await _context.Avaliacoes
            .Where(av => av.BarbeiroId == barbeiroId)
            .Select(av => av.NotaEstrelas)
            .ToListAsync();

        if (!avaliacoes.Any())
            return 0.0;

        return avaliacoes.Average();
    }

    /// <summary>
    /// Verifica se já existe uma avaliação registrada para determinado agendamento.
    /// </summary>
    public async Task<bool> ExisteAvaliacaoParaAgendamentoAsync(int agendamentoId)
    {
        return await _context.Avaliacoes
            .AnyAsync(av => av.AgendamentoId == agendamentoId);
    }

    /// <summary>
    /// Insere uma nova avaliação no banco de dados.
    /// </summary>
    public async Task AdicionarAsync(Avaliacao avaliacao)
    {
        await _context.Avaliacoes.AddAsync(avaliacao);
        await _context.SaveChangesAsync();
    }
}
