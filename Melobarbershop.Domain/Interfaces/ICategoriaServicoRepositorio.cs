using System.Collections.Generic;
using System.Threading.Tasks;
using Melobarbershop.Domain.Entidades;

namespace Melobarbershop.Domain.Interfaces
{
    public interface ICategoriaServicoRepositorio
    {
        Task<IEnumerable<CategoriaServico>> ObterTodasAsync(bool incluirInativas = false);
        Task<CategoriaServico?> ObterPorIdAsync(int id);
        Task<CategoriaServico> AdicionarAsync(CategoriaServico categoria);
        Task AtualizarAsync(CategoriaServico categoria);
        Task DesativarAsync(int id);
        Task ReativarAsync(int id);
    }
}