using System.Text.Json;
using Melobarbershop.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Melobarbershop.UI.Controllers
{
    public class ServicosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ServicosController> _logger;

        public ServicosController(IHttpClientFactory httpClientFactory, ILogger<ServicosController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var servicos = new List<ServicoDto>();

            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                var response = await client.GetAsync("/api/Servicos/todos");

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var apiResult = JsonSerializer.Deserialize<ApiResposta<List<ServicoDto>>>(responseBody, jsonOptions);

                    if (apiResult != null && apiResult.Sucesso && apiResult.Dados != null)
                    {
                        // Exibir serviços ativos e que estão habilitados para exibição no site
                        servicos = apiResult.Dados
                            .Where(s => s.Ativo && s.ExibirNoSite)
                            .ToList();
                    }
                    else
                    {
                        ViewBag.ErrorMessage = apiResult?.Mensagem ?? "Não foi possível carregar os serviços.";
                    }
                }
                else
                {
                    _logger.LogWarning("Falha ao consultar serviços na API. Status: {StatusCode}", response.StatusCode);
                    ViewBag.ErrorMessage = "O serviço está temporariamente indisponível. Tente novamente mais tarde.";
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Erro de conexão ao acessar GET /api/Servicos/todos.");
                ViewBag.ErrorMessage = "Não foi possível conectar ao servidor de serviços. Verifique a conexão.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao carregar serviços.");
                ViewBag.ErrorMessage = "Ocorreu um erro ao carregar os serviços disponíveis.";
            }

            return View(servicos);
        }

        /// <summary>
        /// Retorna os serviços ativos como JSON para consumo client-side (ex: Agendamento).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Dados()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                var response = await client.GetAsync("/api/Servicos/todos");

                if (!response.IsSuccessStatusCode)
                    return Json(new { sucesso = false, dados = Array.Empty<object>() });

                var responseBody = await response.Content.ReadAsStringAsync();
                var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var apiResult = JsonSerializer.Deserialize<ApiResposta<List<ServicoDto>>>(responseBody, jsonOptions);

                var servicos = apiResult?.Dados?
                    .Where(s => s.Ativo && s.ExibirNoSite)
                    .Select(s => new {
                        id = s.Id,
                        nome = s.Nome,
                        descricao = s.Descricao,
                        preco = s.Preco,
                        duracaoMinutos = s.DuracaoMinutos
                    })
                    .ToList() ?? new List<object>() as dynamic;

                return Json(new { sucesso = true, dados = servicos });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao retornar serviços como JSON.");
                return Json(new { sucesso = false, dados = Array.Empty<object>() });
            }
        }
    }
}

