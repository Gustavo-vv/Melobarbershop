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
}
