// ============================================================================
// Arquivo: ServicosController.cs
// Camada: Melobarbershop.API (Controllers)
// Objetivo: Expor endpoints REST para gerenciamento de serviços prestados pela barbearia.
// Papel na Arquitetura:
//   - Fornece operações de CRUD para os serviços (corte, barba, pigmentação, etc.).
//   - Suporta desativação lógica e reativação pelo painel Desktop ou Web.
// ============================================================================

using AutoMapper;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Melobarbershop.API.Controllers;

/// <summary>
/// Controlador responsável pelas operações relacionadas aos serviços da barbearia.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ServicosController : ControllerBase
{
    private readonly IServicoService _servicoService;

    /// <summary>
    /// Construtor com injeção do serviço de regras de negócio de serviços.
    /// </summary>
    public ServicosController(IServicoService servicoService)
    {
        _servicoService = servicoService;        
    }

    /// <summary>
    /// Retorna todos os serviços cadastrados, incluindo os inativos.
    /// </summary>
    [HttpGet("todos")]
    public async Task<IActionResult> ObterTodos()
    {
        var response = await _servicoService.ListarAsync(incluirInativos: true);
        return Ok(response);
    }

    /// <summary>
    /// Retorna apenas os serviços atualmente ativos no catálogo.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ObterAtivas()
    {
        var response = await _servicoService.ListarAsync(incluirInativos: false);
        return Ok(response);
    }

    /// <summary>
    /// Busca os detalhes de um serviço específico pelo ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var response = await _servicoService.ObterPorIdAsync(id);
        if (!response.Sucesso) return NotFound(response);
        return Ok(response);
    }

    /// <summary>
    /// Cadastra um novo serviço na barbearia.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Cadastrar([FromBody] CriarServicoDto dto)
    {
        var response = await _servicoService.CriarAsync(dto);
        if (!response.Sucesso) return BadRequest(response);
        return StatusCode(201, response);
    }

    /// <summary>
    /// Atualiza os dados de um serviço existente.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarServicoDto dto)
    {
        var response = await _servicoService.AtualizarAsync(id, dto);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Desativa logicamente um serviço para que não apareça em novos agendamentos.
    /// </summary>
    [HttpDelete("{id}/desativar")]
    public async Task<IActionResult> Desativar(int id)
    {
        var response = await _servicoService.DesativarAsync(id);
        if (!response.Sucesso) return NotFound(response);
        return Ok(response);
    }

    /// <summary>
    /// Reativa um serviço previamente inativado.
    /// </summary>
    [HttpPut("{id}/reativar")]
    public async Task<IActionResult> Reativar(int id)
    {
        var response = await _servicoService.AtivarAsync(id);
        if (!response.Sucesso) return NotFound(response);
        return Ok(response);
    }

    /// <summary>
    /// Remove permanentemente um serviço do banco de dados (exclusão física).
    /// </summary>
    [HttpDelete("{id}/permanente")]
    public async Task<IActionResult> ExcluirPermanente(int id)
    {
        var response = await _servicoService.RemoverPermanentementeAsync(id);
        if (!response.Sucesso) return NotFound(response);
        return Ok(response);
    }
}
