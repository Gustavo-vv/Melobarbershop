// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Entidades/Agendamento.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem chama/usa:
//   * Melobarbershop.Application (AgendamentoService, DTOs de Agendamento, MappingProfile)
//   * Melobarbershop.Infrastructure (BarbeariaDbContext, AgendamentoConfiguration, AgendamentoRepository)
//   * Melobarbershop.API (AgendamentosController via DTOs/Services)
// - Quem ele referencia:
//   * ApplicationUser (relacionamentos de Cliente e Barbeiro)
//   * AgendamentoItem (composição 1:N dos serviços contratados)
//   * Avaliacao (relação 1:1 opcional de feedback pós-atendimento)
//   * StatusAgendamento e OrigemAgendamento (Enums de estado e canal)
// ============================================================================

using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Domain.Entidades;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Entidade raiz de agregação que representa o núcleo de operação da barbearia: a reserva de horário.
/// 
/// POR QUE EXISTE:
/// Centraliza a amarração entre o Cliente (quem recebe o serviço), o Barbeiro (quem executa),
/// o intervalo temporal de execução (início e fim) e os serviços inclusos (via AgendamentoItem).
/// 
/// O QUE QUEBRARIA SE NÃO EXISTISSE:
/// Todo o fluxo principal do sistema (agendamento web, bloqueio de horários, notificações WhatsApp,
/// cálculo de faturamento por barbeiro e relatórios de atendimento) deixaria de existir.
/// </summary>
public class Agendamento
{
    /// <summary>
    /// Identificador único sequencial do agendamento (chave primária auto-incremental).
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Chave estrangeira que referencia o ApplicationUser com papel de 'Cliente'.
    /// </summary>
    public string ClienteId { get; set; } = string.Empty;

    /// <summary>
    /// Propriedade de navegação do EF Core para os dados cadastrais do Cliente.
    /// Utiliza null! para suprimir avisos de nulabilidade, pois o EF Core se encarrega de preenchê-la ao carregar o relacionamento.
    /// </summary>
    public ApplicationUser Cliente { get; set; } = null!;

    /// <summary>
    /// Chave estrangeira que referencia o ApplicationUser com papel de 'Barbeiro'.
    /// </summary>
    public string BarbeiroId { get; set; } = string.Empty;

    /// <summary>
    /// Propriedade de navegação do EF Core para o Barbeiro prestador do serviço.
    /// </summary>
    public ApplicationUser Barbeiro { get; set; } = null!;

    /// <summary>
    /// Data e hora exata de início do atendimento.
    /// Regra de negócio: Deve respeitar o horário de funcionamento e não conflitar com outros agendamentos do mesmo barbeiro.
    /// </summary>
    public DateTime DataHoraInicio { get; set; }

    /// <summary>
    /// Data e hora prevista para o término do atendimento.
    /// Regra de negócio: Calculada dinamicamente com base na soma da duração (minutos) de todos os itens do agendamento.
    /// </summary>
    public DateTime DataHoraFim { get; set; }

    /// <summary>
    /// Máquina de estados do agendamento (Pendente, Confirmado, EmAtendimento, Concluido, Cancelado, NaoCompareceu).
    /// Padrão: Pendente até que seja aceito/confirmado ou pago.
    /// </summary>
    public StatusAgendamento Status { get; set; } = StatusAgendamento.Pendente;

    /// <summary>
    /// Canal pelo qual a marcação foi criada (Site, WhatsApp, Desktop/Balcão).
    /// Permite métricas de conversão e rastreabilidade da origem do lead.
    /// </summary>
    public OrigemAgendamento Origem { get; set; } = OrigemAgendamento.Site;

    /// <summary>
    /// Notas ou instruções adicionais registradas pelo cliente ou pelo atendente (ex: 'Cliente prefere corte na tesoura').
    /// </summary>
    public string? Observacoes { get; set; }

    /// <summary>
    /// Registro de auditoria do momento exato em que a reserva foi inserida no banco (sempre em UTC).
    /// </summary>
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Coleção dos serviços que compõem este agendamento (ex: Corte + Barba).
    /// Permite múltiplos serviços no mesmo intervalo com registro de valor histórico.
    /// </summary>
    public ICollection<AgendamentoItem> Itens { get; set; } = new List<AgendamentoItem>();

    /// <summary>
    /// Avaliação de satisfação (nota e comentário) deixada pelo cliente após a conclusão do serviço.
    /// Relacionamento opcional 1:1 (nullable).
    /// </summary>
    public Avaliacao? Avaliacao { get; set; }
}
