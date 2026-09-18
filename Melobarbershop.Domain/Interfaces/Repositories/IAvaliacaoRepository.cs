// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Interfaces/Repositories/IAvaliacaoRepository.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem define: Domain (Inversão de Dependência - DIP)
// - Quem implementa: Melobarbershop.Infrastructure (AvaliacaoRepository via EF Core)
// - Quem consome: Melobarbershop.Application (AvaliacaoService)
// ============================================================================

using Melobarbershop.Domain.Entidades;

namespace Melobarbershop.Domain.Interfaces.Repositories;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Contrato de persistência para as avaliações de qualidade deixadas pelos clientes.
/// 
/// POR QUE EXISTE:
/// Isola a forma de cálculo da média e o armazenamento de feedbacks de agendamento,
/// permitindo que o domínio consuma essas operações sem dependência de banco de dados.
/// </summary>
public interface IAvaliacaoRepository
{
    /// <summary>
    /// Busca uma avaliação individual pelo seu ID primário.
    /// </summary>
    Task<Avaliacao?> ObterPorIdAsync(int id);

    /// <summary>
    /// Localiza a avaliação vinculada a um agendamento específico (relação 1:1).
    /// </summary>
    Task<Avaliacao?> ObterPorAgendamentoIdAsync(int agendamentoId);

    /// <summary>
    /// Lista todas as avaliações recebidas por um determinado barbeiro para compor seu histórico de desempenho.
    /// </summary>
    Task<IEnumerable<Avaliacao>> ObterPorBarbeiroAsync(string barbeiroId);

    /// <summary>
    /// Lista todas as avaliações que um determinado cliente já redigiu no sistema.
    /// </summary>
    Task<IEnumerable<Avaliacao>> ObterPorClienteAsync(string clienteId);

    /// <summary>
    /// Executa o cálculo agregado da média aritmética das estrelas (1 a 5) de um barbeiro diretamente no banco de dados (AVG).
    /// </summary>
    Task<double> CalcularMediaAvaliacoesBarbeiroAsync(string barbeiroId);

    /// <summary>
    /// Valida se já existe uma avaliação registrada para o agendamento informado, evitando duplicidade de feedback.
    /// </summary>
    Task<bool> ExisteAvaliacaoParaAgendamentoAsync(int agendamentoId);

    /// <summary>
    /// Salva uma nova avaliação na base de dados.
    /// </summary>
    Task AdicionarAsync(Avaliacao avaliacao);
}