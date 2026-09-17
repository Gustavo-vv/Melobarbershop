using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Melobarbershop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        public ProdutosController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet("todos")]
        public async Task<IActionResult> ObterTodos()
        {
            var response = await _produtoService.ListarTodosAsync();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> ObterAtivas()
        {
            var response = await _produtoService.ListarAtivosAsync();
            return Ok(response);
        }

        [HttpGet("estoque-baixo")]
        public async Task<IActionResult> ObterComEstoqueAbaixoDoMinimo()
        {
            var response = await _produtoService.ListarComEstoqueAbaixoDoMinimoAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var response = await _produtoService.ObterPorIdAsync(id);
            if (!response.Sucesso) return NotFound(response);
            return Ok(response);
        }

        [HttpGet("codigoBarra/{codigoBarras}")]
        public async Task<IActionResult> ObterPorCodigoBarras(string codigoBarras)
        {
            var response = await _produtoService.ObterPorCodigoBarrasAsync(codigoBarras);
            if (!response.Sucesso) return NotFound(response);
            return Ok(response);
        }

        [HttpGet("{produtoId}/estoque-disponivel")]
        public async Task<IActionResult> PossuiEstoque(int produtoId, [FromQuery] int quantidade)
        {
            var response = await _produtoService.PossuiEstoqueAsync(produtoId, quantidade);
            if (!response.Sucesso) return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("{produtoId}/movimentacoes")]
        public async Task<IActionResult> ObterMovimentacoesPorProduto(int produtoId, [FromQuery] DateTime? inicio = null, [FromQuery] DateTime? fim = null)
        {
            var response = await _produtoService.ListarMovimentacoesPorProdutoAsync(produtoId, inicio, fim);
            if (!response.Sucesso) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarProdutoDto dto)
        {
            var response = await _produtoService.CriarAsync(dto);
            if (!response.Sucesso) return BadRequest(response);
            return StatusCode(201, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarProdutoDto dto)
        {
            var response = await _produtoService.AtualizarAsync(id, dto);
            if (!response.Sucesso) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("movimentar-estoque")]
        public async Task<IActionResult> MovimentarEstoque([FromBody] MovimentarEstoqueDto dto)
        {
            var response = await _produtoService.MovimentarEstoqueAsync(dto);
            if (!response.Sucesso) return BadRequest(response);
            return Ok(response);
        }

        [HttpPatch("{id}/desativar")]
        public async Task<IActionResult> Desativar(int id)
        {
            var response = await _produtoService.DesativarAsync(id);
            if (!response.Sucesso) return NotFound(response);
            return Ok(response);
        }

        [HttpPatch("{id}/ativar")]
        public async Task<IActionResult> Ativar(int id)
        {
            var response = await _produtoService.AtivarAsync(id);
            if (!response.Sucesso) return NotFound(response);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverPermanentemente(int id)
        {
            var response = await _produtoService.RemoverPermanentementeAsync(id);
            if (!response.Sucesso) return BadRequest(response);
            return Ok(response);
        }
    }
}