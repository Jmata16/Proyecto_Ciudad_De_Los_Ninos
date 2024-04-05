using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    public class Dashboard : Controller
    {
        public IActionResult Dopaje()
        {
            return View();
        }

        public IActionResult MapaAlbergue()
        {
            return View();
        }

        public IActionResult MapaResidencias()
        {
            return View();
        }

    }
}
