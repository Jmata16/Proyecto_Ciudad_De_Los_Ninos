using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Ciudad_De_Los_Ninos.Models;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    [Authorize]
    public class ChartsDopajeController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ChartsDopajeController(ApplicationDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            // Obtener la fecha actual para la consulta
            var fechaInicio = DateTime.Now.AddMonths(-12); // Ejemplo: últimos 12 meses

            // Agrupar por mes y contar las pruebas
            var pruebasPorMes = _context.Pruebas_Dopaje
                .Where(p => p.fecha >= fechaInicio) // Filtrar por los últimos 12 meses
                .GroupBy(p => new { p.fecha.Year, p.fecha.Month })
                .Select(g => new
                {
                    Mes = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month)} {g.Key.Year}",
                    Count = g.Count()
                })
                .ToList();

            // Agrupar por resultado
            var resultados = _context.Pruebas_Dopaje
                .GroupBy(p => p.resultado)
                .Select(g => new { Resultado = g.Key, Count = g.Count() })
                .ToList();

            // Contar las pruebas positivas y negativas
            var positivos = resultados.FirstOrDefault(r => r.Resultado == "Positivo");
            var negativos = resultados.FirstOrDefault(r => r.Resultado == "Negativo");

            // Contar el total de pruebas y de jóvenes
            var totalPruebas = _context.Pruebas_Dopaje.Count();
            var totalJovenes = _context.Jovenes.Count();  

            // Pasar los datos a la vista
            ViewBag.PositivoCount = positivos != null ? positivos.Count : 0;
            ViewBag.NegativoCount = negativos != null ? negativos.Count : 0;
            ViewBag.TotalPruebas = totalPruebas;
            ViewBag.TotalJovenes = totalJovenes;  
            ViewBag.PruebasPorMes = pruebasPorMes; 

            return View(resultados);
        }






    }
}
