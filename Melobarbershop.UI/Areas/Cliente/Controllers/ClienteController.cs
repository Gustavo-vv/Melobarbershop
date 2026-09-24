using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Melobarbershop.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Melobarbershop.UI.Areas.Cliente.Controllers;

[Area("Cliente")]
[Authorize]
public class ClienteController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ClienteController> _logger;

    public ClienteController(IHttpClientFactory httpClientFactory, ILogger<ClienteController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Perfil()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> PerfilDados()
    {
        var token = ObterTokenJwt();
        if (string.IsNullOrEmpty(token))
        {
            return StatusCode(401, new { sucesso = false, mensagem = "Sessão expirada ou não autenticada." });
        }

        try
        {
            var client = CriarApiClientComBearer(token);
            var resp = await client.GetAsync("/api/Usuarios/me");
            var content = await resp.Content.ReadAsStringAsync();
            Response.StatusCode = (int)resp.StatusCode;
            return Content(content, "application/json");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter dados do perfil do cliente.");
            return StatusCode(500, new { sucesso = false, mensagem = "Erro interno ao comunicar com o servidor." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> AtualizarDados([FromBody] AtualizarDadosClienteDto dto)
    {
        var token = ObterTokenJwt();
        if (string.IsNullOrEmpty(token))
        {
            return StatusCode(401, new { sucesso = false, mensagem = "Sessão expirada ou não autenticada." });
        }

        if (dto == null)
        {
            return BadRequest(new { sucesso = false, mensagem = "Dados inválidos." });
        }

        try
        {
            var client = CriarApiClientComBearer(token);
            var jsonContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var resp = await client.PutAsync("/api/Usuarios/me", jsonContent);
            var content = await resp.Content.ReadAsStringAsync();
            Response.StatusCode = (int)resp.StatusCode;
            return Content(content, "application/json");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar dados do perfil do cliente.");
            return StatusCode(500, new { sucesso = false, mensagem = "Erro interno ao comunicar com o servidor." });
        }
    }

    [HttpGet]
    public async Task<IActionResult> Historico()
    {
        var token = ObterTokenJwt();
        if (string.IsNullOrEmpty(token))
        {
            return StatusCode(401, new { sucesso = false, mensagem = "Sessão expirada ou não autenticada." });
        }

        try
        {
            var client = CriarApiClientComBearer(token);
            var resp = await client.GetAsync("/api/Agendamentos/meus");
            var content = await resp.Content.ReadAsStringAsync();
            Response.StatusCode = (int)resp.StatusCode;
            return Content(content, "application/json");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter histórico de agendamentos do cliente.");
            return StatusCode(500, new { sucesso = false, mensagem = "Erro interno ao comunicar com o servidor." });
        }
    }

    private string? ObterTokenJwt()
    {
        return User.FindFirst("jwt_token")?.Value;
    }

    private HttpClient CriarApiClientComBearer(string token)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }
}
