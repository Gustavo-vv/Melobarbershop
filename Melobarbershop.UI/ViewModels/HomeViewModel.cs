// Nome do arquivo: HomeViewModel.cs
// Objetivo: Representar os dados necessários para exibição na página inicial (Home/Landing Page).
// Camada: UI
// Como participa: É preenchido pelo HomeController a partir de consultas na API via ApiClient
//                 e consumido pela View Home/Index.cshtml para renderização dinâmica dos serviços.

using System.Globalization;

namespace Melobarbershop.UI.ViewModels
{
    public class HomeViewModel
    {
        public List<HomeServicoItemViewModel> Servicos { get; set; } = new();
        public string HorarioFuncionamento { get; set; } = "Terça a Sábado";
        public string ClientesPorDia { get; set; } = "20+";
        public string AnoFundacao { get; set; } = "2024";
        public string? MensagemErro { get; set; }

        public bool TemServicos => Servicos.Count > 0;
    }

    public class HomeServicoItemViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public int DuracaoMinutos { get; set; }
        public bool Destaque { get; set; }
        public string IconeSvg { get; set; } = "tesoura.svg";

        public string PrecoFormatado => Preco.ToString("C", new CultureInfo("pt-BR"));
        public string PrecoInvariante => Preco.ToString("F2", CultureInfo.InvariantCulture);
        public string UrlAgendamento => $"/Agendamento?serviceId={Id}&serviceName={Uri.EscapeDataString(Nome)}&price={PrecoInvariante}&duration={DuracaoMinutos}";
    }
}
