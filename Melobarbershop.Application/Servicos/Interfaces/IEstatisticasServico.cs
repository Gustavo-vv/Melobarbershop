using System.Threading.Tasks;
using SenacFlix.Application.DTOs;

namespace SenacFlix.Application.Servicos.Interfaces
{
    public interface IEstatisticasServico
    {
        Task<ApiResposta<DashboardEstatisticasDto>> ObterEstatisticasDashboardAsync();
    }
}