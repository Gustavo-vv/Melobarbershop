namespace Melobarbershop.Application.DTOs;

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

public class CriarAvaliacaoDto
{
    public int AgendamentoId { get; set; }
    public string ClienteId { get; set; } = string.Empty;
    public int NotaEstrelas { get; set; }
    public string? Comentario { get; set; }
}

public class ResumoAvaliacoesDto
{
    public string BarbeiroId { get; set; } = string.Empty;
    public string NomeBarbeiro { get; set; } = string.Empty;
    public double MediaEstrelas { get; set; }
    public int TotalAvaliacoes { get; set; }
}
