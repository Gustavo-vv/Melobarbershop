using System.Text.Json;
using Melobarbershop.Application.DTOs;
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
    private readonly IHttpClientFactory _httpClientFactory;
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public AdminController(IPainelDadosService painelDadosService, IHttpClientFactory httpClientFactory)
    {
        _painelDadosService = painelDadosService;
        _httpClientFactory = httpClientFactory;
    }

    // Visão Geral
    public IActionResult Index()
    {
        ViewData["ActiveNav"] = "visao-geral";
        ViewData["Secao"] = "visao-geral";
        return View();
    }

    // Agenda dedicada
    public IActionResult Agenda()
    {
        ViewData["ActiveNav"] = "agenda";
        ViewData["Secao"] = "agenda";
        return View("Index");
    }

    // Gestão de Serviços
    public IActionResult Servicos()
    {
        ViewData["ActiveNav"] = "servicos";
        ViewData["Secao"] = "servicos";
        ViewData["PageTitle"] = "Gestão de Serviços";
        ViewData["PageSubtitle"] = "Cadastro, edição, precificação e visibilidade no catálogo";
        return View("Index");
    }

    // Endpoint JSON que alimenta o Dashboard e a Agenda via API
    [HttpGet]
    public async Task<IActionResult> Dados(
        [FromQuery] string periodo = "hoje", 
        [FromQuery] string profissionalId = "todos", 
        [FromQuery] string origem = "todas",
        [FromQuery] DateTime? inicioPersonalizado = null,
        [FromQuery] DateTime? fimPersonalizado = null)
    {
        var dados = await _painelDadosService.ObterDadosPainelAsync(
            periodo, 
            profissionalId, 
            origem, 
            inicioPersonalizado, 
            fimPersonalizado);

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

    // ==========================================
    // ENDPOINTS DE SERVIÇOS (PROXY DIRETO PRA API)
    // ==========================================

    [HttpGet]
    public async Task<IActionResult> ServicosDados()
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var resp = await client.GetAsync("/api/Servicos/todos");
        var content = await resp.Content.ReadAsStringAsync();
        return Content(content, "application/json");
    }

    [HttpPost]
    public async Task<IActionResult> ServicosCriar([FromBody] CriarServicoDto dto)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var resp = await client.PostAsJsonAsync("/api/Servicos", dto);
        var content = await resp.Content.ReadAsStringAsync();
        return StatusCode((int)resp.StatusCode, content);
    }

    [HttpPost]
    public async Task<IActionResult> ServicosAtualizar([FromQuery] int id, [FromBody] AtualizarServicoDto dto)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var resp = await client.PutAsJsonAsync($"/api/Servicos/id?id={id}", dto);
        var content = await resp.Content.ReadAsStringAsync();
        return StatusCode((int)resp.StatusCode, content);
    }

    [HttpPost]
    public async Task<IActionResult> ServicosAlternarStatus([FromBody] AlterarStatusRequest req)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        HttpResponseMessage resp;
        if (req.Ativar == true)
        {
            resp = await client.PutAsync($"/api/Servicos/{req.Id}/reativar", null);
        }
        else
        {
            resp = await client.DeleteAsync($"/api/Servicos/{req.Id}/desativar");
        }

        var content = await resp.Content.ReadAsStringAsync();
        return StatusCode((int)resp.StatusCode, content);
    }

    [HttpPost]
    public async Task<IActionResult> ServicosExcluir([FromQuery] int id)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var resp = await client.DeleteAsync($"/api/Servicos/{id}/permanente");
        var content = await resp.Content.ReadAsStringAsync();
        return StatusCode((int)resp.StatusCode, content);
    }
}
