using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Services;
using Microsoft.AspNetCore.Mvc;

namespace Melobarbershop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendasController : ControllerBase
    {
        private readonly IVendaService _vendaService;
        public VendasController(IVendaService vendaService)
        {
            _vendaService = vendaService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var response = await _vendaService.ObterPorIdAsync(id);
            if (!response.Sucesso) return NotFound(response);
            return Ok(response);
        }

        [HttpGet("periodo")]
        public async Task<IActionResult> ListarPorPeriodo([FromQuery] DateTime inicio, [FromQuery] DateTime fim)
        {
            var response = await _vendaService.ListarPorPeriodoAsync(inicio, fim);
            if (!response.Sucesso) return StatusCode(500, response);
            return Ok(response);
        }

        [HttpGet("cliente/{clienteId}")]
        public async Task<IActionResult> ListarPorCliente(string clienteId)
        {
            var response = await _vendaService.ListarPorClienteAsync(clienteId);
            if (!response.Sucesso) return StatusCode(500, response);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> IniciarVenda([FromBody] IniciarVendaDto dto)
        {
            var response = await _vendaService.IniciarVendaAsync(dto);
            if (!response.Sucesso) return NotFound(response);
            return CreatedAtAction(nameof(ObterPorId), new { id = response.Dados!.Id }, response);
        }

        [HttpPost("{vendaId}/itens/servico")]
        public async Task<IActionResult> AdicionarItemServico(int vendaId, [FromBody] AdicionarItemServicoDto dto)
        {
            var response = await _vendaService.AdicionarItemServicoAsync(vendaId, dto);
            if (!response.Sucesso) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("{vendaId}/itens/produto")]
        public async Task<IActionResult> AdicionarItemProduto(int vendaId, [FromBody] AdicionarItemProdutoDto dto)
        {
            var response = await _vendaService.AdicionarItemProdutoAsync(vendaId, dto);
            if (!response.Sucesso) return BadRequest(response);
            return Ok(response);
        }

        [HttpDelete("{vendaId}/itens/{vendaItemId}")]
        public async Task<IActionResult> RemoverItem(int vendaId, int vendaItemId)
        {
            var response = await _vendaService.RemoverItemAsync(vendaId, vendaItemId);
            if (!response.Sucesso) return NotFound(response);
            return Ok(response);
        }

        [HttpPatch("{vendaId}/desconto")]
        public async Task<IActionResult> AplicarDesconto(int vendaId, [FromBody] decimal valorDesconto)
        {
            var response = await _vendaService.AplicarDescontoAsync(vendaId, valorDesconto);
            if (!response.Sucesso) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("{vendaId}/finalizar")]
        public async Task<IActionResult> FinalizarVenda(int vendaId)
        {
            var response = await _vendaService.FinalizarVendaAsync(vendaId);
            if (!response.Sucesso) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("{vendaId}/cancelar")]
        public async Task<IActionResult> CancelarVenda(int vendaId, [FromBody] string motivo)
        {
            var response = await _vendaService.CancelarVendaAsync(vendaId, motivo);
            if (!response.Sucesso) return BadRequest(response);
            return Ok(response);
        }
    }
}