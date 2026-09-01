using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Interfaces;

namespace Melobarbershop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgendamentosController : ControllerBase
    {
        private readonly IAgendamentoAppService _agendamentoService;

        public AgendamentosController(IAgendamentoAppService agendamentoService)
        {
            _agendamentoService = agendamentoService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var resposta = await _agendamentoService.ObterTodosAsync();
            return Ok(resposta);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var resposta = await _agendamentoService.ObterPorIdAsync(id);
            if (!resposta.Sucesso) return NotFound(resposta);
            return Ok(resposta);
        }

        [HttpGet("cliente/{clienteId}")]
        public async Task<IActionResult> ObterPorCliente(string clienteId)
        {
            var resposta = await _agendamentoService.ObterPorClienteAsync(clienteId);
            return Ok(resposta);
        }

        [HttpGet("barbeiro/{barbeiroId}")]
        public async Task<IActionResult> ObterPorBarbeiro(string barbeiroId, [FromQuery] DateTime? data = null)
        {
            var resposta = await _agendamentoService.ObterPorBarbeiroAsync(barbeiroId, data);
            return Ok(resposta);
        }

        [HttpGet("horarios-disponiveis")]
        public async Task<IActionResult> ObterHorariosDisponiveis([FromQuery] string barbeiroId, [FromQuery] int servicoId, [FromQuery] DateTime data)
        {
            var resposta = await _agendamentoService.ObterHorariosDisponiveisAsync(barbeiroId, servicoId, data);
            return Ok(resposta);
        }

        [HttpPost]
        public async Task<IActionResult> CriarAgendamento([FromBody] CriarAgendamentoDto dto)
        {
            var resposta = await _agendamentoService.CriarAgendamentoAsync(dto);
            if (!resposta.Sucesso) return BadRequest(resposta);
            return StatusCode(201, resposta);
        }

        [HttpPut("{id}/status/{novoStatus}")]
        public async Task<IActionResult> AlterarStatus(int id, int novoStatus)
        {
            var resposta = await _agendamentoService.AlterarStatusAsync(id, novoStatus);
            if (!resposta.Sucesso) return BadRequest(resposta);
            return Ok(resposta);
        }

        [HttpDelete("{id}/cancelar")]
        public async Task<IActionResult> Cancelar(int id, [FromQuery] string usuarioId)
        {
            var resposta = await _agendamentoService.CancelarAgendamentoAsync(id, usuarioId);
            if (!resposta.Sucesso) return BadRequest(resposta);
            return Ok(resposta);
        }
    }
}