// ============================================================================
// Arquivo: ServicosController.cs
// Camada: Melobarbershop.UI (Controllers)
// Objetivo: Controlador para apresentação visual e consumo assíncrono dos serviços no portal Web.
// Papel na Arquitetura:
//   - Faz a ponte entre a API REST interna e os navegadores dos clientes.
//   - Converte ServicoDto para ServicoViewModel para renderização segura e tipada no Razor.
//   - Fornece endpoint JSON para scripts dinâmicos de agendamento na interface do usuário.
// ============================================================================

using System.Text.Json;
using Melobarbershop.Application.DTOs;
using Melobarbershop.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Melobarbershop.UI.Controllers;

/// <summary>
/// Controlador responsável pela listagem e dados de serviços no portal público.
/// </summary>
public class ServicosController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ServicosController> _logger;

    /// <summary>
    /// Construtor com injeção do gerenciador de clientes HTTP e do serviço de log.
    /// </summary>
    public ServicosController(IHttpClientFactory httpClientFactory, ILogger<ServicosController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <summary>
    /// Renderiza a página de catálogo de serviços, dividindo entre serviços populares e os demais.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var viewModel = new ListaServicosViewModel();

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
                    // Exibir serviços ativos e que estão habilitados para exibição no site,
                    // já convertidos de ServicoDto (Application) para ServicoViewModel (UI)
                    var servicosVisiveis = apiResult.Dados
                        .Where(s => s.Ativo && s.ExibirNoSite)
                        .Select(MapearParaViewModel)
                        .ToList();

                    viewModel.Populares = servicosVisiveis.Take(4).ToList();
                    viewModel.Outros = servicosVisiveis.Skip(4).ToList();
                }
                else
                {
                    viewModel.MensagemErro = apiResult?.Mensagem ?? "Não foi possível carregar os serviços.";
                }
            }
            else
            {
                _logger.LogWarning("Falha ao consultar serviços na API. Status: {StatusCode}", response.StatusCode);
                viewModel.MensagemErro = "O serviço está temporariamente indisponível. Tente novamente mais tarde.";
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro de conexão ao acessar GET /api/Servicos/todos.");
            viewModel.MensagemErro = "Não foi possível conectar ao servidor de serviços. Verifique a conexão.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao carregar serviços.");
            viewModel.MensagemErro = "Ocorreu um erro ao carregar os serviços disponíveis.";
        }

        return View(viewModel);
    }

    /// <summary>
    /// Converte o DTO recebido da API para o ViewModel usado pela camada de apresentação (UI).
    /// </summary>
    private static ServicoViewModel MapearParaViewModel(ServicoDto dto)
    {
        return new ServicoViewModel
        {
            Id = dto.Id,
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            DuracaoMinutos = dto.DuracaoMinutos,
            Preco = dto.Preco
        };
    }

    /// <summary>
    /// Retorna os serviços ativos como JSON para consumo client-side (ex: formulário dinâmico de agendamento).
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
