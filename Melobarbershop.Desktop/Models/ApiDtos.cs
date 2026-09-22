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

    public class AtualizarUsuarioDto
    {
        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;

        [JsonPropertyName("telefoneWhatsApp")]
        public string? TelefoneWhatsApp { get; set; }

        [JsonPropertyName("dataNascimento")]
        public DateTime? DataNascimento { get; set; }

        [JsonPropertyName("preferenciasNotas")]
        public string? PreferenciasNotas { get; set; }

        [JsonPropertyName("fotoUrl")]
        public string? FotoUrl { get; set; }

        [JsonPropertyName("percentualComissao")]
        public decimal? PercentualComissao { get; set; }

        [JsonPropertyName("ativo")]
        public bool Ativo { get; set; } = true;
    }

    public enum StatusAgendamentoDto
    {
        Pendente = 1,
        Confirmado = 2,
        EmAtendimento = 3,
        Concluido = 4,
        Cancelado = 5,
        NaoCompareceu = 6
    }

    public enum OrigemAgendamentoDto
    {
        Site = 1,
        WhatsApp = 2,
        Aplicativo = 3,
        PresencialBalcao = 4
    }

    public class AgendamentoItemDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("servicoId")]
        public int ServicoId { get; set; }

        [JsonPropertyName("nomeServico")]
        public string NomeServico { get; set; } = string.Empty;

        [JsonPropertyName("precoCobrado")]
        public decimal PrecoCobrado { get; set; }
    }

    public class AgendamentoDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("clienteId")]
        public string ClienteId { get; set; } = string.Empty;

        [JsonPropertyName("nomeCliente")]
        public string NomeCliente { get; set; } = string.Empty;

        [JsonPropertyName("telefoneCliente")]
        public string? TelefoneCliente { get; set; }

        [JsonPropertyName("barbeiroId")]
        public string BarbeiroId { get; set; } = string.Empty;

        [JsonPropertyName("nomeBarbeiro")]
        public string NomeBarbeiro { get; set; } = string.Empty;

        [JsonPropertyName("dataHoraInicio")]
        public DateTime DataHoraInicio { get; set; }

        [JsonPropertyName("dataHoraFim")]
        public DateTime DataHoraFim { get; set; }

        [JsonPropertyName("status")]
        public StatusAgendamentoDto Status { get; set; }

        [JsonPropertyName("origem")]
        public OrigemAgendamentoDto Origem { get; set; } = OrigemAgendamentoDto.Site;

        [JsonPropertyName("observacoes")]
        public string? Observacoes { get; set; }

        [JsonPropertyName("dataCriacao")]
        public DateTime DataCriacao { get; set; }

        [JsonPropertyName("valorTotal")]
        public decimal ValorTotal { get; set; }

        [JsonPropertyName("itens")]
        public List<AgendamentoItemDto> Itens { get; set; } = new();

        public string ServicosFormatados => Itens != null && Itens.Count > 0
            ? string.Join(", ", Itens.Select(i => i.NomeServico))
            : "-";
    }

    // ──────────────────────────────────────────────────────────────────────
    // Horários de Funcionamento
    // ──────────────────────────────────────────────────────────────────────

    public class HorarioFuncionamentoDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("diaSemana")]
        public int DiaSemana { get; set; }

        [JsonPropertyName("nomeDiaSemana")]
        public string NomeDiaSemana { get; set; } = string.Empty;

        [JsonPropertyName("aberto")]
        public bool Aberto { get; set; }

        [JsonPropertyName("horaAbertura")]
        public string HoraAbertura { get; set; } = "08:00:00";

        [JsonPropertyName("horaFechamento")]
        public string HoraFechamento { get; set; } = "19:00:00";

        /// <summary>Converte "HH:mm:ss" para exibição "HH:mm".</summary>
        public string HoraAberturaFormatada => HoraAbertura.Length >= 5 ? HoraAbertura[..5] : HoraAbertura;
        public string HoraFechamentoFormatada => HoraFechamento.Length >= 5 ? HoraFechamento[..5] : HoraFechamento;
    }

    public class AtualizarHorarioFuncionamentoDto
    {
        [JsonPropertyName("aberto")]
        public bool Aberto { get; set; }

        [JsonPropertyName("horaAbertura")]
        public string HoraAbertura { get; set; } = "08:00:00";

        [JsonPropertyName("horaFechamento")]
        public string HoraFechamento { get; set; } = "19:00:00";
    }

    public class HorarioEspecialDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("data")]
        public DateTime Data { get; set; }

        [JsonPropertyName("aberto")]
        public bool Aberto { get; set; }

        [JsonPropertyName("horaAbertura")]
        public string? HoraAbertura { get; set; }

        [JsonPropertyName("horaFechamento")]
        public string? HoraFechamento { get; set; }

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; } = string.Empty;
    }

    public class CriarHorarioEspecialDto
    {
        [JsonPropertyName("data")]
        public DateTime Data { get; set; }

        [JsonPropertyName("aberto")]
        public bool Aberto { get; set; }

        [JsonPropertyName("horaAbertura")]
        public string? HoraAbertura { get; set; }

        [JsonPropertyName("horaFechamento")]
        public string? HoraFechamento { get; set; }

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; } = string.Empty;
    }
}
