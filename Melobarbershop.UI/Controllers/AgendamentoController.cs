using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Domain.Enums;
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
            viewModel.UsuarioLogado = User.Identity?.IsAuthenticated == true;
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
        /// Retorna os horários disponíveis para um barbeiro/dia/serviço(s),
        /// consumido via AJAX pela tela de Agendamento quando o cliente troca o dia ou o barbeiro.
        /// Retorna objetos { label, valor } com o DateTime exato calculado pela API.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> HorariosDisponiveis(
            [FromQuery] string barbeiroId,
            [FromQuery] DateTime data,
            [FromQuery] string? servicoIds)
        {
            if (string.IsNullOrWhiteSpace(barbeiroId))
                return Json(new { sucesso = false, mensagem = "Barbeiro não informado.", dados = Array.Empty<object>() });

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
                return Json(new { sucesso = false, mensagem = "Não foi possível carregar os horários disponíveis.", dados = Array.Empty<object>() });
            }
        }

        /// <summary>
        /// Cria o agendamento no banco via POST /api/Agendamentos.
        /// O ClienteId é extraído com segurança da claim 'jwt_token' do usuário autenticado.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarAgendamentoRequisicao requisicao)
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    sucesso = false,
                    mensagem = "Você precisa estar autenticado para realizar um agendamento."
                });
            }

            if (requisicao == null || string.IsNullOrWhiteSpace(requisicao.BarbeiroId) || requisicao.DataHoraInicio == default)
            {
                return BadRequest(new { sucesso = false, mensagem = "Dados incompletos para criação do agendamento." });
            }

            if (requisicao.ServicoIds == null || !requisicao.ServicoIds.Any())
            {
                return BadRequest(new { sucesso = false, mensagem = "Selecione pelo menos um serviço." });
            }

            // Extrai a claim jwt_token
            var jwtToken = User.FindFirst("jwt_token")?.Value;
            if (string.IsNullOrWhiteSpace(jwtToken))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    sucesso = false,
                    mensagem = "Sessão inválida. Por favor, faça login novamente."
                });
            }

            string? clienteId = null;
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwtToken);
                clienteId = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "nameid" || c.Type == "sub")?.Value;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Falha ao decodificar token JWT para extrair ClienteId.");
            }

            if (string.IsNullOrWhiteSpace(clienteId))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    sucesso = false,
                    mensagem = "Não foi possível identificar o cliente autenticado."
                });
            }

            var dto = new CriarAgendamentoDto
            {
                ClienteId = clienteId,
                BarbeiroId = requisicao.BarbeiroId,
                DataHoraInicio = requisicao.DataHoraInicio,
                ServicoIds = requisicao.ServicoIds.ToList(),
                Origem = OrigemAgendamento.Site,
                Observacoes = requisicao.Observacoes
            };

            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(dto),
                    Encoding.UTF8,
                    "application/json");

                var response = await client.PostAsync("/api/Agendamentos", jsonContent);
                var responseBody = await response.Content.ReadAsStringAsync();
                var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var apiResult = JsonSerializer.Deserialize<ApiResposta<AgendamentoDto>>(responseBody, jsonOptions);

                if (!response.IsSuccessStatusCode || apiResult == null || !apiResult.Sucesso)
                {
                    var msg = apiResult?.Mensagem ?? "Não foi possível criar o agendamento.";
                    return StatusCode((int)response.StatusCode, new { sucesso = false, mensagem = msg });
                }

                return Ok(new
                {
                    sucesso = true,
                    mensagem = "Agendamento realizado com sucesso!",
                    dados = apiResult.Dados
                });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Erro de conexão ao criar agendamento na API.");
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new
                {
                    sucesso = false,
                    mensagem = "Não foi possível conectar ao servidor. Tente novamente mais tarde."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar agendamento.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    sucesso = false,
                    mensagem = "Ocorreu um erro interno ao processar seu agendamento."
                });
            }
        }

        /// <summary>
        /// Chama GET /api/Agendamentos/horarios-do-dia e converte o resultado
        /// para objetos HorarioDisponivelViewModel (Label "HH:mm", Valor ISO exato e Disponivel).
        /// </summary>
        private async Task<List<HorarioDisponivelViewModel>> ObterHorariosDisponiveisAsync(
            HttpClient client, string barbeiroId, DateTime data, IEnumerable<int> servicoIds)
        {
            var query = new List<string>
            {
                $"barbeiroId={Uri.EscapeDataString(barbeiroId)}",
                $"data={data:yyyy-MM-dd}"
            };
            query.AddRange(servicoIds.Select(id => $"servicoIds={id}"));

            var url = $"/api/Agendamentos/horarios-do-dia?{string.Join('&', query)}";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Falha ao buscar horários do dia. Status: {StatusCode}", response.StatusCode);
                return new List<HorarioDisponivelViewModel>();
            }

            var body = await response.Content.ReadAsStringAsync();
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var apiResult = JsonSerializer.Deserialize<ApiResposta<List<HorarioSlotDto>>>(body, jsonOptions);

            if (apiResult == null || !apiResult.Sucesso || apiResult.Dados == null)
                return new List<HorarioDisponivelViewModel>();

            return apiResult.Dados
                .OrderBy(d => d.Horario)
                .Select(d => new HorarioDisponivelViewModel
                {
                    Label = d.Horario.ToString("HH:mm"),
                    Valor = d.Horario.ToString("o"), // ISO 8601 exato
                    Disponivel = d.Disponivel
                })
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

    public class CriarAgendamentoRequisicao
    {
        public string BarbeiroId { get; set; } = string.Empty;
        public DateTime DataHoraInicio { get; set; }
        public List<int> ServicoIds { get; set; } = new();
        public string? Observacoes { get; set; }
    }
}
