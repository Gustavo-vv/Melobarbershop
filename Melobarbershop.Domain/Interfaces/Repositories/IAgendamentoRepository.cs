// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Interfaces/Repositories/IAgendamentoRepository.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem define: Domain (Inversão de Dependência - DIP)
// - Quem implementa: Melobarbershop.Infrastructure (AgendamentoRepository via EF Core)
// - Quem consome: Melobarbershop.Application (AgendamentoService, HorarioService)
// ============================================================================

using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Domain.Interfaces.Repositories;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Contrato que abstrai a persistência e as consultas complexas da entidade Agendamento.
/// 
/// PRINCÍPIO DE DESIGN (DIP - Dependency Inversion Principle):
/// A camada de domínio declara o que precisa para operar regras de agendamento,
/// sem saber se os dados vêm de SQL Server, PostgreSQL, SQLite ou memória.
/// A camada de Infrastructure se encarrega de implementar os detalhes técnicos.
/// </summary>
public interface IAgendamentoRepository
{
    /// <summary>
    /// Recupera os dados básicos de um agendamento pelo seu ID (sem carregar coleções filhas).
    /// </summary>
    Task<Agendamento?> ObterPorIdAsync(int id);

    /// <summary>
    /// Recupera o agendamento completo com Eager Loading (Include) de Cliente, Barbeiro, Itens, Serviços e Avaliação.
    /// Utilizado para renderizar detalhes completos da comanda e resumo de confirmação.
    /// </summary>
    Task<Agendamento?> ObterPorIdCompletoAsync(int id);

    /// <summary>
    /// Lista o histórico cronológico de agendamentos de um cliente específico.
    /// </summary>
    Task<IEnumerable<Agendamento>> ObterPorClienteAsync(string clienteId);

    /// <summary>
    /// Lista os agendamentos de um determinado barbeiro dentro de uma janela de datas (ex: grade diária/semanal).
    /// </summary>
    Task<IEnumerable<Agendamento>> ObterPorBarbeiroEPeriodoAsync(string barbeiroId, DateTime inicio, DateTime fim);

    /// <summary>
    /// Lista todos os agendamentos de todos os profissionais em um intervalo (visão geral da recepção/gerência).
    /// </summary>
    Task<IEnumerable<Agendamento>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim);

    /// <summary>
    /// Lista os próximos compromissos a partir de um horário de corte, com filtro opcional por barbeiro.
    /// </summary>
    Task<IEnumerable<Agendamento>> ObterProximosAgendamentosAsync(DateTime aPartirDe, string? barbeiroId = null);

    /// <summary>
    /// Consulta agendamentos confirmados que iniciam dentro de uma janela temporal futura (ex: entre 1h55m e 2h05m à frente).
    /// Utilizado pelo serviço em segundo plano (BackgroundService) para disparar lembretes de WhatsApp.
    /// </summary>
    Task<IEnumerable<Agendamento>> ObterAgendamentosParaLembreteAsync(DateTime janelaInicio, DateTime janelaFim);

    /// <summary>
    /// Validação de concorrência e conflito de horário: verifica se o barbeiro já possui outro agendamento ativo
    /// cujo intervalo colida com [inicio, fim].
    /// Permite ignorar um ID existente no caso de reagendamento / edição do próprio registro.
    /// </summary>
    Task<bool> ExisteConflitoDeHorarioAsync(string barbeiroId, DateTime inicio, DateTime fim, int? agendamentoIdIgnorar = null);

    /// <summary>
    /// Insere um novo agendamento na base de dados.
    /// </summary>
    Task AdicionarAsync(Agendamento agendamento);

    /// <summary>
    /// Atualiza os dados de um agendamento existente (horários, barbeiro, observações).
    /// </summary>
    Task AtualizarAsync(Agendamento agendamento);

    /// <summary>
    /// Atualização atômica e rápida apenas da coluna Status do agendamento (ex: de Confirmado para Concluido ou Cancelado).
    /// </summary>
    Task AtualizarStatusAsync(int id, StatusAgendamento status);
}