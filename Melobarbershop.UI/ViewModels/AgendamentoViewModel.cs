// Nome do arquivo: AgendamentoViewModel.cs
// Objetivo: Representar os dados necessários para a tela de Agendamento da barbearia.
// Camada: UI
// Como participa: É preenchido pelo AgendamentoController (buscando dados na API via ApiClient)
//                 e consumido pela View Agendamento/Index.cshtml para renderização inicial no servidor.

using System.Globalization;

namespace Melobarbershop.UI.ViewModels
{
    public class AgendamentoViewModel
    {
        public ServicoItemViewModel? ServicoSelecionado { get; set; }
        public List<ServicoItemViewModel> ServicosDisponiveis { get; set; } = new();
        public List<BarbeiroItemViewModel> BarbeirosDisponiveis { get; set; } = new();
        public string? MensagemErro { get; set; }

        /// <summary>
        /// Indica se o cliente está autenticado na aplicação web.
        /// Usado na View para ajustar o botão de ação (Continuar vs Login).
        /// </summary>
        public bool UsuarioLogado { get; set; }

        /// <summary>
        /// Horários disponíveis (label "HH:mm" e valor exato ISO retornado pela API)
        /// para o barbeiro/serviço/dia na carga inicial da página.
        /// </summary>
        public List<HorarioDisponivelViewModel> HorariosDisponiveis { get; set; } = new();

        public bool TemServicoSelecionado => ServicoSelecionado != null;
    }

    public class HorarioDisponivelViewModel
    {
        public string Label { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
        public bool Disponivel { get; set; } = true;
    }

    public class ServicoItemViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public int DuracaoMinutos { get; set; }
        public decimal Preco { get; set; }

        public string PrecoFormatado => Preco.ToString("C", new CultureInfo("pt-BR"));
        public string PrecoInvariante => Preco.ToString("F2", CultureInfo.InvariantCulture);
    }

    public class BarbeiroItemViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string? FotoUrl { get; set; }
        public string Inicial => !string.IsNullOrWhiteSpace(Nome) ? Nome[0].ToString().ToUpper() : "B";
    }
}
