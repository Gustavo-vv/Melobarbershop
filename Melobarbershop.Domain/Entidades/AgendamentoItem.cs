// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Entidades/AgendamentoItem.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem chama/usa:
//   * Agendamento (agregação pai 1:N)
//   * Melobarbershop.Application (cálculo de preço total, duração de agendamentos)
//   * Melobarbershop.Infrastructure (Mapeamento EF Core e persistência relacional)
// - Quem ele referencia:
//   * Agendamento (chave e navegação pai)
//   * Servico (catálogo do serviço prestado)
// ============================================================================

namespace Melobarbershop.Domain.Entidades
{
    /// <summary>
    /// PAPEL ARQUITETURAL:
    /// Entidade dependente (item de agregação) que materializa a contratação de um serviço específico dentro de um agendamento.
    /// 
    /// POR QUE EXISTE:
    /// Permite que um mesmo agendamento contenha múltiplos serviços (ex: Corte de Cabelo + Barboterapia).
    /// Além disso, armazena o PrecoCobrado de forma desnormalizada para preservar o histórico financeiro.
    /// 
    /// O QUE QUEBRARIA SE NÃO EXISTISSE:
    /// O sistema ficaria limitado a apenas 1 serviço por horário marcado, ou perderia o histórico do valor cobrado
    /// se o preço do catálogo do serviço fosse reajustado no futuro.
    /// </summary>
    public class AgendamentoItem
    {
        /// <summary>
        /// Identificador único do item do agendamento (chave primária).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Chave estrangeira do agendamento pai.
        /// </summary>
        public int AgendamentoId { get; set; }

        /// <summary>
        /// Propriedade de navegação para a entidade pai Agendamento.
        /// </summary>
        public Agendamento Agendamento { get; set; } = null!;

        /// <summary>
        /// Chave estrangeira do serviço associado do catálogo.
        /// </summary>
        public int ServicoId { get; set; }

        /// <summary>
        /// Propriedade de navegação para os dados do serviço (nome, duração original, descrição).
        /// </summary>
        public Servico Servico { get; set; } = null!;

        /// <summary>
        /// Valor monetário efetivamente cobrado por este serviço no momento da marcação.
        /// Regra de negócio: Snapshot financeiro imutável — se o preço do serviço na tabela 'Servicos' subir amanhã,
        /// os agendamentos já criados mantêm este valor original registrado.
        /// </summary>
        public decimal PrecoCobrado { get; set; }
    }
}
