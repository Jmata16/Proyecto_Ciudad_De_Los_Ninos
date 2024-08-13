using Microsoft.AspNetCore.Diagnostics; // Para acceder a detalles de la excepción
using Microsoft.AspNetCore.Mvc;
using Proyecto_Ciudad_De_Los_Ninos.Models;
using System.Diagnostics;
using System.Net;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Página de inicio
        public IActionResult Index()
        {
            return View();
        }

        // Acción de guardar (ejemplo)
        public IActionResult Guardar()
        {
            return View();
        }

        // Página de privacidad
        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> juego()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/html/example.html");
            var htmlContent = await System.IO.File.ReadAllTextAsync(filePath);
            ViewData["HtmlContent"] = htmlContent;
            return View();
        }
        // Página de error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var statusCode = HttpContext.Response.StatusCode;
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = exceptionFeature?.Error;

            var errorViewModel = new ErrorViewModel
            {
                StatusCode = statusCode,
                Message = exception?.Message ?? "Ha ocurrido un error inesperado.",
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            _logger.LogError(exception, "Error occurred with status code {StatusCode}", statusCode);

            return View(errorViewModel);
        }
    }
}
