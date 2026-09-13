namespace Melobarbershop.Domain.Enums;

/// <summary>
/// Representa os possíveis status de um agendamento.
/// </summary>
public enum StatusAgendamento
{
    Pendente = 1,
    Confirmado = 2,
    EmAtendimento = 3,
    Concluido = 4,
    Cancelado = 5,
    NaoCompareceu = 6
}
