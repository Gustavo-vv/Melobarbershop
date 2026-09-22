using System;

namespace Melobarbershop.Domain.Entidades;

/// <summary>
/// Define a configuração de horário de funcionamento padrão para um dia da semana.
/// </summary>
public class HorarioFuncionamento
{
    public int Id { get; set; }
    public DayOfWeek DiaSemana { get; set; }
    public bool Aberto { get; set; }
    public TimeSpan HoraAbertura { get; set; }
    public TimeSpan HoraFechamento { get; set; }
}
