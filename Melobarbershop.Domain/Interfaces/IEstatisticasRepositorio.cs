using System.Threading.Tasks;
using Melobarbershop.Domain.Entidades;

namespace Melobarbershop.Domain.Interfaces
{
    public interface IEstatisticasRepositorio
    {
        Task<EstatisticasDashboard> ObterMetricasGeraisAsync();
    }
}