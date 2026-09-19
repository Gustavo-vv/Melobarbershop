// ============================================================================
// Arquivo: PacoteRepository.cs
// Camada: Melobarbershop.Infrastructure (Repositories)
// Objetivo: Repositório para persistência e recuperação de pacotes de serviços e seus itens associados.
// Papel na Arquitetura:
//   - Executa consultas carregando os serviços associados aos itens via Include/ThenInclude.
//   - Suporta consultas para vendas e contratações de combos promocionais.
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;
using Melobarbershop.Infrastructure.Data;

namespace Melobarbershop.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório para Pacotes e composição de itens de pacotes.
/// </summary>
public class PacoteRepository : IPacoteRepository
{
    private readonly BarbeariaDbContext _context;

    /// <summary>
    /// Construtor com injeção de dependência do contexto EF Core.
    /// </summary>
    public PacoteRepository(BarbeariaDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Busca um pacote apenas por seu identificador, sem carregar as dependências de itens.
    /// </summary>
    public async Task<Pacote?> ObterPorIdAsync(int id)
    {
        return await _context.Pacotes
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// Busca um pacote pelo ID trazendo todos os itens e os serviços associados carregados na consulta.
    /// </summary>
    public async Task<Pacote?> ObterPorIdComItensAsync(int id)
    {
        return await _context.Pacotes
            .Include(p => p.Itens)
                .ThenInclude(i => i.Servico)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// Lista todos os pacotes cadastrados com seus itens e serviços correspondentes.
    /// </summary>
    public async Task<IEnumerable<Pacote>> ObterTodosAsync()
    {
        return await _context.Pacotes
            .Include(p => p.Itens)
                .ThenInclude(i => i.Servico)
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }

    /// <summary>
    /// Lista apenas os pacotes ativos no momento, com itens e serviços inclusos.
    /// </summary>
    public async Task<IEnumerable<Pacote>> ObterAtivosAsync()
    {
        return await _context.Pacotes
            .Include(p => p.Itens)
                .ThenInclude(i => i.Servico)
            .Where(p => p.Ativo)
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }

    /// <summary>
    /// Persiste um novo pacote no banco de dados.
    /// </summary>
    public async Task AdicionarAsync(Pacote pacote)
    {
        await _context.Pacotes.AddAsync(pacote);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Atualiza as propriedades de um pacote existente no banco de dados.
    /// </summary>
    public async Task AtualizarAsync(Pacote pacote)
    {
        _context.Pacotes.Update(pacote);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Remove permanentemente um pacote do banco de dados.
    /// </summary>
    public async Task RemoverAsync(Pacote pacote)
    {
        _context.Pacotes.Remove(pacote);
        await _context.SaveChangesAsync();
    }
}
