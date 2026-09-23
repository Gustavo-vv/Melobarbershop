using System.Text.Json;
using Melobarbershop.Application.DTOs;
using Melobarbershop.Domain.Enums;
using Melobarbershop.UI.Areas.Admin.Models;

namespace Melobarbershop.UI.Areas.Admin.Services;

public class PainelDadosService : IPainelDadosService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<PainelDadosService> _logger;
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public PainelDadosService(IHttpClientFactory httpClientFactory, ILogger<PainelDadosService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<PainelDadosPayloadViewModel> ObterDadosPainelAsync(string periodo, string profissionalId, string origem)
    {
        var payload = new PainelDadosPayloadViewModel();
        var client = _httpClientFactory.CreateClient("ApiClient");

        try
        {
            var agora = DateTime.Now;
            DateTime dataInicioAtual;
            DateTime dataFimAtual;
            DateTime dataInicioAnterior;
            DateTime dataFimAnterior;

            switch (periodo?.ToLowerInvariant())
            {
                case "semana":
                    // Últimos 7 dias
                    dataInicioAtual = agora.Date.AddDays(-6);
                    dataFimAtual = agora.Date.AddDays(1).AddTicks(-1);
                    dataInicioAnterior = dataInicioAtual.AddDays(-7);
                    dataFimAnterior = dataInicioAtual.AddTicks(-1);
                    break;
                case "mes":
                    // Primeiro ao último momento do mês atual
                    dataInicioAtual = new DateTime(agora.Year, agora.Month, 1);
                    dataFimAtual = dataInicioAtual.AddMonths(1).AddTicks(-1);
                    var mesAnterior = dataInicioAtual.AddMonths(-1);
                    dataInicioAnterior = mesAnterior;
                    dataFimAnterior = dataInicioAtual.AddTicks(-1);
                    break;
                case "hoje":
                default:
                    dataInicioAtual = agora.Date;
                    dataFimAtual = agora.Date.AddDays(1).AddTicks(-1);
                    dataInicioAnterior = dataInicioAtual.AddDays(-1);
                    dataFimAnterior = dataInicioAtual.AddTicks(-1);
                    break;
            }

            // 1. Obter Usuários (Barbeiros e Profissionais)
            var listaProfissionais = new List<UsuarioDto>();
            try
            {
                var respUsuarios = await client.GetAsync("/api/Usuarios");
                if (respUsuarios.IsSuccessStatusCode)
                {
                    var content = await respUsuarios.Content.ReadAsStringAsync();
                    var apiResult = JsonSerializer.Deserialize<ApiResposta<List<UsuarioDto>>>(content, _jsonOptions);
                    if (apiResult?.Sucesso == true && apiResult.Dados != null)
                    {
                        listaProfissionais = apiResult.Dados
                            .Where(u => u.Ativo && u.Roles.Any(r =>
                                string.Equals(r, "Barbeiro", StringComparison.OrdinalIgnoreCase) ||
                                string.Equals(r, "Admin", StringComparison.OrdinalIgnoreCase)))
                            .ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar usuários na API.");
            }

            payload.Profissionais = listaProfissionais
                .Select(p => new PainelProfissionalOpcaoViewModel { Id = p.Id, Nome = p.Nome })
                .ToList();

            // 2. Obter Agendamentos do Período Atual
            var agendamentosAtuais = new List<AgendamentoDto>();
            try
            {
                var urlAtual = $"/api/Agendamentos/periodo?inicio={Uri.EscapeDataString(dataInicioAtual.ToString("yyyy-MM-ddTHH:mm:ss"))}&fim={Uri.EscapeDataString(dataFimAtual.ToString("yyyy-MM-ddTHH:mm:ss"))}";
                var respAgenda = await client.GetAsync(urlAtual);
                if (respAgenda.IsSuccessStatusCode)
                {
                    var content = await respAgenda.Content.ReadAsStringAsync();
                    var apiResult = JsonSerializer.Deserialize<ApiResposta<List<AgendamentoDto>>>(content, _jsonOptions);
                    if (apiResult?.Sucesso == true && apiResult.Dados != null)
                    {
                        agendamentosAtuais = apiResult.Dados;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar agendamentos atuais na API.");
            }

            // 3. Obter Agendamentos do Período Anterior (para delta de KPIs)
            var agendamentosAnteriores = new List<AgendamentoDto>();
            try
            {
                var urlAnterior = $"/api/Agendamentos/periodo?inicio={Uri.EscapeDataString(dataInicioAnterior.ToString("yyyy-MM-ddTHH:mm:ss"))}&fim={Uri.EscapeDataString(dataFimAnterior.ToString("yyyy-MM-ddTHH:mm:ss"))}";
                var respAgendaAnt = await client.GetAsync(urlAnterior);
                if (respAgendaAnt.IsSuccessStatusCode)
                {
                    var content = await respAgendaAnt.Content.ReadAsStringAsync();
                    var apiResult = JsonSerializer.Deserialize<ApiResposta<List<AgendamentoDto>>>(content, _jsonOptions);
                    if (apiResult?.Sucesso == true && apiResult.Dados != null)
                    {
                        agendamentosAnteriores = apiResult.Dados;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar agendamentos anteriores na API.");
            }

            // 4. Filtrar por Profissional e Origem se solicitado
            IEnumerable<AgendamentoDto> agendamentosFiltrados = agendamentosAtuais;

            if (!string.IsNullOrWhiteSpace(profissionalId) && !string.Equals(profissionalId, "todos", StringComparison.OrdinalIgnoreCase))
            {
                agendamentosFiltrados = agendamentosFiltrados.Where(a =>
                    string.Equals(a.BarbeiroId, profissionalId, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(a.NomeBarbeiro, profissionalId, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(origem) && !string.Equals(origem, "todas", StringComparison.OrdinalIgnoreCase))
            {
                agendamentosFiltrados = agendamentosFiltrados.Where(a =>
                    MapearOrigemString(a.Origem).Equals(origem, StringComparison.OrdinalIgnoreCase));
            }

            var listaFiltrada = agendamentosFiltrados.ToList();

            // 5. Montar Itens da Agenda (Kanban)
            payload.Agenda = listaFiltrada
                .OrderBy(a => a.DataHoraInicio)
                .Select(a => new PainelAgendaItemViewModel
                {
                    Id = a.Id,
                    Cliente = !string.IsNullOrWhiteSpace(a.NomeCliente) ? a.NomeCliente : "Cliente",
                    Hora = a.DataHoraInicio.ToString("HH:mm"),
                    DataHoraIso = a.DataHoraInicio.ToString("yyyy-MM-ddTHH:mm:ss"),
                    Servico = a.Itens.Any() ? string.Join(", ", a.Itens.Select(i => i.NomeServico)) : "Serviço",
                    ProfId = a.BarbeiroId,
                    ProfNome = !string.IsNullOrWhiteSpace(a.NomeBarbeiro) ? a.NomeBarbeiro : "Profissional",
                    Valor = a.ValorTotal,
                    Origem = MapearOrigemFormatada(a.Origem),
                    Status = MapearStatusString(a.Status)
                })
                .ToList();

            // 6. Calcular KPIs do Período
            var concluidosAtual = listaFiltrada.Where(a => a.Status == StatusAgendamento.Concluido).ToList();
            var concluidosAnterior = agendamentosAnteriores.Where(a => a.Status == StatusAgendamento.Concluido).ToList();

            decimal fatAtual = concluidosAtual.Sum(a => a.ValorTotal);
            decimal fatAnterior = concluidosAnterior.Sum(a => a.ValorTotal);
            decimal deltaFat = 0;
            if (fatAnterior > 0)
            {
                deltaFat = Math.Round(((fatAtual - fatAnterior) / fatAnterior) * 100, 0);
            }

            int totalAg = listaFiltrada.Count;
            int totalWeb = listaFiltrada.Count(a => a.Origem == OrigemAgendamento.Site);
            decimal ticket = concluidosAtual.Count > 0 ? Math.Round(fatAtual / concluidosAtual.Count, 0) : 0;

            // Ocupação estimada: baseado em 10 slots diários por barbeiro ativo
            int totalBarbeiros = listaProfissionais.Count > 0 ? listaProfissionais.Count : 1;
            int diasPeriodo = Math.Max(1, (int)(dataFimAtual - dataInicioAtual).TotalDays);
            int capacidadeEstimada = totalBarbeiros * 10 * diasPeriodo;
            int ocupacao = capacidadeEstimada > 0 ? Math.Min(100, (int)Math.Round(((double)concluidosAtual.Count / capacidadeEstimada) * 100)) : 0;

            payload.Kpis = new PainelKpisViewModel
            {
                Faturamento = fatAtual,
                FaturamentoDelta = deltaFat,
                Atendimentos = concluidosAtual.Count,
                AtendimentosMeta = Math.Max(15, totalBarbeiros * 6 * diasPeriodo),
                TotalAgendamentos = totalAg,
                Ocupacao = ocupacao,
                OcupacaoDelta = 0,
                Ticket = ticket,
                AgendamentosWeb = totalWeb
            };

            // 7. Origem dos Agendamentos
            if (totalAg > 0)
            {
                payload.Origem = new List<PainelOrigemItemViewModel>
                {
                    new() { Label = "Site", Quantidade = totalWeb, Pct = (int)Math.Round((double)totalWeb / totalAg * 100), Color = "var(--blue)" },
                    new() { Label = "WhatsApp", Quantidade = listaFiltrada.Count(a => a.Origem == OrigemAgendamento.WhatsApp), Pct = (int)Math.Round((double)listaFiltrada.Count(a => a.Origem == OrigemAgendamento.WhatsApp) / totalAg * 100), Color = "#7aa5ff" },
                    new() { Label = "Aplicativo", Quantidade = listaFiltrada.Count(a => a.Origem == OrigemAgendamento.Aplicativo), Pct = (int)Math.Round((double)listaFiltrada.Count(a => a.Origem == OrigemAgendamento.Aplicativo) / totalAg * 100), Color = "#a8c4ff" },
                    new() { Label = "Balcão", Quantidade = listaFiltrada.Count(a => a.Origem == OrigemAgendamento.PresencialBalcao), Pct = (int)Math.Round((double)listaFiltrada.Count(a => a.Origem == OrigemAgendamento.PresencialBalcao) / totalAg * 100), Color = "var(--panel-3)" }
                };
            }

            // 8. Cancelamentos e Faltas
            int cancelados = listaFiltrada.Count(a => a.Status == StatusAgendamento.Cancelado);
            int faltas = listaFiltrada.Count(a => a.Status == StatusAgendamento.NaoCompareceu);
            payload.Cancel = new PainelCancelamentosViewModel
            {
                TotalCancelados = cancelados,
                TotalNaoCompareceu = faltas,
                TaxaCancelamento = totalAg > 0 ? (int)Math.Round((double)cancelados / totalAg * 100) : 0,
                TaxaNaoComparecimento = totalAg > 0 ? (int)Math.Round((double)faltas / totalAg * 100) : 0
            };

            payload.Sucesso = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao consolidar dados do painel da API.");
            payload.Sucesso = false;
            payload.Mensagem = "Falha ao carregar dados da API: " + ex.Message;
        }

        return payload;
    }

    public async Task<bool> AlterarStatusAgendamentoAsync(int id, string acao, string? motivo)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            string rotaAcao = acao?.ToLowerInvariant() switch
            {
                "confirmar" => $"/api/Agendamentos/{id}/confirmar",
                "iniciar-atendimento" => $"/api/Agendamentos/{id}/iniciar-atendimento",
                "concluir" => $"/api/Agendamentos/{id}/concluir",
                "cancelar" => $"/api/Agendamentos/{id}/cancelar" + (!string.IsNullOrWhiteSpace(motivo) ? $"?motivo={Uri.EscapeDataString(motivo)}" : ""),
                "nao-comparecimento" => $"/api/Agendamentos/{id}/nao-comparecimento",
                _ => string.Empty
            };

            if (string.IsNullOrEmpty(rotaAcao))
                return false;

            var response = await client.PatchAsync(rotaAcao, null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao alterar status do agendamento {Id} para ação {Acao}.", id, acao);
            return false;
        }
    }

    private static string MapearStatusString(StatusAgendamento status) => status switch
    {
        StatusAgendamento.Pendente => "pendente",
        StatusAgendamento.Confirmado => "confirmado",
        StatusAgendamento.EmAtendimento => "em_atendimento",
        StatusAgendamento.Concluido => "concluido",
        StatusAgendamento.Cancelado => "cancelado",
        StatusAgendamento.NaoCompareceu => "cancelado",
        _ => "pendente"
    };

    private static string MapearOrigemFormatada(OrigemAgendamento origem) => origem switch
    {
        OrigemAgendamento.Site => "Site",
        OrigemAgendamento.WhatsApp => "WhatsApp",
        OrigemAgendamento.Aplicativo => "Aplicativo",
        OrigemAgendamento.PresencialBalcao => "Balcão",
        _ => "Site"
    };

    private static string MapearOrigemString(OrigemAgendamento origem) => origem switch
    {
        OrigemAgendamento.Site => "site",
        OrigemAgendamento.WhatsApp => "whatsapp",
        OrigemAgendamento.Aplicativo => "app",
        OrigemAgendamento.PresencialBalcao => "balcao",
        _ => "site"
    };
}
