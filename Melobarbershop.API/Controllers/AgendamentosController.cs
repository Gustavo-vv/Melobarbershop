// ============================================================================
// Arquivo: AgendamentosController.cs
// Camada: Melobarbershop.API (Controllers)
// Objetivo: Expor endpoints REST para gerenciamento de agendamentos, verificação
//           de horários disponíveis e transições de status do atendimento.
// Papel na Arquitetura:
//   - Recebe as requisições HTTP (GET, POST, PATCH) dos clientes Web, Mobile ou Desktop.
//   - Delega a execução para IAgendamentoService e retorna códigos HTTP semânticos (200, 201, 400, 404).
// ============================================================================

using Melobarbershop.Application.DTOs;
using Melobarbershop.Application.Servicos.Services;
using Microsoft.AspNetCore.Mvc;

namespace Melobarbershop.API.Controllers;

/// <summary>
/// Controlador responsável pelas operações de agendamento de serviços da barbearia.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AgendamentosController : ControllerBase
{
    private readonly IAgendamentoService _agendamentoService;

    /// <summary>
    /// Construtor com injeção do serviço de agendamentos.
    /// </summary>
    public AgendamentosController(IAgendamentoService agendamentoService)
    {
        _agendamentoService = agendamentoService;
    }

    /// <summary>
    /// Obtém um agendamento pelo seu ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var response = await _agendamentoService.ObterPorIdAsync(id);
        if (!response.Sucesso) return NotFound(response);
        return Ok(response);
    }

    /// <summary>
    /// Lista agendamentos em um intervalo de datas especificado.
    /// </summary>
    [HttpGet("periodo")]
    public async Task<IActionResult> ListarPorPeriodo([FromQuery] DateTime inicio, [FromQuery] DateTime fim)
    {
        var response = await _agendamentoService.ListarPorPeriodoAsync(inicio, fim);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Retorna o histórico de agendamentos de um determinado cliente.
    /// </summary>
    [HttpGet("cliente/{clienteId}")]
    public async Task<IActionResult> ListarPorCliente(string clienteId)
    {
        var response = await _agendamentoService.ListarPorClienteAsync(clienteId);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Retorna somente os horários livres na agenda para o barbeiro e serviços selecionados na data.
    /// </summary>
    [HttpGet("horarios-disponiveis")]
    public async Task<IActionResult> ListarHorariosDisponiveis([FromQuery] string barbeiroId, [FromQuery] DateTime data, [FromQuery] IEnumerable<int> servicoIds)
    {
        var response = await _agendamentoService.ListarHorariosDisponiveisAsync(barbeiroId, data, servicoIds);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Retorna todos os horários da grade do dia com indicador de disponibilidade (livre ou ocupado).
    /// </summary>
    [HttpGet("horarios-do-dia")]
    public async Task<IActionResult> ListarHorariosDoDia([FromQuery] string barbeiroId, [FromQuery] DateTime data, [FromQuery] IEnumerable<int> servicoIds)
    {
        var response = await _agendamentoService.ListarTodosHorariosDoDiaAsync(barbeiroId, data, servicoIds);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Registra uma nova solicitação de agendamento na barbearia.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarAgendamentoDto dto)
    {
        var response = await _agendamentoService.CriarAsync(dto);
        if (!response.Sucesso) return BadRequest(response);
        return StatusCode(201, response);
    }

    /// <summary>
    /// Confirma um agendamento pendente.
    /// </summary>
    [HttpPatch("{id}/confirmar")]
    public async Task<IActionResult> Confirmar(int id)
    {
        var response = await _agendamentoService.ConfirmarAsync(id);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Registra o início do atendimento com o cliente na cadeira do barbeiro.
    /// </summary>
    [HttpPatch("{id}/iniciar-atendimento")]
    public async Task<IActionResult> IniciarAtendimento(int id)
    {
        var response = await _agendamentoService.IniciarAtendimentoAsync(id);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Marca o atendimento do agendamento como concluído.
    /// </summary>
    [HttpPatch("{id}/concluir")]
    public async Task<IActionResult> Concluir(int id)
    {
        var response = await _agendamentoService.ConcluirAsync(id);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Cancela o agendamento registrando opcionalmente o motivo.
    /// </summary>
    [HttpPatch("{id}/cancelar")]
    public async Task<IActionResult> Cancelar(int id, [FromQuery] string? motivo = null)
    {
        var response = await _agendamentoService.CancelarAsync(id, motivo);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Registra a falta do cliente ao horário agendado (No-Show).
    /// </summary>
    [HttpPatch("{id}/nao-comparecimento")]
    public async Task<IActionResult> RegistrarNaoComparecimento(int id)
    {
        var response = await _agendamentoService.RegistrarNaoComparecimentoAsync(id);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Altera a data, horário ou barbeiro de um agendamento existente.
    /// </summary>
    [HttpPatch("{id}/reagendar")]
    public async Task<IActionResult> Reagendar(int id, [FromBody] ReagendarAgendamentoDto dto)
    {
        var response = await _agendamentoService.ReagendarAsync(id, dto);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }
}
