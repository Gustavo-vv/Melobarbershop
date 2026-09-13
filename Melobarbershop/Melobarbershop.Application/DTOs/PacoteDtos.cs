namespace Melobarbershop.Application.DTOs;

public class PacoteDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal PrecoTotal { get; set; }
    public bool Ativo { get; set; }
    public ICollection<ServicoDto> Servicos { get; set; } = new List<ServicoDto>();
}

public class CriarPacoteDto
{
    public string Nome { get; set; } = string.Empty;
    public decimal PrecoTotal { get; set; }
    public ICollection<int> ServicoIds { get; set; } = new List<int>();
}

public class AtualizarPacoteDto
{
    public string Nome { get; set; } = string.Empty;
    public decimal PrecoTotal { get; set; }
    public ICollection<int> ServicoIds { get; set; } = new List<int>();
    public bool Ativo { get; set; } = true;
}
