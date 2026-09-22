// ============================================================================
// Arquivo: HorarioApiService.cs
// Camada: Melobarbershop.Desktop (Services)
// Objetivo: Serviço cliente para consumo dos endpoints de Horários de Funcionamento.
// ============================================================================

using Melobarbershop.Desktop.Models;

namespace Melobarbershop.Desktop.Services;

public class HorarioApiService
{
    public async Task<ApiResposta<List<HorarioFuncionamentoDto>>> ObterSemanalAsync()
        => await ApiClient.GetAsync<List<HorarioFuncionamentoDto>>("/api/HorarioFuncionamento");

    public async Task<ApiResposta<HorarioFuncionamentoDto>> AtualizarDiaAsync(int diaSemana, AtualizarHorarioFuncionamentoDto dto)
        => await ApiClient.PutAsync<AtualizarHorarioFuncionamentoDto, HorarioFuncionamentoDto>($"/api/HorarioFuncionamento/{diaSemana}", dto);

    public async Task<ApiResposta<List<HorarioEspecialDto>>> ObterEspeciaisAsync()
        => await ApiClient.GetAsync<List<HorarioEspecialDto>>("/api/HorarioFuncionamento/especiais");

    public async Task<ApiResposta<HorarioEspecialDto>> CriarEspecialAsync(CriarHorarioEspecialDto dto)
        => await ApiClient.PostAsync<CriarHorarioEspecialDto, HorarioEspecialDto>("/api/HorarioFuncionamento/especiais", dto);

    public async Task<ApiResposta<bool>> RemoverEspecialAsync(int id)
        => await ApiClient.DeleteAsync<bool>($"/api/HorarioFuncionamento/especiais/{id}");

    public async Task<ApiResposta<List<BloqueioAgendaDto>>> ObterBloqueiosAsync(string? barbeiroId = null, DateTime? inicio = null, DateTime? fim = null)
    {
        var query = new List<string>();
        if (!string.IsNullOrWhiteSpace(barbeiroId)) query.Add($"barbeiroId={Uri.EscapeDataString(barbeiroId)}");
        if (inicio.HasValue) query.Add($"inicio={inicio.Value:yyyy-MM-ddTHH:mm:ss}");
        if (fim.HasValue) query.Add($"fim={fim.Value:yyyy-MM-ddTHH:mm:ss}");
        var qs = query.Count > 0 ? "?" + string.Join("&", query) : "";
        return await ApiClient.GetAsync<List<BloqueioAgendaDto>>($"/api/usuarios/bloqueios{qs}");
    }

    public async Task<ApiResposta<BloqueioAgendaDto>> CriarBloqueioAsync(CriarBloqueioAgendaDto dto)
        => await ApiClient.PostAsync<CriarBloqueioAgendaDto, BloqueioAgendaDto>("/api/usuarios/bloqueios", dto);

    public async Task<ApiResposta<bool>> RemoverBloqueioAsync(int id)
        => await ApiClient.DeleteAsync<bool>($"/api/usuarios/bloqueios/{id}");
}

