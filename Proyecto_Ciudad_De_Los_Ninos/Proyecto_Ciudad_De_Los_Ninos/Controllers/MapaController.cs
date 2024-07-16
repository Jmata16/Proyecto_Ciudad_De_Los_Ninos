using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    public class MapaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
