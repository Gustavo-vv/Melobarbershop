// ============================================================================
// Arquivo: ProdutosController.cs
// Camada: Melobarbershop.API (Controllers)
// Objetivo: Expor endpoints REST para gestão do catálogo de produtos, controle
//           e movimentação de estoque da barbearia.
// Papel na Arquitetura:
//   - Fornece rotas para consultas de catálogo, alertas de estoque baixo e histórico de movimentações.
//   - Encaminha comandos de alteração e movimentação de estoque para IProdutoService.
// ============================================================================

using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Services;
using Microsoft.AspNetCore.Mvc;

namespace Melobarbershop.API.Controllers;

/// <summary>
/// Controlador responsável pelo catálogo de produtos e controle de estoque.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoService _produtoService;

    /// <summary>
    /// Construtor com injeção do serviço de produtos.
    /// </summary>
    public ProdutosController(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    /// <summary>
    /// Retorna todos os produtos cadastrados (ativos e inativos).
    /// </summary>
    [HttpGet("todos")]
    public async Task<IActionResult> ObterTodos()
    {
        var response = await _produtoService.ListarTodosAsync();
        return Ok(response);
    }

    /// <summary>
    /// Retorna somente os produtos ativos disponíveis para comercialização.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ObterAtivos()
    {
        var response = await _produtoService.ListarAtivosAsync();
        return Ok(response);
    }

    /// <summary>
    /// Retorna a lista de produtos com estoque em nível de alerta ou crítico.
    /// </summary>
    [HttpGet("estoque-baixo")]
    public async Task<IActionResult> ObterComEstoqueAbaixoDoMinimo()
    {
        var response = await _produtoService.ListarComEstoqueAbaixoDoMinimoAsync();
        return Ok(response);
    }

    /// <summary>
    /// Obtém os dados de um produto pelo seu ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var response = await _produtoService.ObterPorIdAsync(id);
        if (!response.Sucesso) return NotFound(response);
        return Ok(response);
    }

    /// <summary>
    /// Localiza um produto pelo código de barras.
    /// </summary>
    [HttpGet("codigoBarra/{codigoBarras}")]
    public async Task<IActionResult> ObterPorCodigoBarras(string codigoBarras)
    {
        var response = await _produtoService.ObterPorCodigoBarrasAsync(codigoBarras);
        if (!response.Sucesso) return NotFound(response);
        return Ok(response);
    }

    /// <summary>
    /// Checa se há saldo de estoque suficiente para a quantidade especificada.
    /// </summary>
    [HttpGet("{produtoId}/estoque-disponivel")]
    public async Task<IActionResult> PossuiEstoque(int produtoId, [FromQuery] int quantidade)
    {
        var response = await _produtoService.PossuiEstoqueAsync(produtoId, quantidade);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Retorna o histórico de movimentações (entradas/saídas/perdas) de um determinado produto.
    /// </summary>
    [HttpGet("{produtoId}/movimentacoes")]
    public async Task<IActionResult> ObterMovimentacoesPorProduto(int produtoId, [FromQuery] DateTime? inicio = null, [FromQuery] DateTime? fim = null)
    {
        var response = await _produtoService.ListarMovimentacoesPorProdutoAsync(produtoId, inicio, fim);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Cadastra um novo produto no catálogo.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarProdutoDto dto)
    {
        var response = await _produtoService.CriarAsync(dto);
        if (!response.Sucesso) return BadRequest(response);
        return StatusCode(201, response);
    }

    /// <summary>
    /// Atualiza os dados de cadastro de um produto existente.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarProdutoDto dto)
    {
        var response = await _produtoService.AtualizarAsync(id, dto);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Realiza uma movimentação manual de estoque (entrada de lote, perda ou uso interno).
    /// </summary>
    [HttpPost("movimentar-estoque")]
    public async Task<IActionResult> MovimentarEstoque([FromBody] MovimentarEstoqueDto dto)
    {
        var response = await _produtoService.MovimentarEstoqueAsync(dto);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Desativa um produto (exclusão lógica).
    /// </summary>
    [HttpPatch("{id}/desativar")]
    public async Task<IActionResult> Desativar(int id)
    {
        var response = await _produtoService.DesativarAsync(id);
        if (!response.Sucesso) return NotFound(response);
        return Ok(response);
    }

    /// <summary>
    /// Reativa um produto desativado.
    /// </summary>
    [HttpPatch("{id}/ativar")]
    public async Task<IActionResult> Ativar(int id)
    {
        var response = await _produtoService.AtivarAsync(id);
        if (!response.Sucesso) return NotFound(response);
        return Ok(response);
    }

    /// <summary>
    /// Remove permanentemente um produto do banco de dados (exclusão física).
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoverPermanentemente(int id)
    {
        var response = await _produtoService.RemoverPermanentementeAsync(id);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }
}