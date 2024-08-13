using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    public class HtmlController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
