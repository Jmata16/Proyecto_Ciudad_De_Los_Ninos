using Microsoft.AspNetCore.Mvc;
using Proyecto_Ciudad_De_Los_Ninos.Models;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    public class MapaController : Controller
    {
        private readonly ApplicationDBContext _context;

        public MapaController(ApplicationDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var data = _context.Jovenes
                .GroupBy(j => j.Localizacion)
                .Select(g => new {
                    Locacion = g.Key,
                    TotalJovenes = g.Count()
                })
                .ToList();

            ViewData["DatosLocacion"] = data;
            return View();
        }
    }
}