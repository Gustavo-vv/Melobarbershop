// ============================================================================
// ARQUIVO: Melobarbershop.Application/DTOs/AvaliacaoDtos.cs
// CAMADA: Application (Contratos e DTOs)
// CONEXÕES ARQUITETURAIS:
// - Quem consome: Melobarbershop.API (AvaliacoesController), Melobarbershop.UI (depoimentos da Home)
// - Quem mapeia: MappingProfile.cs (AutoMapper transforma Avaliacao -> AvaliacaoDto)
// ============================================================================

namespace Melobarbershop.Application.DTOs;

/// <summary>
/// PAPEL ARQUITETURAL:
/// DTO de saída para visualização individual de uma avaliação de atendimento.
/// Fornece o feedback do cliente acompanhado dos nomes formatados de cliente e profissional.
/// </summary>
public class AvaliacaoDto
{
    public int Id { get; set; }
    public int AgendamentoId { get; set; }
    public string ClienteId { get; set; } = string.Empty;
    public string NomeCliente { get; set; } = string.Empty;
    public string BarbeiroId { get; set; } = string.Empty;
    public string NomeBarbeiro { get; set; } = string.Empty;
    public int NotaEstrelas { get; set; }
    public string? Comentario { get; set; }
    public DateTime DataCriacao { get; set; }
}

/// <summary>
/// DTO de entrada (Request Model) para submissão de feedback após um serviço prestado.
/// </summary>
public class CriarAvaliacaoDto
{
    public int AgendamentoId { get; set; }
    public string ClienteId { get; set; } = string.Empty;
    public int NotaEstrelas { get; set; }
    public string? Comentario { get; set; }
}

/// <summary>
/// DTO agregado com indicadores de desempenho e reputação de um barbeiro (média e total de avaliações).
/// </summary>
public class ResumoAvaliacoesDto
{
    public string BarbeiroId { get; set; } = string.Empty;
    public string NomeBarbeiro { get; set; } = string.Empty;
    public double MediaEstrelas { get; set; }
    public int TotalAvaliacoes { get; set; }
}
