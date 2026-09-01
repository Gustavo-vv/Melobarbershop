using System.Collections.Generic;
using System.Threading.Tasks;
using Melobarbershop.Domain.Entidades;

namespace Melobarbershop.Domain.Interfaces
{
    public interface IAuditoriaRepositorio
    {
        Task RegistrarAsync(Auditoria auditoria);
        Task<IEnumerable<Auditoria>> ObterRecentesAsync(int quantidade = 50);
    }
}