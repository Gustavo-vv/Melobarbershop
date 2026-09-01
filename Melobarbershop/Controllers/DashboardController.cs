using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Melobarbershop.Application.Servicos.Interfaces;

namespace Melobarbershop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardAppService _dashboardService;

        public DashboardController(IDashboardAppService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterEstatisticas()
        {
            var resposta = await _dashboardService.ObterEstatisticasAsync();
            return Ok(resposta);
        }
    }
}