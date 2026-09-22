using System;
using System.Threading.Tasks;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Services;
using Microsoft.AspNetCore.Mvc;

namespace Melobarbershop.API.Controllers;

/// <summary>
/// Controlador responsável pelo gerenciamento de horários de funcionamento padrão e datas especiais.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class HorarioFuncionamentoController : ControllerBase
{
    private readonly IHorarioFuncionamentoService _horarioService;

    public HorarioFuncionamentoController(IHorarioFuncionamentoService horarioService)
    {
        _horarioService = horarioService;
    }

    /// <summary>
    /// Lista a configuração dos 7 dias da semana.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ListarSemana()
    {
        var response = await _horarioService.ListarSemanaAsync();
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Atualiza o horário de funcionamento de um dia específico da semana.
    /// </summary>
    [HttpPut("{diaSemana}")]
    public async Task<IActionResult> AtualizarDia(DayOfWeek diaSemana, [FromBody] AtualizarHorarioFuncionamentoDto dto)
    {
        var response = await _horarioService.AtualizarDiaAsync(diaSemana, dto.Aberto, dto.HoraAbertura, dto.HoraFechamento);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Lista os horários/datas especiais configurados, opcionalmente filtrando por ano.
    /// </summary>
    [HttpGet("especiais")]
    public async Task<IActionResult> ListarEspeciais([FromQuery] int? ano = null)
    {
        var response = await _horarioService.ListarEspeciaisAsync(ano);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Cadastra ou atualiza um horário especial para uma data comemorativa ou feriado.
    /// </summary>
    [HttpPost("especiais")]
    public async Task<IActionResult> CriarEspecial([FromBody] CriarHorarioEspecialDto dto)
    {
        var response = await _horarioService.CriarEspecialAsync(dto.Data, dto.Aberto, dto.HoraAbertura, dto.HoraFechamento, dto.Descricao);
        if (!response.Sucesso) return BadRequest(response);
        return StatusCode(201, response);
    }

    /// <summary>
    /// Remove um horário especial pelo seu ID.
    /// </summary>
    [HttpDelete("especiais/{id}")]
    public async Task<IActionResult> RemoverEspecial(int id)
    {
        var response = await _horarioService.RemoverEspecialAsync(id);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }
}
