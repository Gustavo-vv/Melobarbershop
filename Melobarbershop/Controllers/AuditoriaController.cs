using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Melobarbershop.Application.Servicos.Interfaces;

namespace Melobarbershop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditoriaController : ControllerBase
    {
        private readonly IAuditoriaAppService _auditoriaService;

        public AuditoriaController(IAuditoriaAppService auditoriaService)
        {
            _auditoriaService = auditoriaService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterRecentes([FromQuery] int quantidade = 50)
        {
            var resposta = await _auditoriaService.ObterRecentesAsync(quantidade);
            return Ok(resposta);
        }
    }
}