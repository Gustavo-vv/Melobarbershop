// ============================================================================
// Arquivo: PacotesController.cs
// Camada: Melobarbershop.API (Controllers)
// Objetivo: Expor endpoints REST para gerenciamento de pacotes promocionais de serviços.
// Papel na Arquitetura:
//   - Recebe requisições HTTP para listagem, criação, atualização e ativação de pacotes.
//   - Delega as regras de negócio para a interface IPacoteService.
// ============================================================================

using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Services;
using Microsoft.AspNetCore.Mvc;

namespace Melobarbershop.API.Controllers;

/// <summary>
/// Controlador responsável pelas operações de cadastro e consulta de pacotes de serviços.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PacotesController : ControllerBase
{
    private readonly IPacoteService _pacoteService;

    /// <summary>
    /// Construtor com injeção do serviço de pacotes.
    /// </summary>
    public PacotesController(IPacoteService pacoteService)
    {
        _pacoteService = pacoteService;
    }

    /// <summary>
    /// Obtém os dados de um pacote específico pelo seu ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var response = await _pacoteService.ObterPorIdAsync(id);
        if (!response.Sucesso) return NotFound(response);
        return Ok(response);
    }

    /// <summary>
    /// Lista apenas os pacotes de serviços que estão atualmente ativos.
    /// </summary>
    [HttpGet("ativos")]
    public async Task<IActionResult> ListarAtivos()
    {
        var response = await _pacoteService.ListarAtivosAsync();
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Lista todos os pacotes cadastrados, incluindo os inativos.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ListarTodos()
    {
        var response = await _pacoteService.ListarTodosAsync();
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Cadastra um novo pacote de serviços no sistema.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarPacoteDto dto)
    {
        var response = await _pacoteService.CriarAsync(dto);
        if (!response.Sucesso) return BadRequest(response);
        return StatusCode(201, response);
    }

    /// <summary>
    /// Atualiza as informações de um pacote existente.
    /// </summary>
    [HttpPut("{id}/atualizar")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarPacoteDto dto)
    {
        var response = await _pacoteService.AtualizarAsync(id, dto);
        if (!response.Sucesso) return NotFound(response);
        return Ok(response);
    }

    /// <summary>
    /// Inativa um pacote de serviços impedindo novas vendas ou agendamentos.
    /// </summary>
    [HttpPatch("{id}/desativar")]
    public async Task<IActionResult> Desativar(int id)
    {
        var response = await _pacoteService.DesativarAsync(id);
        if (!response.Sucesso) return NotFound(response);
        return Ok(response);
    }

    /// <summary>
    /// Reativa um pacote de serviços que estava desativado.
    /// </summary>
    [HttpPatch("{id}/ativar")]
    public async Task<IActionResult> Ativar(int id)
    {
        var response = await _pacoteService.AtivarAsync(id);
        if (!response.Sucesso) return NotFound(response);
        return Ok(response);
    }
}