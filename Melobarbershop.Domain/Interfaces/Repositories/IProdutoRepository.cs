// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Interfaces/Repositories/IProdutoRepository.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem define: Domain (Inversão de Dependência - DIP)
// - Quem implementa: Melobarbershop.Infrastructure (ProdutoRepository via EF Core)
// - Quem consome: Melobarbershop.Application (ProdutoService, EstoqueService, VendaService)
// ============================================================================

namespace Melobarbershop.Domain.Interfaces.Repositories;

using Melobarbershop.Domain.Entidades;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Contrato de persistência para o catálogo de mercadorias físicas e controle de movimentações de estoque.
/// 
/// POR QUE EXISTE:
/// Centraliza a leitura por código de barras, listagens de compras urgentes (estoque mínimo)
/// e a gravação de auditoria de inventário.
/// </summary>
public interface IProdutoRepository
{
    /// <summary>
    /// Busca um produto pelo seu ID primário.
    /// </summary>
    Task<Produto?> ObterPorIdAsync(int id);

    /// <summary>
    /// Busca um produto pelo código de barras exato (utilizado para leitura de scanner no balcão).
    /// </summary>
    Task<Produto?> ObterPorCodigoBarrasAsync(string codigoBarras);

    /// <summary>
    /// Lista todos os produtos cadastrados (ativos e inativos) para o painel de administração.
    /// </summary>
    Task<IEnumerable<Produto>> ObterTodosAsync();

    /// <summary>
    /// Lista apenas produtos ativos com status de comercialização liberado.
    /// </summary>
    Task<IEnumerable<Produto>> ObterAtivosAsync();

    /// <summary>
    /// Identifica produtos em situação crítica (EstoqueAtual menor ou igual a EstoqueMinimoAlerta).
    /// Utilizado para geração da lista de compras da barbearia.
    /// </summary>
    Task<IEnumerable<Produto>> ObterComEstoqueAbaixoDoMinimoAsync();

    /// <summary>
    /// Cadastra um novo produto no catálogo.
    /// </summary>
    Task AdicionarAsync(Produto produto);

    /// <summary>
    /// Atualiza preços, dados cadastrais e saldo de estoque de um produto existente.
    /// </summary>
    Task AtualizarAsync(Produto produto);

    /// <summary>
    /// Insere um novo registro de auditoria na tabela de movimentações de estoque.
    /// </summary>
    Task AdicionarMovimentacaoEstoqueAsync(MovimentacaoEstoque movimentacao);

    /// <summary>
    /// Consulta o extrato histórico de entradas e saídas de um produto específico, com filtro de período opcional.
    /// </summary>
    Task<IEnumerable<MovimentacaoEstoque>> ObterMovimentacoesPorProdutoAsync(int produtoId, DateTime? inicio = null, DateTime? fim = null);

    /// <summary>
    /// Remove um produto do banco de dados (quando aplicável).
    /// </summary>
    Task RemoverAsync(Produto produto);
}