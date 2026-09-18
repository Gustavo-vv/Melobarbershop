// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Entidades/PacoteItem.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem chama/usa:
//   * Pacote (agregação pai 1:N)
//   * Melobarbershop.Infrastructure (BarbeariaDbContext, Mapeamento EF Core N:M indireto)
// - Quem ele referencia:
//   * Pacote (chave e navegação pai)
//   * Servico (serviço incluído no pacote)
// ============================================================================

namespace Melobarbershop.Domain.Entidades
{
    /// <summary>
    /// PAPEL ARQUITETURAL:
    /// Entidade associativa (tabela de junção N:M com dados relacionais) entre Pacote e Servico.
    /// 
    /// POR QUE EXISTE:
    /// Define exatamente quais serviços individuais fazem parte de um pacote específico,
    /// permitindo que o mesmo serviço (ex: Barba) participe de múltiplos pacotes promocionais distintos.
    /// 
    /// O QUE QUEBRARIA SE NÃO EXISTISSE:
    /// A modelagem relacional de N serviços para N pacotes não existiria no banco de dados.
    /// </summary>
    public class PacoteItem
    {
        /// <summary>
        /// Identificador único da relação de item do pacote (chave primária).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Chave estrangeira do pacote pai.
        /// </summary>
        public int PacoteId { get; set; }

        /// <summary>
        /// Propriedade de navegação para a entidade Pacote.
        /// </summary>
        public Pacote Pacote { get; set; } = null!;

        /// <summary>
        /// Chave estrangeira do serviço que integra o combo.
        /// </summary>
        public int ServicoId { get; set; }

        /// <summary>
        /// Propriedade de navegação para a entidade Servico.
        /// </summary>
        public Servico Servico { get; set; } = null!;
    }
}
