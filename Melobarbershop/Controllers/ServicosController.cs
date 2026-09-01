using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Interfaces;

namespace Melobarbershop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicosController : ControllerBase
    {
        private readonly IServicoAppService _servicoService;

        public ServicosController(IServicoAppService servicoService)
        {
            _servicoService = servicoService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterAtivos()
        {
            var resposta = await _servicoService.ObterTodosAsync(incluirInativos: false);
            return Ok(resposta);
        }

        [HttpGet("todos")]
        public async Task<IActionResult> ObterTodos()
        {
            var resposta = await _servicoService.ObterTodosAsync(incluirInativos: true);
            return Ok(resposta);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var resposta = await _servicoService.ObterPorIdAsync(id);
            if (!resposta.Sucesso) return NotFound(resposta);
            return Ok(resposta);
        }

        [HttpGet("categoria/{categoriaId}")]
        public async Task<IActionResult> ObterPorCategoria(int categoriaId)
        {
            var resposta = await _servicoService.ObterPorCategoriaAsync(categoriaId);
            return Ok(resposta);
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string? termo, [FromQuery] int? categoriaId = null)
        {
            var resposta = await _servicoService.BuscarAsync(termo, categoriaId);
            return Ok(resposta);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] CriarServicoDto dto)
        {
            var resposta = await _servicoService.CadastrarAsync(dto);
            if (!resposta.Sucesso) return BadRequest(resposta);
            return StatusCode(201, resposta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarServicoDto dto)
        {
            var resposta = await _servicoService.AtualizarAsync(id, dto);
            if (!resposta.Sucesso) return BadRequest(resposta);
            return Ok(resposta);
        }

        [HttpDelete("{id}/desativar")]
        public async Task<IActionResult> Desativar(int id)
        {
            var resposta = await _servicoService.DesativarAsync(id);
            if (!resposta.Sucesso) return NotFound(resposta);
            return Ok(resposta);
        }

        [HttpPut("{id}/reativar")]
        public async Task<IActionResult> Reativar(int id)
        {
            var resposta = await _servicoService.ReativarAsync(id);
            if (!resposta.Sucesso) return NotFound(resposta);
            return Ok(resposta);
        }
    }
}