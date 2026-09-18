// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Interfaces/Repositories/IPacoteRepository.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem define: Domain (Inversão de Dependência - DIP)
// - Quem implementa: Melobarbershop.Infrastructure (PacoteRepository via EF Core)
// - Quem consome: Melobarbershop.Application (PacoteService)
// ============================================================================

namespace Melobarbershop.Domain.Interfaces.Repositories;

using Melobarbershop.Domain.Entidades;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Contrato de persistência para combos e pacotes de serviços.
/// 
/// POR QUE EXISTE:
/// Padroniza as operações de busca com inclusão de itens (Eager Loading)
/// e listagem de ofertas ativas para a vitrine pública e para o sistema gerencial.
/// </summary>
public interface IPacoteRepository
{
    /// <summary>
    /// Busca dados básicos de um pacote pelo ID.
    /// </summary>
    Task<Pacote?> ObterPorIdAsync(int id);

    /// <summary>
    /// Busca o pacote carregando a coleção de itens filhos e as entidades de serviço vinculadas.
    /// </summary>
    Task<Pacote?> ObterPorIdComItensAsync(int id);

    /// <summary>
    /// Lista todos os pacotes cadastrados na base (ativos e inativos) para o painel administrativo.
    /// </summary>
    Task<IEnumerable<Pacote>> ObterTodosAsync();

    /// <summary>
    /// Lista apenas os pacotes marcados com Ativo == true para exibição aos clientes.
    /// </summary>
    Task<IEnumerable<Pacote>> ObterAtivosAsync();

    /// <summary>
    /// Insere um novo pacote promocional na base.
    /// </summary>
    Task AdicionarAsync(Pacote pacote);

    /// <summary>
    /// Atualiza nome, preço ou status de um pacote existente.
    /// </summary>
    Task AtualizarAsync(Pacote pacote);

    /// <summary>
    /// Remove o registro de um pacote do banco de dados.
    /// </summary>
    Task RemoverAsync(Pacote pacote);
}