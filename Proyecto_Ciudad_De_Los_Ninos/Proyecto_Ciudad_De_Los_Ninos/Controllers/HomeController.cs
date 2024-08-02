using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Proyecto_Ciudad_De_Los_Ninos.Models;
using System.Diagnostics;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // P�gina de inicio
        public IActionResult Index()
        {
            return View();
        }

        // Acci�n de guardar (ejemplo)
        public IActionResult Guardar()
        {
            return View();
        }

        // P�gina de privacidad
        public IActionResult Privacy()
        {
            return View();
        }

        // P�gina de error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
