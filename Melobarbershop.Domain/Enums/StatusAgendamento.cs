namespace Melobarbershop.Domain.Enums
{
    public enum StatusAgendamento
    {
        Pendente = 1,   // Cliente solicitou o agendamento
        Confirmado = 2, // Barbeiro ou Admin confirmou o horÃ¡rio
        Concluido = 3,  // Atendimento realizado com sucesso
        Cancelado = 4   // Agendamento cancelado
    }
}