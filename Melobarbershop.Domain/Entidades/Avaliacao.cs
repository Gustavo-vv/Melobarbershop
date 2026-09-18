// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Entidades/Avaliacao.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem chama/usa:
//   * Melobarbershop.Application (AvaliacaoService, DTOs de Avaliação, Mapeamentos)
//   * Melobarbershop.Infrastructure (BarbeariaDbContext, AvaliacaoConfiguration, AvaliacaoRepository)
//   * Melobarbershop.API e UI (Exibição de depoimentos no site e dashboard de métricas dos barbeiros)
// - Quem ele referencia:
//   * Agendamento (relação 1:1 obrigatória)
//   * ApplicationUser (relacionamento com Cliente autor e Barbeiro avaliado)
// ============================================================================

namespace Melobarbershop.Domain.Entidades;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Entidade de domínio responsável pelo feedback de qualidade dos serviços prestados.
/// 
/// POR QUE EXISTE:
/// Permite medir a satisfação dos clientes e calcular a média de estrelas de cada barbeiro,
/// alimentando o ranqueamento no site e feedback contínuo da barbearia.
/// 
/// O QUE QUEBRARIA SE NÃO EXISTISSE:
/// O sistema de reputação e notas dos profissionais, os depoimentos da landing page e
/// os relatórios de qualidade da gerência não poderiam ser gerados.
/// </summary>
public class Avaliacao
{
    /// <summary>
    /// Identificador único da avaliação (chave primária).
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Chave estrangeira do agendamento que originou esta avaliação.
    /// Regra de negócio: Uma avaliação só pode existir atrelada a um agendamento concluído (1:1).
    /// </summary>
    public int AgendamentoId { get; set; }

    /// <summary>
    /// Propriedade de navegação para o agendamento correspondente.
    /// </summary>
    public Agendamento Agendamento { get; set; } = null!;

    /// <summary>
    /// Chave estrangeira do cliente que redigiu a avaliação.
    /// </summary>
    public string ClienteId { get; set; } = string.Empty;

    /// <summary>
    /// Propriedade de navegação para os dados do cliente avaliador.
    /// </summary>
    public ApplicationUser Cliente { get; set; } = null!;

    /// <summary>
    /// Chave estrangeira do barbeiro avaliado.
    /// </summary>
    public string BarbeiroId { get; set; } = string.Empty;

    /// <summary>
    /// Propriedade de navegação para os dados do barbeiro avaliado.
    /// </summary>
    public ApplicationUser Barbeiro { get; set; } = null!;

    /// <summary>
    /// Pontuação quantitativa atribuída ao serviço.
    /// Regra de validação: Deve ser um número inteiro estritamente entre 1 (mínimo) e 5 (máximo).
    /// </summary>
    public int NotaEstrelas { get; set; }

    /// <summary>
    /// Texto descritivo opcional contendo o elogio, crítica ou sugestão deixada pelo cliente.
    /// </summary>
    public string? Comentario { get; set; }

    /// <summary>
    /// Data e hora do envio do feedback (em UTC).
    /// </summary>
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
}
