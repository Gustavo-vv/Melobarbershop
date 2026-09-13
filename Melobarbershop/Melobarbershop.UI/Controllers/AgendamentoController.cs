using Microsoft.AspNetCore.Mvc;

namespace Melobarbershop.UI.Controllers
{
    public class AgendamentoController : Controller
    {
        public IActionResult Index([FromQuery] string? serviceId)
        {
            ViewBag.SelectedServiceId = serviceId;
            return View();
        }
    }
}
