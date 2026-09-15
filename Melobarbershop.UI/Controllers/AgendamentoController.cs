using System.Text.Json;
using Melobarbershop.Application.DTOs;
using Melobarbershop.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Melobarbershop.UI.Controllers
{
    public class AgendamentoController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AgendamentoController> _logger;

        public AgendamentoController(IHttpClientFactory httpClientFactory, ILogger<AgendamentoController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string? serviceId, [FromQuery] string? serviceName)
        {
            var viewModel = new AgendamentoViewModel();
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");

                // 1. Busca serviços ativos
                var servicosResponse = await client.GetAsync("/api/Servicos/todos");
                if (servicosResponse.IsSuccessStatusCode)
                {
                    var servicosBody = await servicosResponse.Content.ReadAsStringAsync();
                    var servicosResult = JsonSerializer.Deserialize<ApiResposta<List<ServicoDto>>>(servicosBody, jsonOptions);

                    if (servicosResult != null && servicosResult.Sucesso && servicosResult.Dados != null)
                    {
                        var ativos = servicosResult.Dados
                            .Where(s => s.Ativo && s.ExibirNoSite)
                            .Select(MapearServicoParaViewModel)
                            .ToList();

                        viewModel.ServicosDisponiveis = ativos;

                        // Localiza o serviço selecionado se informado via query string
                        if (!string.IsNullOrWhiteSpace(serviceId) && int.TryParse(serviceId, out int idNum))
                        {
                            viewModel.ServicoSelecionado = ativos.FirstOrDefault(s => s.Id == idNum);
                        }

                        if (viewModel.ServicoSelecionado == null && !string.IsNullOrWhiteSpace(serviceName))
                        {
                            viewModel.ServicoSelecionado = ativos.FirstOrDefault(s =>
                                string.Equals(s.Nome, serviceName, StringComparison.OrdinalIgnoreCase) ||
                                s.Nome.Contains(serviceName, StringComparison.OrdinalIgnoreCase));
                        }

                        // Se ainda não tiver selecionado nenhum, assume o primeiro disponível como padrão
                        viewModel.ServicoSelecionado ??= ativos.FirstOrDefault();
                    }
                    else
                    {
                        viewModel.MensagemErro = servicosResult?.Mensagem ?? "Não foi possível carregar os serviços.";
                    }
                }
                else
                {
                    _logger.LogWarning("Falha ao buscar serviços para agendamento. Status: {StatusCode}", servicosResponse.StatusCode);
                    viewModel.MensagemErro = "Serviços temporariamente indisponíveis.";
                }

                // 2. Busca barbeiros/profissionais cadastrados
                try
                {
                    var usuariosResponse = await client.GetAsync("/api/Usuarios");
                    if (usuariosResponse.IsSuccessStatusCode)
                    {
                        var usuariosBody = await usuariosResponse.Content.ReadAsStringAsync();
                        var usuariosResult = JsonSerializer.Deserialize<ApiResposta<List<UsuarioDto>>>(usuariosBody, jsonOptions);

                        if (usuariosResult != null && usuariosResult.Sucesso && usuariosResult.Dados != null)
                        {
                            var barbeiros = usuariosResult.Dados
                                .Where(u => u.Ativo && u.Roles.Any(r => string.Equals(r, "Barbeiro", StringComparison.OrdinalIgnoreCase) ||
                                                                        string.Equals(r, "Admin", StringComparison.OrdinalIgnoreCase)))
                                .Select(MapearBarbeiroParaViewModel)
                                .ToList();

                            viewModel.BarbeirosDisponiveis = barbeiros;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Não foi possível carregar a lista de barbeiros da API.");
                }

                // Fallback de barbeiro caso a API não retorne nenhum barbeiro específico cadastrado
                if (!viewModel.BarbeirosDisponiveis.Any())
                {
                    viewModel.BarbeirosDisponiveis.Add(new BarbeiroItemViewModel
                    {
                        Id = "1",
                        Nome = "Guilherme"
                    });
                }

                // 3. Busca os horários disponíveis para o barbeiro/serviço padrão (hoje),
                // apenas para a renderização inicial da página. Trocas de dia/barbeiro
                // no cliente são resolvidas via AJAX em GET /Agendamento/HorariosDisponiveis.
                var barbeiroPadrao = viewModel.BarbeirosDisponiveis.FirstOrDefault();
                if (barbeiroPadrao != null)
                {
                    var servicoIds = viewModel.ServicoSelecionado != null
                        ? new List<int> { viewModel.ServicoSelecionado.Id }
                        : new List<int>();

                    viewModel.HorariosDisponiveis = await ObterHorariosDisponiveisAsync(
                        client, barbeiroPadrao.Id, DateTime.Today, servicoIds);
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Erro de conexão ao acessar a API para a tela de Agendamento.");
                viewModel.MensagemErro = "Não foi possível conectar ao servidor. Verifique sua conexão.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao carregar dados de Agendamento.");
                viewModel.MensagemErro = "Ocorreu um erro ao carregar os dados para agendamento.";
            }

            return View(viewModel);
        }

        /// <summary>
        /// Retorna os horários disponíveis (formato "HH:mm") para um barbeiro/dia/serviço(s),
        /// consumido via AJAX pela tela de Agendamento quando o cliente troca o dia ou o barbeiro.
        /// Espelha a regra de negócio de GET /api/Agendamentos/horarios-disponiveis, que já
        /// desconsidera horários com conflito de agenda (ExisteConflitoDeHorarioAsync) ou bloqueio.
        /// </summary>
        /// <param name="servicoIds">IDs dos serviços selecionados, separados por vírgula (ex: "1,3").</param>
        [HttpGet]
        public async Task<IActionResult> HorariosDisponiveis(
            [FromQuery] string barbeiroId,
            [FromQuery] DateTime data,
            [FromQuery] string? servicoIds)
        {
            if (string.IsNullOrWhiteSpace(barbeiroId))
                return Json(new { sucesso = false, mensagem = "Barbeiro não informado.", dados = Array.Empty<string>() });

            var ids = (servicoIds ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(s => int.TryParse(s, out var n) ? n : (int?)null)
                .Where(n => n.HasValue)
                .Select(n => n!.Value)
                .ToList();

            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                var horarios = await ObterHorariosDisponiveisAsync(client, barbeiroId, data.Date, ids);
                return Json(new { sucesso = true, dados = horarios });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar horários disponíveis para o barbeiro {BarbeiroId} em {Data}.", barbeiroId, data);
                return Json(new { sucesso = false, mensagem = "Não foi possível carregar os horários disponíveis.", dados = Array.Empty<string>() });
            }
        }

        /// <summary>
        /// Chama GET /api/Agendamentos/horarios-disponiveis e converte o resultado
        /// (lista de DateTime em UTC) para strings "HH:mm" prontas para exibição.
        /// </summary>
        private async Task<List<string>> ObterHorariosDisponiveisAsync(
            HttpClient client, string barbeiroId, DateTime data, IEnumerable<int> servicoIds)
        {
            var query = new List<string>
            {
                $"barbeiroId={Uri.EscapeDataString(barbeiroId)}",
                $"data={data:yyyy-MM-dd}"
            };
            query.AddRange(servicoIds.Select(id => $"servicoIds={id}"));

            var url = $"/api/Agendamentos/horarios-disponiveis?{string.Join('&', query)}";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Falha ao buscar horários disponíveis. Status: {StatusCode}", response.StatusCode);
                return new List<string>();
            }

            var body = await response.Content.ReadAsStringAsync();
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var apiResult = JsonSerializer.Deserialize<ApiResposta<List<DateTime>>>(body, jsonOptions);

            if (apiResult == null || !apiResult.Sucesso || apiResult.Dados == null)
                return new List<string>();

            return apiResult.Dados
                .OrderBy(d => d)
                .Select(d => d.ToString("HH:mm"))
                .ToList();
        }

        private static ServicoItemViewModel MapearServicoParaViewModel(ServicoDto dto)
        {
            return new ServicoItemViewModel
            {
                Id = dto.Id,
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                DuracaoMinutos = dto.DuracaoMinutos,
                Preco = dto.Preco
            };
        }

        private static BarbeiroItemViewModel MapearBarbeiroParaViewModel(UsuarioDto dto)
        {
            return new BarbeiroItemViewModel
            {
                Id = dto.Id,
                Nome = dto.Nome,
                FotoUrl = dto.FotoUrl
            };
        }
    }
}
