using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Services;
using Microsoft.AspNetCore.Mvc;

namespace Melobarbershop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PacotesController : ControllerBase
{
    private readonly IPacoteService _pacoteService;

    public PacotesController(IPacoteService pacoteService)
    {
        _pacoteService = pacoteService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var response = await _pacoteService.ObterPorIdAsync(id);
        if (!response.Sucesso) return NotFound(response);
        return Ok(response);
    }

    [HttpGet("ativos")]
    public async Task<IActionResult> ListarAtivos()
    {
        var response = await _pacoteService.ListarAtivosAsync();
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> ListarTodos()
    {
        var response = await _pacoteService.ListarTodosAsync();
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarPacoteDto dto)
    {
        var response = await _pacoteService.CriarAsync(dto);
        if (!response.Sucesso) return BadRequest(response);
        return StatusCode(201, response);
    }

    [HttpPut("{id}/atualizar")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarPacoteDto dto)
    {
        var response = await _pacoteService.AtualizarAsync(id, dto);
        if (!response.Sucesso) return NotFound(response);
        return Ok(response);
    }

    [HttpPatch("{id}/desativar")]
    public async Task<IActionResult> Desativar(int id)
    {
        var response = await _pacoteService.DesativarAsync(id);
        if (!response.Sucesso) return NotFound(response);
        return Ok(response);
    }

    [HttpPatch("{id}/ativar")]
    public async Task<IActionResult> Ativar(int id)
    {
        var response = await _pacoteService.AtivarAsync(id);
        if (!response.Sucesso) return NotFound(response);
        return Ok(response);
    }
}