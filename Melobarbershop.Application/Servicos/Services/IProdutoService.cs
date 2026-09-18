// ============================================================================
// Arquivo: IProdutoService.cs
// Camada: Melobarbershop.Application (Serviços - Contratos)
// Objetivo: Definir os contratos de negócio para gestão de produtos físicos,
//           controle e auditoria de movimentações de estoque.
// Papel na Arquitetura:
//   - Interface que expõe consultas de saldo, alertas de estoque baixo e entradas/saídas auditadas.
// ============================================================================

using Melobarbershop.Application.DTOs;

namespace Melobarbershop.Application.Servicos.Services;

/// <summary>
/// Contrato do serviço de catálogo de produtos e controle de estoque da barbearia.
/// </summary>
public interface IProdutoService
{
    /// <summary>Recupera um produto pelo seu ID único.</summary>
    Task<ApiResposta<ProdutoDto>> ObterPorIdAsync(int id);

    /// <summary>Busca produto pelo código de barras (útil para leitores óticos de código de barras no PDV).</summary>
    Task<ApiResposta<ProdutoDto>> ObterPorCodigoBarrasAsync(string codigoBarras);

    /// <summary>Lista somente os produtos ativos para venda no balcão.</summary>
    Task<ApiResposta<IEnumerable<ProdutoDto>>> ListarAtivosAsync();

    /// <summary>Lista todos os produtos cadastrados (ativos e inativos).</summary>
    Task<ApiResposta<IEnumerable<ProdutoDto>>> ListarTodosAsync();

    /// <summary>Lista produtos cujo estoque atual atingiu ou ultrapassou para baixo o estoque mínimo de segurança.</summary>
    Task<ApiResposta<IEnumerable<ProdutoDto>>> ListarComEstoqueAbaixoDoMinimoAsync();

    /// <summary>Cadastra um novo produto com saldo inicial de estoque e gera log de auditoria.</summary>
    Task<ApiResposta<ProdutoDto>> CriarAsync(CriarProdutoDto dto);

    /// <summary>Atualiza dados cadastrais, preços e parâmetros de alerta de um produto.</summary>
    Task<ApiResposta<ProdutoDto>> AtualizarAsync(int id, AtualizarProdutoDto dto);

    /// <summary>Executa uma movimentação de estoque (entrada, saída, perda ou inventário) atualizando o saldo.</summary>
    Task<ApiResposta<ProdutoDto>> MovimentarEstoqueAsync(MovimentarEstoqueDto dto);

    /// <summary>Recupera o extrato/histórico de movimentações de estoque de um produto em determinado período.</summary>
    Task<ApiResposta<IEnumerable<MovimentacaoEstoqueDto>>> ListarMovimentacoesPorProdutoAsync(int produtoId, DateTime? inicio = null, DateTime? fim = null);

    /// <summary>Verifica atomicamente se há saldo suficiente disponível para atender uma venda.</summary>
    Task<ApiResposta<bool>> PossuiEstoqueAsync(int produtoId, int quantidade);

    /// <summary>Desativa um produto do catálogo (soft delete funcional).</summary>
    Task<ApiResposta<ProdutoDto>> DesativarAsync(int id);

    /// <summary>Reativa um produto inativado.</summary>
    Task<ApiResposta<ProdutoDto>> AtivarAsync(int id);

    /// <summary>Exclui permanentemente o registro de um produto caso não possua movimentações vinculadas.</summary>
    Task<ApiResposta<ProdutoDto>> RemoverPermanentementeAsync(int id);
}

