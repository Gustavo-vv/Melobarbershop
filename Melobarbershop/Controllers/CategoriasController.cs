using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Interfaces;

namespace Melobarbershop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaServicoAppService _categoriaService;

        public CategoriasController(ICategoriaServicoAppService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterAtivas()
        {
            var resposta = await _categoriaService.ObterTodasAsync(incluirInativas: false);
            return Ok(resposta);
        }

        [HttpGet("todas")]
        public async Task<IActionResult> ObterTodas()
        {
            var resposta = await _categoriaService.ObterTodasAsync(incluirInativas: true);
            return Ok(resposta);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var resposta = await _categoriaService.ObterPorIdAsync(id);
            if (!resposta.Sucesso) return NotFound(resposta);
            return Ok(resposta);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] CriarCategoriaServicoDto dto)
        {
            var resposta = await _categoriaService.CadastrarAsync(dto);
            if (!resposta.Sucesso) return BadRequest(resposta);
            return StatusCode(201, resposta);
        }

        [HttpDelete("{id}/desativar")]
        public async Task<IActionResult> Desativar(int id)
        {
            var resposta = await _categoriaService.DesativarAsync(id);
            if (!resposta.Sucesso) return NotFound(resposta);
            return Ok(resposta);
        }

        [HttpPut("{id}/reativar")]
        public async Task<IActionResult> Reativar(int id)
        {
            var resposta = await _categoriaService.ReativarAsync(id);
            if (!resposta.Sucesso) return NotFound(resposta);
            return Ok(resposta);
        }
    }
}