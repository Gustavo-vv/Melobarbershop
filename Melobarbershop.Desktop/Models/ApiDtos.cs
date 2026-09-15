using System.Text.Json.Serialization;

namespace Melobarbershop.Desktop.Models
{
    public class ApiResposta<T>
    {
        [JsonPropertyName("sucesso")]
        public bool Sucesso { get; set; }

        [JsonPropertyName("mensagem")]
        public string Mensagem { get; set; } = string.Empty;

        [JsonPropertyName("dados")]
        public T? Dados { get; set; }

        [JsonPropertyName("erros")]
        public List<string>? Erros { get; set; }
    }

    public class LoginDto
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("senha")]
        public string Senha { get; set; } = string.Empty;
    }

    public class LoginRespostaDto
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        [JsonPropertyName("expiracao")]
        public DateTime Expiracao { get; set; }

        [JsonPropertyName("nomeUsuario")]
        public string NomeUsuario { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("fotoPerfilUrl")]
        public string? FotoPerfilUrl { get; set; }

        [JsonPropertyName("perfis")]
        public List<string> Perfis { get; set; } = new();
    }

    public class ServicoDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;

        [JsonPropertyName("descricao")]
        public string? Descricao { get; set; }

        [JsonPropertyName("preco")]
        public decimal Preco { get; set; }

        [JsonPropertyName("duracaoMinutos")]
        public int DuracaoMinutos { get; set; }

        [JsonPropertyName("ativo")]
        public bool Ativo { get; set; }

        [JsonPropertyName("exibirNoSite")]
        public bool ExibirNoSite { get; set; }
    }

    public class CriarServicoDto
    {
        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;

        [JsonPropertyName("descricao")]
        public string? Descricao { get; set; }

        [JsonPropertyName("preco")]
        public decimal Preco { get; set; }

        [JsonPropertyName("duracaoMinutos")]
        public int DuracaoMinutos { get; set; }

        [JsonPropertyName("exibirNoSite")]
        public bool ExibirNoSite { get; set; } = true;
    }

    public class AtualizarServicoDto
    {
        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;

        [JsonPropertyName("descricao")]
        public string? Descricao { get; set; }

        [JsonPropertyName("preco")]
        public decimal Preco { get; set; }

        [JsonPropertyName("duracaoMinutos")]
        public int DuracaoMinutos { get; set; }

        [JsonPropertyName("ativo")]
        public bool Ativo { get; set; } = true;

        [JsonPropertyName("exibirNoSite")]
        public bool ExibirNoSite { get; set; } = true;
    }

    public class UsuarioDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("phoneNumber")]
        public string? PhoneNumber { get; set; }

        [JsonPropertyName("dataNascimento")]
        public DateTime? DataNascimento { get; set; }

        [JsonPropertyName("preferenciasNotas")]
        public string? PreferenciasNotas { get; set; }

        [JsonPropertyName("fotoUrl")]
        public string? FotoUrl { get; set; }

        [JsonPropertyName("percentualComissao")]
        public decimal? PercentualComissao { get; set; }

        [JsonPropertyName("ativo")]
        public bool Ativo { get; set; }

        [JsonPropertyName("dataCadastro")]
        public DateTime DataCadastro { get; set; }

        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; } = new();

        public string RolesFormatadas => Roles != null && Roles.Count > 0 ? string.Join(", ", Roles) : "Sem perfil";
    }
}
