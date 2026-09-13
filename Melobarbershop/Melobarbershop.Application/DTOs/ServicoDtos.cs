namespace Melobarbershop.Application.DTOs;

public class ServicoDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int DuracaoMinutos { get; set; }
    public bool Ativo { get; set; }
    public bool ExibirNoSite { get; set; }
}

public class CriarServicoDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int DuracaoMinutos { get; set; }
    public bool ExibirNoSite { get; set; } = true;
}

public class AtualizarServicoDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int DuracaoMinutos { get; set; }
    public bool Ativo { get; set; } = true;
    public bool ExibirNoSite { get; set; } = true;
}
