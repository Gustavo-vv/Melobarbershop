namespace Barbearia.Application.Interfaces.Services;

using Melobarbershop.Domain.Entidades;

public interface IServicoService
{
    Task<Servico?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Servico>> ListarAtivosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Servico>> ListarExibidosNoSiteAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Servico>> ListarTodosAsync(CancellationToken cancellationToken = default);
    Task<Servico> CriarAsync(string nome, string? descricao, decimal preco, int duracaoMinutos, bool exibirNoSite = true, CancellationToken cancellationToken = default);
    Task<Servico> AtualizarAsync(int id, string nome, string? descricao, decimal preco, int duracaoMinutos, bool exibirNoSite, CancellationToken cancellationToken = default);
    Task DesativarAsync(int id, CancellationToken cancellationToken = default);
    Task AtivarAsync(int id, CancellationToken cancellationToken = default);
}