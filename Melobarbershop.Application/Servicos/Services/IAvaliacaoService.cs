// ============================================================================
// Arquivo: IAvaliacaoService.cs
// Camada: Melobarbershop.Application (Serviços - Contratos)
// Objetivo: Definir as operações de negócio para registro e consulta de avaliações (feedback/NPS)
//           dos clientes em relação aos barbeiros e atendimentos prestados.
// Papel na Arquitetura:
//   - Interface que desacopla os controladores de avaliação da implementação concreta.
//   - Fornece cálculos estatísticos agregados (médias, contagem por estrelas) para o painel gerencial.
// ============================================================================

using Melobarbershop.Application.DTOs;

namespace Melobarbershop.Application.Servicos.Services;

/// <summary>
/// Contrato do serviço de avaliações e reputação de profissionais na barbearia.
/// </summary>
public interface IAvaliacaoService
{
    /// <summary>Obtém uma avaliação pelo seu identificador único.</summary>
    Task<AvaliacaoDto?> ObterPorIdAsync(int id);

    /// <summary>Recupera a avaliação associada a um agendamento específico (cada agendamento pode ter apenas uma avaliação).</summary>
    Task<AvaliacaoDto?> ObterPorAgendamentoAsync(int agendamentoId);

    /// <summary>Lista o histórico de avaliações recebidas por um barbeiro específico.</summary>
    Task<IEnumerable<AvaliacaoDto>> ListarPorBarbeiroAsync(string barbeiroId);

    /// <summary>Lista todas as avaliações feitas por um cliente específico.</summary>
    Task<IEnumerable<AvaliacaoDto>> ListarPorClienteAsync(string clienteId);

    /// <summary>Calcula a média de pontuação, total de feedbacks e distribuição de 1 a 5 estrelas do barbeiro.</summary>
    Task<ResumoAvaliacoesDto> ObterResumoAvaliacoesBarbeiroAsync(string barbeiroId);

    /// <summary>Valida e persiste a avaliação de um cliente referente a um atendimento concluído.</summary>
    Task<AvaliacaoDto> RegistrarAvaliacaoAsync(CriarAvaliacaoDto dto);
}

