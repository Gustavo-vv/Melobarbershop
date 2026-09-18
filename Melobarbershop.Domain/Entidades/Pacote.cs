// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Entidades/Pacote.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem chama/usa:
//   * Melobarbershop.Application (PacoteService, DTOs de Pacote)
//   * Melobarbershop.Infrastructure (BarbeariaDbContext, PacoteConfiguration, PacoteRepository)
//   * Melobarbershop.API e UI (Exibição de pacotes promocionais para agendamento)
// - Quem ele referencia:
//   * PacoteItem (itens de composição 1:N)
// ============================================================================

namespace Melobarbershop.Domain.Entidades
{
    /// <summary>
    /// PAPEL ARQUITETURAL:
    /// Entidade raiz de agregação que representa um combo promocional de múltiplos serviços vendidos juntos.
    /// 
    /// POR QUE EXISTE:
    /// Permite criar ofertas comerciais como 'Combo Barba + Cabelo + Sobrancelha' com desconto progressivo,
    /// agrupando serviços existentes sob um valor promocional unificado.
    /// 
    /// O QUE QUEBRARIA SE NÃO EXISTISSE:
    /// A barbearia teria que lançar serviços redundantes no catálogo (ex: criar um serviço avulso chamado 'Combo'),
    /// perdendo a rastreabilidade dos serviços individuais executados e da divisão de tempo de atendimento.
    /// </summary>
    public class Pacote
    {
        /// <summary>
        /// Identificador único do pacote (chave primária).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome comercial do combo (ex: "Combo Barba + Cabelo Tradicional").
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Preço fechado de venda do combo promocional.
        /// Regra de negócio: Geralmente menor do que a soma individual dos preços de tabela de cada item.
        /// </summary>
        public decimal PrecoTotal { get; set; }

        /// <summary>
        /// Define se o pacote está atualmente disponível para contratação no site ou sistema desktop.
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// Coleção dos serviços que compõem este pacote.
        /// </summary>
        public ICollection<PacoteItem> Itens { get; set; } = new List<PacoteItem>();
    }
}