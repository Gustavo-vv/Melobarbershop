using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melobarbershop.Application.DTOs;

namespace Melobarbershop.Application.Servicos.Services;

public interface IHorarioFuncionamentoService
{
    Task<ApiResposta<IEnumerable<HorarioFuncionamentoDto>>> ListarSemanaAsync();
    Task<ApiResposta<HorarioFuncionamentoDto>> AtualizarDiaAsync(DayOfWeek diaSemana, bool aberto, TimeSpan abertura, TimeSpan fechamento);
    Task<ApiResposta<IEnumerable<HorarioEspecialDto>>> ListarEspeciaisAsync(int? ano);
    Task<ApiResposta<HorarioEspecialDto>> CriarEspecialAsync(DateTime data, bool aberto, TimeSpan? abertura, TimeSpan? fechamento, string descricao);
    Task<ApiResposta<bool>> RemoverEspecialAsync(int id);
}
