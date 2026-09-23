namespace Melobarbershop.UI.Areas.Admin.Models;

public class PainelDadosPayloadViewModel
{
    public bool Sucesso { get; set; } = true;
    public string Mensagem { get; set; } = string.Empty;
    public PainelKpisViewModel Kpis { get; set; } = new();
    public List<PainelAgendaItemViewModel> Agenda { get; set; } = new();
    public List<PainelProfissionalOpcaoViewModel> Profissionais { get; set; } = new();
    public List<PainelOrigemItemViewModel> Origem { get; set; } = new();
    public PainelCancelamentosViewModel Cancel { get; set; } = new();
}

public class PainelKpisViewModel
{
    public decimal Faturamento { get; set; }
    public decimal FaturamentoDelta { get; set; }
    public int Atendimentos { get; set; }
    public int AtendimentosMeta { get; set; } = 30;
    public int TotalAgendamentos { get; set; }
    public int Ocupacao { get; set; }
    public int OcupacaoDelta { get; set; }
    public decimal Ticket { get; set; }
    public int AgendamentosWeb { get; set; }
}

public class PainelAgendaItemViewModel
{
    public int Id { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public string Hora { get; set; } = string.Empty;
    public string DataHoraIso { get; set; } = string.Empty;
    public string Servico { get; set; } = string.Empty;
    public string ProfId { get; set; } = string.Empty;
    public string ProfNome { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public string Origem { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty; // pendente, confirmado, em_atendimento, concluido, cancelado, nao_compareceu
}

public class PainelProfissionalOpcaoViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
}

public class PainelOrigemItemViewModel
{
    public string Label { get; set; } = string.Empty;
    public int Pct { get; set; }
    public int Quantidade { get; set; }
    public string Color { get; set; } = string.Empty;
}

public class PainelCancelamentosViewModel
{
    public int TaxaCancelamento { get; set; }
    public int TaxaNaoComparecimento { get; set; }
    public int TotalCancelados { get; set; }
    public int TotalNaoCompareceu { get; set; }
}

public class AlterarStatusRequest
{
    public int Id { get; set; }
    public string Acao { get; set; } = string.Empty; // confirmar, iniciar-atendimento, concluir, cancelar, nao-comparecimento
    public string? Motivo { get; set; }
}
