// Arquivo: Melobarbershop.Domain/Interfaces/Repositories/IPacoteRepository.cs
// Namespace: Melobarbershop.Domain.Interfaces.Repositories
// Conteúdo: interface IPacoteRepository
// Resumo: Contrato de repositório para operações com Pacote e seus itens.
namespace Melobarbershop.Domain.Interfaces.Repositories;

using Melobarbershop.Domain.Entidades;

public interface IPacoteRepository
{
    Task<Pacote?> ObterPorIdAsync(int id);
    Task<Pacote?> ObterPorIdComItensAsync(int id);
    Task<IEnumerable<Pacote>> ObterTodosAsync();
    Task<IEnumerable<Pacote>> ObterAtivosAsync();
    Task AdicionarAsync(Pacote pacote);
    Task AtualizarAsync(Pacote pacote);
    Task RemoverAsync(Pacote pacote);
}