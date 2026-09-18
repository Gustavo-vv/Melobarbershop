// ============================================================================
// Arquivo: HomeController.cs
// Camada: Melobarbershop.UI (Controllers)
// Objetivo: Controlador da página inicial do portal Web da barbearia.
// Papel na Arquitetura:
//   - Consome a API REST interna via IHttpClientFactory para carregar serviços em destaque.
//   - Fornece fallback resiliente caso a API esteja temporariamente indisponível.
//   - Transforma DTOs em ViewModels específicos para renderização na View Razor.
// ============================================================================

using System.Diagnostics;
using System.Text.Json;
using Melobarbershop.Application.DTOs;
using Melobarbershop.UI.Models;
using Melobarbershop.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Melobarbershop.UI.Controllers;

/// <summary>
/// Controlador principal da página inicial da barbearia no portal público.
/// </summary>
public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<HomeController> _logger;

    /// <summary>
    /// Construtor com injeção do HttpClientFactory e do serviço de log.
    /// </summary>
    public HomeController(IHttpClientFactory httpClientFactory, ILogger<HomeController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <summary>
    /// Renderiza a página inicial exibindo os serviços em destaque e cards de apresentação.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var viewModel = new HomeViewModel();

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
                    var servicosVisiveis = apiResult.Dados
                        .Where(s => s.Ativo && s.ExibirNoSite)
                        .Take(5) // Mantém a Home com uma quantidade fixa de cards em destaque
                        .Select((s, index) => MapearParaViewModel(s, index == 0))
                        .ToList();

                    viewModel.Servicos = servicosVisiveis;
                }
                else
                {
                    viewModel.MensagemErro = apiResult?.Mensagem ?? "Não foi possível carregar os serviços.";
                }
            }
            else
            {
                _logger.LogWarning("Falha ao consultar serviços na API para Home. Status: {StatusCode}", response.StatusCode);
                viewModel.MensagemErro = "Serviços temporariamente indisponíveis.";
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro de conexão ao acessar GET /api/Servicos/todos na Home.");
            viewModel.MensagemErro = "Não foi possível conectar ao servidor de serviços. Verifique a conexão.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao carregar serviços na Home.");
            viewModel.MensagemErro = "Ocorreu um erro ao carregar os serviços disponíveis.";
        }

        // Fallback defensivo para manter a interface funcional caso a API esteja offline
        if (!viewModel.Servicos.Any())
        {
            PreencherServicosFallback(viewModel);
        }

        return View(viewModel);
    }

    /// <summary>
    /// Converte o DTO retornado pela API em ViewModel de apresentação com ícone SVG apropriado.
    /// </summary>
    private static HomeServicoItemViewModel MapearParaViewModel(ServicoDto dto, bool destaque)
    {
        var icone = "tesoura.svg";
        var nomeUpper = dto.Nome.ToUpperInvariant();

        if (nomeUpper.Contains("BARBA") && nomeUpper.Contains("CORTE"))
            icone = "corte_barba.svg";
        else if (nomeUpper.Contains("BARBA"))
            icone = "Bigode.svg";
        else if (nomeUpper.Contains("SOBRANCELHA"))
            icone = "sobrancelha.svg";
        else if (nomeUpper.Contains("NAVALH"))
            icone = "star.svg";

        return new HomeServicoItemViewModel
        {
            Id = dto.Id,
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Preco = dto.Preco,
            DuracaoMinutos = dto.DuracaoMinutos,
            Destaque = destaque,
            IconeSvg = icone
        };
    }

    /// <summary>
    /// Preenche dados padrão no ViewModel caso ocorra falha de rede ao consultar a API.
    /// </summary>
    private static void PreencherServicosFallback(HomeViewModel viewModel)
    {
        viewModel.Servicos = new List<HomeServicoItemViewModel>
        {
            new() { Id = 1, Nome = "Corte Masculino Tradicional", Descricao = "Corte de cabelo clássico feito com tesoura e máquina.", Preco = 45.00m, DuracaoMinutos = 40, Destaque = true, IconeSvg = "tesoura.svg" },
            new() { Id = 2, Nome = "Barba Completa", Descricao = "Aparo, desenho e finalização da barba com toalha quente.", Preco = 40.00m, DuracaoMinutos = 30, Destaque = false, IconeSvg = "Bigode.svg" },
            new() { Id = 3, Nome = "Combo Corte + Barba", Descricao = "Corte de cabelo à escolha combinado com barba completa.", Preco = 75.00m, DuracaoMinutos = 75, Destaque = false, IconeSvg = "corte_barba.svg" },
            new() { Id = 4, Nome = "Sobrancelha na Navalha", Descricao = "Design de sobrancelha masculina feito na navalha.", Preco = 20.00m, DuracaoMinutos = 15, Destaque = false, IconeSvg = "sobrancelha.svg" },
            new() { Id = 5, Nome = "Corte Navalhado", Descricao = "Acabamento 100% na navalha para um contorno preciso.", Preco = 60.00m, DuracaoMinutos = 45, Destaque = false, IconeSvg = "star.svg" }
        };
    }

    /// <summary>
    /// Página de termos e políticas de privacidade.
    /// </summary>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Página de tratamento de erros não tratados do portal.
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}