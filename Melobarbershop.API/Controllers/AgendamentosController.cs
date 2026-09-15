using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Melobarbershop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgendamentosController : ControllerBase
    {
        private readonly IAgendamentoService _agendamentoService;
        public AgendamentosController(IAgendamentoService agendamentoService)
        {
            _agendamentoService = agendamentoService;
        }

        [HttpGet("id")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var response = await _agendamentoService.ObterPorIdAsync(id);
            if (!response.Sucesso) return NotFound(response);
            return Ok(response);
        }

        [HttpGet("periodo")]
        public async Task<IActionResult> ListarPorPeriodo([FromQuery] DateTime inicio, DateTime fim)
        {
            var response = await _agendamentoService.ListarPorPeriodoAsync(inicio, fim);
            if (!response.Sucesso) return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("cliente/{clienteId}")]
        public async Task<IActionResult> ListarPorCliente(string clienteId)
        {
            var response = await _agendamentoService.ListarPorClienteAsync(clienteId);
            if (!response.Sucesso) return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("horarios-disponiveis")]
        public async Task<IActionResult> ListarHorariosDisponiveis([FromQuery] string barbeiroId, [FromQuery] DateTime data, [FromQuery] IEnumerable<int> servicoIds)
        {
            var response = await _agendamentoService.ListarHorariosDisponiveisAsync(barbeiroId, data, servicoIds);
            if (!response.Sucesso) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarAgendamentoDto dto)
        {
            var response = await _agendamentoService.CriarAsync(dto);
            if (!response.Sucesso) return BadRequest(response);
            return StatusCode(201, response);
        }

        [HttpPatch("{id}/confirmar")]
        public async Task<IActionResult> Confirmar(int id)
        {
            var response = await _agendamentoService.ConfirmarAsync(id);
            if (!response.Sucesso) return BadRequest(response);
            return Ok(response);
        }
    }
}
