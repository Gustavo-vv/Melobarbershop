using AutoMapper;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Melobarbershop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicosController : ControllerBase
    {
        private readonly IServicoService _servicoService;
        private readonly IMapper _mapper;

        public ServicosController(IServicoService servicoService, IMapper mapper)
        {
            _servicoService = servicoService;
            _mapper = mapper;            
        }

        [HttpGet("todos")]
        public async Task<IActionResult> ObterTodos()
        {
            var response = await _servicoService.ListarAsync(incluirInativos: true);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> ObterAtivas()
        {
            var response = await _servicoService.ListarAsync(incluirInativos: false);
            return Ok(response);
        }

        [HttpGet("id")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var response = await _servicoService.ObterPorIdAsync(id);
            if (!response.Sucesso) return NotFound(response);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] CriarServicoDto dto)
        {
            var response = await _servicoService.CriarAsync(dto);
            if (!response.Sucesso) return BadRequest(response);
            return StatusCode(201, response);
        }
    }
}
