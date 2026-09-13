// Nome do arquivo: ServicoViewModel.cs
// Objetivo: Representar os dados de um serviço especificamente para exibição na View.
// Camada: UI
// Como participa: É preenchido pelo ServicosController (a partir do ServicoDto vindo da API)
//                 e consumido pela View Servicos/Index.cshtml. Mantém a View desacoplada do DTO da Application.

using System.Globalization;

namespace Melobarbershop.UI.ViewModels
{
    public class ServicoViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public int DuracaoMinutos { get; set; }
        public decimal Preco { get; set; }

        // Propriedades auxiliares só para exibição (formatação), evitando lógica de apresentação na .cshtml
        public string PrecoFormatado => Preco.ToString("C", new CultureInfo("pt-BR"));
        public string PrecoInvariante => Preco.ToString("F2", CultureInfo.InvariantCulture);
    }

    // Agrupa a lista de serviços já separada em populares/outros, como a View precisa.
    public class ListaServicosViewModel
    {
        public List<ServicoViewModel> Populares { get; set; } = new();
        public List<ServicoViewModel> Outros { get; set; } = new();
        public string? MensagemErro { get; set; }

        public bool TemServicos => Populares.Count > 0 || Outros.Count > 0;
    }
}
