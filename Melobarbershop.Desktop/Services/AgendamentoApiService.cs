using Melobarbershop.Desktop.Models;

namespace Melobarbershop.Desktop.Services
{
    public class AgendamentoApiService
    {
        public async Task<ApiResposta<List<AgendamentoDto>>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim)
        {
            var inicioStr = Uri.EscapeDataString(inicio.ToString("o"));
            var fimStr = Uri.EscapeDataString(fim.ToString("o"));
            return await ApiClient.GetAsync<List<AgendamentoDto>>($"/api/Agendamentos/periodo?inicio={inicioStr}&fim={fimStr}");
        }

        public async Task<ApiResposta<List<AgendamentoDto>>> ListarPorClienteAsync(string clienteId)
        {
            var clienteIdEsc = Uri.EscapeDataString(clienteId);
            return await ApiClient.GetAsync<List<AgendamentoDto>>($"/api/Agendamentos/cliente/{clienteIdEsc}");
        }

        public async Task<ApiResposta<AgendamentoDto>> ObterPorIdAsync(int id)
        {
            return await ApiClient.GetAsync<AgendamentoDto>($"/api/Agendamentos/id?id={id}");
        }

        public async Task<ApiResposta<AgendamentoDto>> ConfirmarAsync(int id)
        {
            return await ApiClient.PatchAsync<object, AgendamentoDto>($"/api/Agendamentos/{id}/confirmar", new { });
        }

        public async Task<ApiResposta<AgendamentoDto>> IniciarAtendimentoAsync(int id)
        {
            return await ApiClient.PatchAsync<object, AgendamentoDto>($"/api/Agendamentos/{id}/iniciar-atendimento", new { });
        }

        public async Task<ApiResposta<AgendamentoDto>> ConcluirAsync(int id)
        {
            return await ApiClient.PatchAsync<object, AgendamentoDto>($"/api/Agendamentos/{id}/concluir", new { });
        }

        public async Task<ApiResposta<AgendamentoDto>> CancelarAsync(int id, string? motivo = null)
        {
            var query = string.IsNullOrWhiteSpace(motivo) ? "" : $"?motivo={Uri.EscapeDataString(motivo)}";
            return await ApiClient.PatchAsync<object, AgendamentoDto>($"/api/Agendamentos/{id}/cancelar{query}", new { });
        }

        public async Task<ApiResposta<AgendamentoDto>> RegistrarNaoComparecimentoAsync(int id)
        {
            return await ApiClient.PatchAsync<object, AgendamentoDto>($"/api/Agendamentos/{id}/nao-comparecimento", new { });
        }
    }
}
