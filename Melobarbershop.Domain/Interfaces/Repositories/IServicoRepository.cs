// Arquivo: Melobarbershop.Domain/Interfaces/Repositories/IServicoRepository.cs
// Namespace: Melobarbershop.Domain.Interfaces.Repositories
// Conteúdo: interface IServicoRepository
// Resumo: Contrato de repositório para operações sobre Serviços (listar, obter, atualizar disponibilidade).
namespace Melobarbershop.Domain.Interfaces.Repositories;

using Melobarbershop.Domain.Entidades;

public interface IServicoRepository
{
    Task<Servico?> ObterPorIdAsync(int id);
    Task<IEnumerable<Servico>> ObterTodosAsync();
    Task<IEnumerable<Servico>> ObterAtivosAsync();
    Task<IEnumerable<Servico>> ObterPorIdsAsync(IEnumerable<int> ids);
    Task AdicionarAsync(Servico servico);
    Task AtualizarAsync(Servico servico);
    Task RemoverAsync(Servico servico);
}