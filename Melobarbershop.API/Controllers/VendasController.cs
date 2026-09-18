// ============================================================================
// Arquivo: VendasController.cs
// Camada: Melobarbershop.API (Controllers)
// Objetivo: Expor endpoints REST para controle de vendas, comandas e caixa (PDV).
// Papel na Arquitetura:
//   - Recebe requisições de abertura de comanda, adição de itens, aplicação de desconto e fechamento financeiro.
//   - Delega a lógica de checkout e baixa de estoque para IVendaService.
// ============================================================================

using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Services;
using Microsoft.AspNetCore.Mvc;

namespace Melobarbershop.API.Controllers;

/// <summary>
/// Controlador responsável pelas operações de frente de caixa e comandas de venda (PDV).
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class VendasController : ControllerBase
{
    private readonly IVendaService _vendaService;

    /// <summary>
    /// Construtor com injeção do serviço de vendas.
    /// </summary>
    public VendasController(IVendaService vendaService)
    {
        _vendaService = vendaService;
    }

    /// <summary>
    /// Obtém os detalhes completos de uma comanda de venda pelo ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var response = await _vendaService.ObterPorIdAsync(id);
        if (!response.Sucesso) return NotFound(response);
        return Ok(response);
    }

    /// <summary>
    /// Lista as vendas realizadas em um intervalo de datas.
    /// </summary>
    [HttpGet("periodo")]
    public async Task<IActionResult> ListarPorPeriodo([FromQuery] DateTime inicio, [FromQuery] DateTime fim)
    {
        var response = await _vendaService.ListarPorPeriodoAsync(inicio, fim);
        if (!response.Sucesso) return StatusCode(500, response);
        return Ok(response);
    }

    /// <summary>
    /// Lista o histórico de vendas associado a um determinado cliente.
    /// </summary>
    [HttpGet("cliente/{clienteId}")]
    public async Task<IActionResult> ListarPorCliente(string clienteId)
    {
        var response = await _vendaService.ListarPorClienteAsync(clienteId);
        if (!response.Sucesso) return StatusCode(500, response);
        return Ok(response);
    }

    /// <summary>
    /// Abre uma nova comanda de venda no caixa.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> IniciarVenda([FromBody] IniciarVendaDto dto)
    {
        var response = await _vendaService.IniciarVendaAsync(dto);
        if (!response.Sucesso) return NotFound(response);
        return CreatedAtAction(nameof(ObterPorId), new { id = response.Dados!.Id }, response);
    }

    /// <summary>
    /// Adiciona um serviço com respectivo barbeiro responsável à comanda aberta.
    /// </summary>
    [HttpPost("{vendaId}/itens/servico")]
    public async Task<IActionResult> AdicionarItemServico(int vendaId, [FromBody] AdicionarItemServicoDto dto)
    {
        var response = await _vendaService.AdicionarItemServicoAsync(vendaId, dto);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Adiciona um ou mais produtos à comanda aberta validando estoque.
    /// </summary>
    [HttpPost("{vendaId}/itens/produto")]
    public async Task<IActionResult> AdicionarItemProduto(int vendaId, [FromBody] AdicionarItemProdutoDto dto)
    {
        var response = await _vendaService.AdicionarItemProdutoAsync(vendaId, dto);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Remove um item da comanda aberta e recalcula os totais.
    /// </summary>
    [HttpDelete("{vendaId}/itens/{vendaItemId}")]
    public async Task<IActionResult> RemoverItem(int vendaId, int vendaItemId)
    {
        var response = await _vendaService.RemoverItemAsync(vendaId, vendaItemId);
        if (!response.Sucesso) return NotFound(response);
        return Ok(response);
    }

    /// <summary>
    /// Aplica um valor de desconto fixo sobre o total da comanda.
    /// </summary>
    [HttpPatch("{vendaId}/desconto")]
    public async Task<IActionResult> AplicarDesconto(int vendaId, [FromBody] decimal valorDesconto)
    {
        var response = await _vendaService.AplicarDescontoAsync(vendaId, valorDesconto);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Finaliza e liquida a comanda, abatendo estoque e concluindo o atendimento.
    /// </summary>
    [HttpPost("{vendaId}/finalizar")]
    public async Task<IActionResult> FinalizarVenda(int vendaId)
    {
        var response = await _vendaService.FinalizarVendaAsync(vendaId);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Cancela uma venda/comanda aberta sem pagamentos processados.
    /// </summary>
    [HttpPost("{vendaId}/cancelar")]
    public async Task<IActionResult> CancelarVenda(int vendaId, [FromBody] string motivo)
    {
        var response = await _vendaService.CancelarVendaAsync(vendaId, motivo);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }
}