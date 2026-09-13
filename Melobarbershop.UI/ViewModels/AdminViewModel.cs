// Nome do arquivo: AdminViewModel.cs
// Objetivo: Representar os dados do Painel Administrativo / Desktop Pro especificamente para exibição na View.
// Camada: UI
// Como participa: É preenchido pelo AdminController (a partir dos DTOs vindo da API via ApiClient)
//                 e consumido pela View Admin/Index.cshtml para renderização no servidor.

using System.Globalization;

namespace Melobarbershop.UI.ViewModels
{
    public class AdminViewModel
    {
        public List<AdminServicoViewModel> Servicos { get; set; } = new();
        public List<AdminUsuarioViewModel> Barbeiros { get; set; } = new();
        public List<AdminAgendamentoItemViewModel> AgendamentosFila { get; set; } = new();
        
        // Métricas do Dashboard
        public decimal FaturamentoHoje { get; set; } = 2450.00m;
        public int AtendimentosHoje { get; set; } = 28;
        public int AgendamentosWeb { get; set; } = 20;
        public double TaxaOcupacao { get; set; } = 91.4;

        public string FaturamentoHojeFormatado => FaturamentoHoje.ToString("C", new CultureInfo("pt-BR"));

        public string? MensagemErro { get; set; }
    }

    public class AdminServicoViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public int DuracaoMinutos { get; set; }
        public bool Ativo { get; set; }
        public bool ExibirNoSite { get; set; }

        public string PrecoFormatado => Preco.ToString("C", new CultureInfo("pt-BR"));
    }

    public class AdminUsuarioViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public string? FotoUrl { get; set; }
        public bool Ativo { get; set; }
        public List<string> Roles { get; set; } = new();

        public string Inicial => !string.IsNullOrWhiteSpace(Nome) ? Nome[0].ToString().ToUpper() : "B";
    }

    public class AdminAgendamentoItemViewModel
    {
        public int Id { get; set; }
        public string Horario { get; set; } = string.Empty;
        public string ClienteNome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string ServicoNome { get; set; } = string.Empty;
        public string BarbeiroNome { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string Origem { get; set; } = "Site Web";
        public string Status { get; set; } = "Confirmado";

        public string ValorFormatado => Valor.ToString("C", new CultureInfo("pt-BR"));
    }
}
