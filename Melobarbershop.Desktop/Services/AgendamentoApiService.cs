// ============================================================================
// Arquivo: AgendamentoApiService.cs
// Camada: Melobarbershop.Desktop (Services)
// Objetivo: Serviço cliente para consumo dos endpoints de Agendamentos (/api/Agendamentos).
// Papel na Arquitetura:
//   - Fornece operações de consulta por período e cliente para o painel de atendimento (UcDashboard, UcAgendamentos).
//   - Gerencia transições de status da agenda (Confirmar, Iniciar Atendimento, Concluir, Cancelar, Falta).
// ============================================================================

using Melobarbershop.Desktop.Models;

namespace Melobarbershop.Desktop.Services;

/// <summary>
/// Serviço de integração com a API para operações e transições de status de agendamentos.
/// </summary>
public class AgendamentoApiService
{
    /// <summary>
    /// Consulta os agendamentos realizados dentro de um período de datas.
    /// </summary>
    public async Task<ApiResposta<List<AgendamentoDto>>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim)
    {
        var inicioStr = Uri.EscapeDataString(inicio.ToString("o"));
        var fimStr = Uri.EscapeDataString(fim.ToString("o"));
        return await ApiClient.GetAsync<List<AgendamentoDto>>($"/api/Agendamentos/periodo?inicio={inicioStr}&fim={fimStr}");
    }

    /// <summary>
    /// Consulta o histórico de agendamentos de um cliente específico.
    /// </summary>
    public async Task<ApiResposta<List<AgendamentoDto>>> ListarPorClienteAsync(string clienteId)
    {
        var clienteIdEsc = Uri.EscapeDataString(clienteId);
        return await ApiClient.GetAsync<List<AgendamentoDto>>($"/api/Agendamentos/cliente/{clienteIdEsc}");
    }

    /// <summary>
    /// Busca as informações completas de um agendamento pelo ID.
    /// </summary>
    public async Task<ApiResposta<AgendamentoDto>> ObterPorIdAsync(int id)
    {
        return await ApiClient.GetAsync<AgendamentoDto>($"/api/Agendamentos/id?id={id}");
    }

    /// <summary>
    /// Transiciona o status do agendamento para Confirmado.
    /// </summary>
    public async Task<ApiResposta<AgendamentoDto>> ConfirmarAsync(int id)
    {
        return await ApiClient.PatchAsync<object, AgendamentoDto>($"/api/Agendamentos/{id}/confirmar", new { });
    }

    /// <summary>
    /// Transiciona o status do agendamento para Em Atendimento.
    /// </summary>
    public async Task<ApiResposta<AgendamentoDto>> IniciarAtendimentoAsync(int id)
    {
        return await ApiClient.PatchAsync<object, AgendamentoDto>($"/api/Agendamentos/{id}/iniciar-atendimento", new { });
    }

    /// <summary>
    /// Transiciona o status do agendamento para Concluído.
    /// </summary>
    public async Task<ApiResposta<AgendamentoDto>> ConcluirAsync(int id)
    {
        return await ApiClient.PatchAsync<object, AgendamentoDto>($"/api/Agendamentos/{id}/concluir", new { });
    }

    /// <summary>
    /// Cancela o agendamento registrando opcionalmente o motivo.
    /// </summary>
    public async Task<ApiResposta<AgendamentoDto>> CancelarAsync(int id, string? motivo = null)
    {
        var query = string.IsNullOrWhiteSpace(motivo) ? "" : $"?motivo={Uri.EscapeDataString(motivo)}";
        return await ApiClient.PatchAsync<object, AgendamentoDto>($"/api/Agendamentos/{id}/cancelar{query}", new { });
    }

    /// <summary>
    /// Registra o não comparecimento do cliente (falha de presença).
    /// </summary>
    public async Task<ApiResposta<AgendamentoDto>> RegistrarNaoComparecimentoAsync(int id)
    {
        return await ApiClient.PatchAsync<object, AgendamentoDto>($"/api/Agendamentos/{id}/nao-comparecimento", new { });
    }
}
