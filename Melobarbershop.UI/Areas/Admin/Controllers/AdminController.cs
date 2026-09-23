using Melobarbershop.UI.Areas.Admin.Models;
using Melobarbershop.UI.Areas.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Melobarbershop.UI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IPainelDadosService _painelDadosService;

    public AdminController(IPainelDadosService painelDadosService)
    {
        _painelDadosService = painelDadosService;
    }

    // Visão Geral
    public IActionResult Index()
    {
        return View();
    }

    // Agenda dedicada
    public IActionResult Agenda()
    {
        return View("Index");
    }

    // Endpoint JSON que alimenta o Dashboard via API
    [HttpGet]
    public async Task<IActionResult> Dados([FromQuery] string periodo = "hoje", [FromQuery] string profissionalId = "todos", [FromQuery] string origem = "todas")
    {
        var dados = await _painelDadosService.ObterDadosPainelAsync(periodo, profissionalId, origem);
        return Json(dados);
    }

    // Endpoint POST que repassa a alteração de status para a API
    [HttpPost]
    public async Task<IActionResult> AlterarStatusAgendamento([FromBody] AlterarStatusRequest request)
    {
        if (request == null || request.Id <= 0 || string.IsNullOrWhiteSpace(request.Acao))
        {
            return BadRequest(new { sucesso = false, mensagem = "Dados da solicitação inválidos." });
        }

        var sucesso = await _painelDadosService.AlterarStatusAgendamentoAsync(request.Id, request.Acao, request.Motivo);
        if (!sucesso)
        {
            return StatusCode(500, new { sucesso = false, mensagem = "Não foi possível atualizar o agendamento na API." });
        }

        return Ok(new { sucesso = true, mensagem = "Status do agendamento alterado com sucesso." });
    }
}
