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
        /// Horários disponíveis (formato "HH:mm") para o barbeiro/serviço/dia usados
        /// na carga inicial da página (hoje, primeiro barbeiro da lista).
        /// A troca de dia/barbeiro no cliente é feita via AJAX em
        /// GET /Agendamento/HorariosDisponiveis, que consulta a mesma regra de negócio.
        /// </summary>
        public List<string> HorariosDisponiveis { get; set; } = new();

        public bool TemServicoSelecionado => ServicoSelecionado != null;
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
