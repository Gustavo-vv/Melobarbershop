// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Entidades/Servico.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem chama/usa:
//   * Melobarbershop.Application (ServicoService, AgendamentoService, DTOs de Serviço)
//   * Melobarbershop.Infrastructure (BarbeariaDbContext, ServicoConfiguration, ServicoRepository)
//   * Melobarbershop.UI (Catálogo da Home/Serviços e fluxo de agendamento online)
//   * Melobarbershop.Desktop (Cadastro de serviços e seleção na Frente de Caixa)
// - Quem ele referencia:
//   * Entidade autocontida (referenciada por AgendamentoItem e PacoteItem)
// ============================================================================

namespace Melobarbershop.Domain.Entidades;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Entidade de domínio que define o catálogo de serviços executados pela barbearia.
/// 
/// POR QUE EXISTE:
/// Parametriza o tempo padrão de duração de cada procedimento (para montagem da grade da agenda)
/// e o preço de tabela para cobrança nos agendamentos e faturamento.
/// 
/// O QUE QUEBRARIA SE NÃO EXISTISSE:
/// O cálculo de intervalo de início e fim dos agendamentos (DataHoraFim = DataHoraInicio + DuracaoMinutos)
/// e a exibição de preços e serviços no site e aplicativo deixariam de funcionar.
/// </summary>
public class Servico
{
    /// <summary>
    /// Identificador único do serviço (chave primária).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nome descritivo do procedimento (ex: "Corte Degradê (Fade)", "Barba Tradicional").
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Detalhamento dos cuidados inclusos (ex: "Inclui toalha quente, alinhamento na navalha e pós-barba").
    /// </summary>
    public string? Descricao { get; set; }

    /// <summary>
    /// Valor monetário padrão de tabela cobrado pelo serviço.
    /// </summary>
    public decimal Preco { get; set; }

    /// <summary>
    /// Tempo estimado em minutos para conclusão do serviço (ex: 30, 45, 60 min).
    /// Regra de negócio: Define a fração de tempo reservada na agenda do barbeiro.
    /// </summary>
    public int DuracaoMinutos { get; set; }

    /// <summary>
    /// Flag de controle operacional. Se false, o serviço foi descontinuado e não pode ser agendado.
    /// </summary>
    public bool Ativo { get; set; } = true;

    /// <summary>
    /// Flag de visibilidade comercial. Se false, o serviço é executado apenas internamente no balcão,
    /// não sendo exposto para auto-agendamento pelo cliente no site público.
    /// </summary>
    public bool ExibirNoSite { get; set; } = true;
}

