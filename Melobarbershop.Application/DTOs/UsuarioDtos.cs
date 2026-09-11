using System.ComponentModel.DataAnnotations;

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
    [Required(ErrorMessage = "O Nome Completo e obrigatorio.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O E-mail e obrigatorio.")]
    [EmailAddress(ErrorMessage = "E-mail em formato invalido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A Senha e obrigatoria.")]
    [MinLength(6, ErrorMessage = "A senha deve ter no minimo 6 caracteres.")]
    public string Senha { get; set; } = string.Empty;

    [Required(ErrorMessage = "A confirmacao de senha e obrigatoria.")]
    [Compare("Senha", ErrorMessage = "As senhas nao coincidem.")]
    public string ConfirmarSenha { get; set; } = string.Empty;

    [Required(ErrorMessage = "O Telefone é obrigatorio.")]
    public string TelefoneWhatsApp { get; set; } = string.Empty;
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
