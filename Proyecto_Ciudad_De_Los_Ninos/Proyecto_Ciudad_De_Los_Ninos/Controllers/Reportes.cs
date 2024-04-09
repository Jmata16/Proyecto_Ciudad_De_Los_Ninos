using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    public class Reportes : Controller
    {
        public IActionResult Citas()
        {
            return View();
        }
        public IActionResult Incidentes()
        {
            return View();
        }
        public IActionResult PruebasDopaje()
        {
            return View();
        }
        public IActionResult ReportesMedicos()
        {
            return View();
        }
    }
}
