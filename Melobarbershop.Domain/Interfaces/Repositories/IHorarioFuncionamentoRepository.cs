using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melobarbershop.Domain.Entidades;

namespace Melobarbershop.Domain.Interfaces.Repositories;

public interface IHorarioFuncionamentoRepository
{
    Task<IEnumerable<HorarioFuncionamento>> ObterSemanaAsync();
    Task<HorarioFuncionamento?> ObterPorDiaSemanaAsync(DayOfWeek diaSemana);
    Task AdicionarOuAtualizarDiaAsync(HorarioFuncionamento horario);
    Task<IEnumerable<HorarioEspecial>> ObterEspeciaisAsync(int? ano = null);
    Task<HorarioEspecial?> ObterEspecialPorDataAsync(DateTime data);
    Task<HorarioEspecial?> ObterEspecialPorIdAsync(int id);
    Task AdicionarOuAtualizarEspecialAsync(HorarioEspecial especial);
    Task RemoverEspecialAsync(HorarioEspecial especial);
}
