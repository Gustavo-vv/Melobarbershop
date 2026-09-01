using System.Collections.Generic;
using System.Threading.Tasks;
using Melobarbershop.Domain.Entidades;

namespace Melobarbershop.Domain.Interfaces
{
    public interface IServicoRepositorio
    {
        Task<IEnumerable<Servico>> ObterTodosAsync(bool incluirInativos = false);
        Task<Servico?> ObterPorIdAsync(int id);
        Task<IEnumerable<Servico>> ObterPorCategoriaAsync(int categoriaId);
        Task<IEnumerable<Servico>> BuscarAsync(string? termo, int? categoriaId = null);
        Task<Servico> AdicionarAsync(Servico servico);
        Task AtualizarAsync(Servico servico);
        Task DesativarAsync(int id);
        Task ReativarAsync(int id);
        Task ExcluirPermanentementeAsync(int id);
    }
}