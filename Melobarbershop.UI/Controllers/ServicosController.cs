using Microsoft.AspNetCore.Mvc;

namespace Melobarbershop.UI.Controllers
{
    public class ServicosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
