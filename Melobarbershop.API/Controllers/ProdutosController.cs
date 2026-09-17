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
    }
}

//Task<ApiResposta<IEnumerable<ProdutoDto>>> ListarComEstoqueAbaixoDoMinimoAsync();
//Task<ApiResposta<ProdutoDto>> CriarAsync(CriarProdutoDto dto);
//Task<ApiResposta<ProdutoDto>> AtualizarAsync(int id, AtualizarProdutoDto dto);
//Task<ApiResposta<ProdutoDto>> MovimentarEstoqueAsync(MovimentarEstoqueDto dto);
//Task<ApiResposta<IEnumerable<MovimentacaoEstoqueDto>>> ListarMovimentacoesPorProdutoAsync(int produtoId, DateTime? inicio = null, DateTime? fim = null);
//Task<ApiResposta<bool>> PossuiEstoqueAsync(int produtoId, int quantidade);
//Task<ApiResposta<ProdutoDto>> DesativarAsync(int id);
//Task<ApiResposta<ProdutoDto>> AtivarAsync(int id);
//Task<ApiResposta<ProdutoDto>> RemoverPermanentementeAsync(int id);