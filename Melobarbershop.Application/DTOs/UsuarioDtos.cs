namespace Melobarbershop.Application.DTOs;

public class UsuarioDto
{
    public string Id { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateTime? DataNascimento { get; set; }
    public string? PreferenciasNotas { get; set; }
    public string? FotoUrl { get; set; }
    public decimal? PercentualComissao { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCadastro { get; set; }
    public IList<string> Roles { get; set; } = new List<string>();
}

public class CriarUsuarioDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string? TelefoneWhatsApp { get; set; }
    public string Role { get; set; } = "Cliente"; // "Cliente", "Barbeiro", "Admin"
    public DateTime? DataNascimento { get; set; }
    public string? PreferenciasNotas { get; set; }
    public string? FotoUrl { get; set; }
    public decimal? PercentualComissao { get; set; }
}

public class AtualizarUsuarioDto
{
    public string Nome { get; set; } = string.Empty;
    public string? TelefoneWhatsApp { get; set; }
    public DateTime? DataNascimento { get; set; }
    public string? PreferenciasNotas { get; set; }
    public string? FotoUrl { get; set; }
    public decimal? PercentualComissao { get; set; }
    public bool Ativo { get; set; } = true;
}

public class CriarBloqueioAgendaDto
{
    public string BarbeiroId { get; set; } = string.Empty;
    public DateTime DataHoraInicio { get; set; }
    public DateTime DataHoraFim { get; set; }
    public string Motivo { get; set; } = string.Empty;
}

public class BloqueioAgendaDto
{
    public int Id { get; set; }
    public string BarbeiroId { get; set; } = string.Empty;
    public string NomeBarbeiro { get; set; } = string.Empty;
    public DateTime DataHoraInicio { get; set; }
    public DateTime DataHoraFim { get; set; }
    public string Motivo { get; set; } = string.Empty;
}
