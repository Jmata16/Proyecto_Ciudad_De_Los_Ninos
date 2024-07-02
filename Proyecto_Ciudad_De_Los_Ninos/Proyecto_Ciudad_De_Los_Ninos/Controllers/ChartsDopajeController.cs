using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Ciudad_De_Los_Ninos.Models;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    public class ChartsDopajeController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ChartsDopajeController(ApplicationDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var resultados = _context.Pruebas_Dopaje
                .GroupBy(p => p.resultado)
                .Select(g => new { Resultado = g.Key, Count = g.Count() })
                .ToList();

            return View(resultados);
        }
    }
}
