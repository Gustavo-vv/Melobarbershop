using System.Collections.Generic;

namespace Melobarbershop.Domain.Entidades
{
    public class EstatisticasDashboard
    {
        public decimal FaturamentoTotalMes { get; set; }
        public decimal FaturamentoHoje { get; set; }
        public int TotalAgendamentosHoje { get; set; }
        public int AgendamentosPendentes { get; set; }
        public int TotalClientesCadastrados { get; set; }

        public List<ServicoMaisVendido> TopServicos { get; set; } = new();
        public List<BarbeiroProdutividade> ProdutividadeBarbeiros { get; set; } = new();
    }

    public class ServicoMaisVendido
    {
        public string NomeServico { get; set; } = string.Empty;
        public int QuantidadeRealizada { get; set; }
        public decimal TotalArrecadado { get; set; }
    }

    public class BarbeiroProdutividade
    {
        public string NomeBarbeiro { get; set; } = string.Empty;
        public int CortesRealizados { get; set; }
    }
}