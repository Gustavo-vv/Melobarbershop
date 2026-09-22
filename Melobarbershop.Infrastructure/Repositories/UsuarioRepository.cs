// ============================================================================
// Arquivo: UsuarioRepository.cs
// Camada: Melobarbershop.Infrastructure (Repositories)
// Objetivo: Repositório para consultas de usuários, roles e gestão de bloqueios de agenda.
// Papel na Arquitetura:
//   - Executa buscas por telefone, e-mail e perfil (role) associado ao Identity.
//   - Fornece métodos de validação de existência para evitar duplicidades de dados cadastrais.
//   - Gerencia operações CRUD na tabela de bloqueios de agenda (BloqueioAgenda).
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Interfaces.Repositories;
using Melobarbershop.Infrastructure.Data;

namespace Melobarbershop.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório para usuários e bloqueios de agenda.
/// </summary>
public class UsuarioRepository : IUsuarioRepository
{
    private readonly BarbeariaDbContext _context;

    /// <summary>
    /// Construtor com injeção do DbContext.
    /// </summary>
    public UsuarioRepository(BarbeariaDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Busca um usuário pelo seu ID único.
    /// </summary>
    public async Task<ApplicationUser?> ObterPorIdAsync(string id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    /// <summary>
    /// Busca um usuário pelo seu número de telefone.
    /// </summary>
    public async Task<ApplicationUser?> ObterPorTelefoneAsync(string telefone)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == telefone);
    }

    /// <summary>
    /// Busca um usuário pelo endereço de e-mail.
    /// </summary>
    public async Task<ApplicationUser?> ObterPorEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    /// <summary>
    /// Lista todos os usuários que possuem uma determinada role (perfil de acesso).
    /// </summary>
    public async Task<IEnumerable<ApplicationUser>> ObterPorRoleAsync(string roleName)
    {
        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.NormalizedName == roleName.ToUpper());

        if (role == null)
            return Enumerable.Empty<ApplicationUser>();

        var userIds = await _context.UserRoles
            .Where(ur => ur.RoleId == role.Id)
            .Select(ur => ur.UserId)
            .ToListAsync();

        return await _context.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync();
    }

    /// <summary>
    /// Lista apenas os usuários ativos que possuem uma determinada role (ex: Barbeiros em atividade).
    /// </summary>
    public async Task<IEnumerable<ApplicationUser>> ObterAtivosPorRoleAsync(string roleName)
    {
        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.NormalizedName == roleName.ToUpper());

        if (role == null)
            return Enumerable.Empty<ApplicationUser>();

        var userIds = await _context.UserRoles
            .Where(ur => ur.RoleId == role.Id)
            .Select(ur => ur.UserId)
            .ToListAsync();

        return await _context.Users
            .Where(u => userIds.Contains(u.Id) && u.Ativo)
            .ToListAsync();
    }

    /// <summary>
    /// Verifica se já existe um usuário com o telefone informado, com suporte a ignorar o próprio ID em atualizações.
    /// </summary>
    public async Task<bool> ExisteTelefoneAsync(string telefone, string? usuarioIdIgnorar = null)
    {
        return await _context.Users
            .AnyAsync(u => u.PhoneNumber == telefone && (usuarioIdIgnorar == null || u.Id != usuarioIdIgnorar));
    }

    /// <summary>
    /// Verifica se já existe um usuário com o e-mail informado, com suporte a ignorar o próprio ID em atualizações.
    /// </summary>
    public async Task<bool> ExisteEmailAsync(string email, string? usuarioIdIgnorar = null)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email && (usuarioIdIgnorar == null || u.Id != usuarioIdIgnorar));
    }

    /// <summary>
    /// Retorna os bloqueios de agenda registrados para um barbeiro que colidam ou ocorram dentro do intervalo informado.
    /// </summary>
    public async Task<IEnumerable<BloqueioAgenda>> ObterBloqueiosPorPeriodoAsync(string barbeiroId, DateTime inicio, DateTime fim)
    {
        return await _context.BloqueiosAgenda
            .Include(b => b.Barbeiro)
            .Where(b => b.BarbeiroId == barbeiroId && b.DataHoraInicio < fim && b.DataHoraFim > inicio)
            .OrderBy(b => b.DataHoraInicio)
            .ToListAsync();
    }

    /// <summary>
    /// Retorna todos os bloqueios de agenda registrados de todos os barbeiros dentro do intervalo informado.
    /// </summary>
    public async Task<IEnumerable<BloqueioAgenda>> ObterBloqueiosGeraisPorPeriodoAsync(DateTime inicio, DateTime fim)
    {
        return await _context.BloqueiosAgenda
            .Include(b => b.Barbeiro)
            .Where(b => b.DataHoraInicio < fim && b.DataHoraFim > inicio)
            .OrderBy(b => b.DataHoraInicio)
            .ToListAsync();
    }

    /// <summary>
    /// Checa de forma rápida (AnyAsync) se há algum bloqueio ativo para o barbeiro no horário especificado.
    /// </summary>
    public async Task<bool> ExisteBloqueioNoPeriodoAsync(string barbeiroId, DateTime inicio, DateTime fim)
    {
        return await _context.BloqueiosAgenda
            .AnyAsync(b => b.BarbeiroId == barbeiroId && b.DataHoraInicio < fim && b.DataHoraFim > inicio);
    }

    /// <summary>
    /// Obtém um registro de bloqueio de agenda por seu ID.
    /// </summary>
    public async Task<BloqueioAgenda?> ObterBloqueioPorIdAsync(int bloqueioId)
    {
        return await _context.BloqueiosAgenda
            .FirstOrDefaultAsync(b => b.Id == bloqueioId);
    }

    /// <summary>
    /// Cadastra um novo bloqueio na agenda.
    /// </summary>
    public async Task AdicionarBloqueioAsync(BloqueioAgenda bloqueio)
    {
        await _context.BloqueiosAgenda.AddAsync(bloqueio);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Remove um bloqueio existente liberando a agenda.
    /// </summary>
    public async Task RemoverBloqueioAsync(BloqueioAgenda bloqueio)
    {
        _context.BloqueiosAgenda.Remove(bloqueio);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Atualiza os dados cadastrais do usuário no banco.
    /// </summary>
    public async Task AtualizarAsync(ApplicationUser usuario)
    {
        _context.Users.Update(usuario);
        await _context.SaveChangesAsync();
    }
}
