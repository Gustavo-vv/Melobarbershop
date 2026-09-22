using System;

namespace Melobarbershop.Domain.Entidades;

/// <summary>
/// Define uma exceção ou horário especial para uma data específica (ex: feriados, datas comemorativas ou horários estendidos).
/// </summary>
public class HorarioEspecial
{
    public int Id { get; set; }
    public DateTime Data { get; set; } // Apenas a data importa
    public bool Aberto { get; set; }
    public TimeSpan? HoraAbertura { get; set; }  // null se Aberto = false
    public TimeSpan? HoraFechamento { get; set; }
    public string Descricao { get; set; } = string.Empty; // ex: "Véspera de Natal"
}
