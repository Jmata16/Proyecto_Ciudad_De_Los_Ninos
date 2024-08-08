using API_Ciudad_De_Los_Ninos.Models;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Ciudad_De_Los_Ninos.Models;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    public class RifaController : Controller
    {
        private readonly ApplicationDBContext _context;

        public RifaController(ApplicationDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Rifas = _context.Rifas.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Index(RifaEntry entry)
        {
            if (ModelState.IsValid)
            {
                entry.FechaCompra = DateTime.Now;
                _context.RifaEntries.Add(entry);
                _context.SaveChanges();
                return RedirectToAction("Confirmacion");
            }
            ViewBag.Rifas = _context.Rifas.ToList();
            return View(entry);
        }

        public IActionResult Confirmacion()
        {
            return View();
        }
        public IActionResult CrearRifa()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CrearRifa(Rifa rifa)
        {
            if (ModelState.IsValid)
            {
                rifa.NumeroGanador = null;
                _context.Rifas.Add(rifa);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(rifa);
        }

        public async Task<IActionResult> DeterminarGanador(int rifaId)
        {
            var rifa = await _context.Rifas.FindAsync(rifaId);
            if (rifa != null && rifa.FechaRifa <= DateTime.Now && rifa.NumeroGanador == null)
            {
                var random = new Random();
                var numeroGanador = random.Next(1, 101);
                rifa.NumeroGanador = numeroGanador;

                _context.Rifas.Update(rifa);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("RifaDetalles");
        }


        public IActionResult RifaDetalles()
        {
            var rifas = _context.Rifas
                .OrderBy(r => r.FechaRifa)
                .ToList();

           
            foreach (var rifa in rifas)
            {
                if (rifa.FechaRifa <= DateTime.Now && rifa.NumeroGanador == null)
                {
                    var random = new Random();
                    var numeroGanador = random.Next(1, 101);

                    rifa.NumeroGanador = numeroGanador;

                    _context.Rifas.Update(rifa);
                    _context.SaveChanges();
                }
            }


            ViewBag.Rifas = rifas;

            return View();
        }


        public IActionResult RifasProximas()
        {
            var rifasProximas = _context.Rifas
                .Where(r => r.FechaRifa >= DateTime.Now)
                .OrderBy(r => r.FechaRifa)
                .ToList();

            return View(rifasProximas);
        }













    }
}
