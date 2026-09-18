// ============================================================================
// ARQUIVO: Melobarbershop.Domain/Interfaces/Repositories/IServicoRepository.cs
// CAMADA: Domain (Núcleo do Domínio)
// CONEXÕES ARQUITETURAIS:
// - Quem define: Domain (Inversão de Dependência - DIP)
// - Quem implementa: Melobarbershop.Infrastructure (ServicoRepository via EF Core)
// - Quem consome: Melobarbershop.Application (ServicoService, AgendamentoService)
// ============================================================================

namespace Melobarbershop.Domain.Interfaces.Repositories;

using Melobarbershop.Domain.Entidades;

/// <summary>
/// PAPEL ARQUITETURAL:
/// Contrato de persistência para os serviços profissionais oferecidos pela barbearia.
/// 
/// POR QUE EXISTE:
/// Provê acesso ao catálogo de procedimentos, permitindo consultas otimizadas por lote de IDs
/// para cálculo de duração e preços ao montar agendamentos com múltiplos itens.
/// </summary>
public interface IServicoRepository
{
    /// <summary>
    /// Busca os detalhes de um serviço pelo seu ID primário.
    /// </summary>
    Task<Servico?> ObterPorIdAsync(int id);

    /// <summary>
    /// Lista todos os serviços cadastrados (inclusive os desativados) para o painel de controle.
    /// </summary>
    Task<IEnumerable<Servico>> ObterTodosAsync();

    /// <summary>
    /// Lista apenas serviços ativos e marcados como ExibirNoSite para montagem da página pública e agendamento.
    /// </summary>
    Task<IEnumerable<Servico>> ObterAtivosAsync();

    /// <summary>
    /// Busca em lote (SQL IN) múltiplos serviços através de seus IDs.
    /// Otimização: Evita o problema N+1 ao validar todos os serviços de um agendamento em uma única query.
    /// </summary>
    Task<IEnumerable<Servico>> ObterPorIdsAsync(IEnumerable<int> ids);

    /// <summary>
    /// Insere um novo serviço no catálogo da barbearia.
    /// </summary>
    Task AdicionarAsync(Servico servico);

    /// <summary>
    /// Atualiza nome, preço, duração ou status de um serviço existente.
    /// </summary>
    Task AtualizarAsync(Servico servico);

    /// <summary>
    /// Remove um serviço do banco de dados (quando aplicável).
    /// </summary>
    Task RemoverAsync(Servico servico);
}