using System;

namespace Melobarbershop.Application.DTOs;

public class HorarioFuncionamentoDto
{
    public int Id { get; set; }
    public DayOfWeek DiaSemana { get; set; }
    public string NomeDiaSemana { get; set; } = string.Empty;
    public bool Aberto { get; set; }
    public TimeSpan HoraAbertura { get; set; }
    public TimeSpan HoraFechamento { get; set; }
}

public class AtualizarHorarioFuncionamentoDto
{
    public bool Aberto { get; set; }
    public TimeSpan HoraAbertura { get; set; }
    public TimeSpan HoraFechamento { get; set; }
}

public class HorarioEspecialDto
{
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public bool Aberto { get; set; }
    public TimeSpan? HoraAbertura { get; set; }
    public TimeSpan? HoraFechamento { get; set; }
    public string Descricao { get; set; } = string.Empty;
}

public class CriarHorarioEspecialDto
{
    public DateTime Data { get; set; }
    public bool Aberto { get; set; }
    public TimeSpan? HoraAbertura { get; set; }
    public TimeSpan? HoraFechamento { get; set; }
    public string Descricao { get; set; } = string.Empty;
}
