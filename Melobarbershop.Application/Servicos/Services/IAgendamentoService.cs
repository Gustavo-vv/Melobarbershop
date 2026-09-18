// ============================================================================
// Arquivo: IAgendamentoService.cs
// Camada: Melobarbershop.Application (Serviços - Contratos)
// Objetivo: Definir as operações de negócio relacionadas ao ciclo de vida
//           dos agendamentos e cálculo de disponibilidade de horários.
// Papel na Arquitetura:
//   - Interface que desacopla os controladores e telas da implementação concreta (AgendamentoService).
//   - Todas as operações retornam ApiResposta<T> encapsulando status, mensagem e payload de retorno.
// ============================================================================

using Melobarbershop.Application.DTOs;

namespace Melobarbershop.Application.Servicos.Services;

/// <summary>
/// Contrato do serviço de agendamentos, orquestrando reservas, slots livres e transições de estado.
/// </summary>
public interface IAgendamentoService
{
    /// <summary>Recupera um agendamento específico com itens e relacionamentos carregados.</summary>
    Task<ApiResposta<AgendamentoDto>> ObterPorIdAsync(int id);

    /// <summary>Lista os agendamentos em uma faixa de datas, com filtro opcional por barbeiro.</summary>
    Task<ApiResposta<IEnumerable<AgendamentoDto>>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim, string? barbeiroId = null);

    /// <summary>Lista o histórico de agendamentos solicitados por um cliente específico.</summary>
    Task<ApiResposta<IEnumerable<AgendamentoDto>>> ListarPorClienteAsync(string clienteId);

    /// <summary>Calcula e retorna os horários iniciais livres de um profissional considerando duração total dos serviços.</summary>
    Task<ApiResposta<IEnumerable<DateTime>>> ListarHorariosDisponiveisAsync(string barbeiroId, DateTime data, IEnumerable<int> servicoIds);

    /// <summary>Retorna a grade completa do dia do barbeiro com o status (Disponível, Ocupado, Bloqueado) de cada slot de 30min.</summary>
    Task<ApiResposta<IEnumerable<HorarioSlotDto>>> ListarTodosHorariosDoDiaAsync(string barbeiroId, DateTime data, IEnumerable<int> servicoIds);

    /// <summary>Valida e cria um novo agendamento, checando colisões e disparando notificações.</summary>
    Task<ApiResposta<AgendamentoDto>> CriarAsync(CriarAgendamentoDto dto);

    /// <summary>Confirma o agendamento solicitado (transição de Pendente para Confirmado).</summary>
    Task<ApiResposta<AgendamentoDto>> ConfirmarAsync(int agendamentoId);

    /// <summary>Inicia o atendimento na cadeira (transição para EmAtendimento).</summary>
    Task<ApiResposta<AgendamentoDto>> IniciarAtendimentoAsync(int agendamentoId);

    /// <summary>Finaliza a execução dos serviços (transição para Concluido).</summary>
    Task<ApiResposta<AgendamentoDto>> ConcluirAsync(int agendamentoId);

    /// <summary>Cancela o horário agendado com justificativa opcional.</summary>
    Task<ApiResposta<AgendamentoDto>> CancelarAsync(int agendamentoId, string? motivo = null);

    /// <summary>Registra que o cliente faltou ao horário marcado sem aviso prévio (NoShow).</summary>
    Task<ApiResposta<AgendamentoDto>> RegistrarNaoComparecimentoAsync(int agendamentoId);

    /// <summary>Move o agendamento para uma nova data/hora mantendo histórico e validando nova disponibilidade.</summary>
    Task<ApiResposta<AgendamentoDto>> ReagendarAsync(int agendamentoId, ReagendarAgendamentoDto dto);
}