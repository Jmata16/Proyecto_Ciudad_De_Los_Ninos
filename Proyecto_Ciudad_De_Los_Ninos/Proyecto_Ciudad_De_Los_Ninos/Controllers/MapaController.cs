using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Ciudad_De_Los_Ninos.Models;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    [Authorize(Policy = "RolAll")]
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
                .Where(j => j.estado == "Activo") // Filtrar solo los jóvenes con estado "Activo"
                .GroupBy(j => j.Localizacion)
                .Select(g => new {
                    Locacion = g.Key,
                    TotalJovenes = g.Count() // Contar solo los jóvenes con estado "Activo"
                })
                .ToList();

            ViewData["DatosLocacion"] = data;
            return View();
        }


    }
}