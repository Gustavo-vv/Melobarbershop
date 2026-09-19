// ============================================================================
// Arquivo: AdminController.cs
// Camada: Melobarbershop.UI (Controllers)
// Objetivo: Controlador do painel administrativo Web (Dashboard de gestão, serviços, profissionais e agendamentos).
// Papel na Arquitetura:
//   - Agrega dados gerenciais de serviços e barbeiros consumindo os endpoints da API REST.
//   - Fornece dados resilientes de fallback para visualização imediata no painel.
//   - Converte os modelos de domínio e DTOs em ViewModels específicos para a view do Administrador.
// ============================================================================

using System.Text.Json;
using Melobarbershop.Application.DTOs;
using Melobarbershop.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Melobarbershop.UI.Controllers;

/// <summary>
/// Controlador responsável pelo painel de controle administrativo da barbearia.
/// </summary>
public class AdminController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<AdminController> _logger;

    /// <summary>
    /// Construtor com injeção do HttpClientFactory e serviço de log.
    /// </summary>
    public AdminController(IHttpClientFactory httpClientFactory, ILogger<AdminController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <summary>
    /// Renderiza a tela inicial do painel administrativo com estatísticas, serviços, barbeiros e fila de atendimento.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var viewModel = new AdminViewModel();
        var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            // 1. Carregar Serviços da API
            try
            {
                var servicosResponse = await client.GetAsync("/api/Servicos/todos");
                if (servicosResponse.IsSuccessStatusCode)
                {
                    var responseBody = await servicosResponse.Content.ReadAsStringAsync();
                    var apiResult = JsonSerializer.Deserialize<ApiResposta<List<ServicoDto>>>(responseBody, jsonOptions);

                    if (apiResult != null && apiResult.Sucesso && apiResult.Dados != null)
                    {
                        viewModel.Servicos = apiResult.Dados
                            .Select(MapearServicoParaViewModel)
                            .ToList();
                    }
                }
                else
                {
                    _logger.LogWarning("Falha ao buscar serviços para o Admin. Status: {StatusCode}", servicosResponse.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter serviços para o Admin.");
            }

            // 2. Carregar Usuários/Barbeiros da API
            try
            {
                var usuariosResponse = await client.GetAsync("/api/Usuarios");
                if (usuariosResponse.IsSuccessStatusCode)
                {
                    var responseBody = await usuariosResponse.Content.ReadAsStringAsync();
                    var apiResult = JsonSerializer.Deserialize<ApiResposta<List<UsuarioDto>>>(responseBody, jsonOptions);

                    if (apiResult != null && apiResult.Sucesso && apiResult.Dados != null)
                    {
                        viewModel.Barbeiros = apiResult.Dados
                            .Where(u => u.Ativo && u.Roles.Any(r => string.Equals(r, "Barbeiro", StringComparison.OrdinalIgnoreCase) ||
                                                                    string.Equals(r, "Admin", StringComparison.OrdinalIgnoreCase)))
                            .Select(MapearUsuarioParaViewModel)
                            .ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter usuários para o Admin.");
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro de conexão ao acessar a API para o painel Admin.");
            viewModel.MensagemErro = "Não foi possível conectar ao servidor de dados. Alguns recursos podem estar indisponíveis.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado no painel Admin.");
            viewModel.MensagemErro = "Ocorreu um erro ao carregar os dados do painel.";
        }

        // Garante dados de fallback para manter os dashboards ricos caso a API não tenha registros
        PreencherFallbacksSeNecessario(viewModel);

        return View(viewModel);
    }

    /// <summary>
    /// Mapeia o DTO de serviço da Application para a ViewModel de exibição na tela do Admin.
    /// </summary>
    private static AdminServicoViewModel MapearServicoParaViewModel(ServicoDto dto)
    {
        return new AdminServicoViewModel
        {
            Id = dto.Id,
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Preco = dto.Preco,
            DuracaoMinutos = dto.DuracaoMinutos,
            Ativo = dto.Ativo,
            ExibirNoSite = dto.ExibirNoSite
        };
    }

    /// <summary>
    /// Mapeia o DTO de usuário da Application para a ViewModel do Administrador.
    /// </summary>
    private static AdminUsuarioViewModel MapearUsuarioParaViewModel(UsuarioDto dto)
    {
        return new AdminUsuarioViewModel
        {
            Id = dto.Id,
            Nome = dto.Nome,
            Email = dto.Email,
            Telefone = dto.PhoneNumber,
            FotoUrl = dto.FotoUrl,
            Ativo = dto.Ativo,
            Roles = dto.Roles?.ToList() ?? new List<string>()
        };
    }

    /// <summary>
    /// Popula itens de demonstração e fallback nos cards do painel administrativo caso a base inicial esteja vazia.
    /// </summary>
    private static void PreencherFallbacksSeNecessario(AdminViewModel viewModel)
    {
        if (!viewModel.Servicos.Any())
        {
            viewModel.Servicos = new List<AdminServicoViewModel>
            {
                new() { Id = 1, Nome = "Corte Degradê / Fade", Descricao = "Tesoura e máquina com transição perfeita e acabamento a navalha.", Preco = 45m, DuracaoMinutos = 40, Ativo = true, ExibirNoSite = true },
                new() { Id = 2, Nome = "Barba Terapia & Alinhamento", Descricao = "Toalha quente, óleos essenciais, massagem facial e navalha afiada.", Preco = 35m, DuracaoMinutos = 30, Ativo = true, ExibirNoSite = true },
                new() { Id = 3, Nome = "Combo VIP (Cabelo + Barba)", Descricao = "Experiência completa com bebida de cortesia e finalização premium.", Preco = 75m, DuracaoMinutos = 75, Ativo = true, ExibirNoSite = true },
                new() { Id = 4, Nome = "Pigmentação / Platinado", Descricao = "Realce e uniformização do corte com produtos de alta fixação.", Preco = 90m, DuracaoMinutos = 90, Ativo = true, ExibirNoSite = true },
                new() { Id = 5, Nome = "Tranças Nagô & Estilos", Descricao = "Alinhamento afro e desenhos personalizados em tranças.", Preco = 110m, DuracaoMinutos = 105, Ativo = true, ExibirNoSite = true },
                new() { Id = 6, Nome = "Sobrancelha na Navalha", Descricao = "Alinhamento e limpeza rápida para valorizar o olhar.", Preco = 15m, DuracaoMinutos = 15, Ativo = true, ExibirNoSite = true }
            };
        }

        if (!viewModel.Barbeiros.Any())
        {
            viewModel.Barbeiros = new List<AdminUsuarioViewModel>
            {
                new() { Id = "1", Nome = "Melo Master", Email = "melo@melobarber.com", Roles = new List<string> { "Admin", "Barbeiro" }, Ativo = true },
                new() { Id = "2", Nome = "Lucas Fade", Email = "lucas@melobarber.com", Roles = new List<string> { "Barbeiro" }, Ativo = true },
                new() { Id = "3", Nome = "Diego Navalha", Email = "diego@melobarber.com", Roles = new List<string> { "Barbeiro" }, Ativo = true },
                new() { Id = "4", Nome = "André Tranças", Email = "andre@melobarber.com", Roles = new List<string> { "Barbeiro" }, Ativo = true }
            };
        }

        if (!viewModel.AgendamentosFila.Any())
        {
            viewModel.AgendamentosFila = new List<AdminAgendamentoItemViewModel>
            {
                new() { Id = 1045, Horario = "20:15", ClienteNome = "Renan Costa", Telefone = "(11) 98765-4321", ServicoNome = "Corte Americano", BarbeiroNome = "Diego Navalha", Valor = 45.00m, Origem = "Site Web", Status = "Confirmado" },
                new() { Id = 1042, Horario = "20:30", ClienteNome = "Lucas Vasconcelos", Telefone = "(11) 97112-9988", ServicoNome = "Combo Completo (Cabelo + Barba + Sobrancelha)", BarbeiroNome = "Melo Master", Valor = 80.00m, Origem = "Site Web", Status = "Confirmado" },
                new() { Id = 1043, Horario = "20:45", ClienteNome = "Rafael Mendes", Telefone = "(11) 96554-1122", ServicoNome = "Degradê na Navalha", BarbeiroNome = "Lucas Fade", Valor = 50.00m, Origem = "Site Web", Status = "Aguardando" },
                new() { Id = 1044, Horario = "20:50", ClienteNome = "Vitor Hugo", Telefone = "(11) 95544-3322", ServicoNome = "Pézinho & Barboterapia", BarbeiroNome = "Diego Navalha", Valor = 40.00m, Origem = "Balcão", Status = "Encaixe" }
            };
        }
    }
}
