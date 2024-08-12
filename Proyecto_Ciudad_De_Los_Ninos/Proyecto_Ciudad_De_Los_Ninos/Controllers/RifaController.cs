using API_Ciudad_De_Los_Ninos.Models;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Ciudad_De_Los_Ninos.Models;
using Microsoft.EntityFrameworkCore;

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
                return RedirectToAction("RifaDetalles", "Rifa");
            }
            return View(rifa);
        }

        [HttpPost]
        public async Task<IActionResult> DeterminarGanadorAPI(int rifaId)
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

            return Ok();
        }




        public IActionResult RifaDetalles()
        {
            var rifas = _context.Rifas
                .OrderBy(r => r.FechaRifa)
                .ToList();

           


            ViewBag.Rifas = rifas;

            return View();
        }

        public IActionResult RifaHistorial()
        {
            var rifas = _context.Rifas
                .OrderBy(r => r.FechaRifa)
                .ToList();



            ViewBag.Rifas = rifas;

            return View();
        }

        public IActionResult RifasProximas()
        {
            var fechaHoy = DateTime.Now.Date;
            var rifasHoy = _context.Rifas
                .Where(r => r.FechaRifa.Date == fechaHoy)
                .OrderBy(r => r.FechaRifa)
                .ToList();

            var rifasFuturas = _context.Rifas
                .Where(r => r.FechaRifa.Date > fechaHoy)
                .OrderBy(r => r.FechaRifa)
                .ToList();

            ViewBag.RifasHoy = rifasHoy;
            ViewBag.RifasFuturas = rifasFuturas;

            return View();
        }

        // Lista todas las rifas

        // Muestra el formulario para crear una nueva rifa
        public IActionResult Create()
        {
            return View();
        }

        // Procesa el formulario de creación de una nueva rifa
        [HttpPost]
        public IActionResult Create(Rifa rifa)
        {
            if (ModelState.IsValid)
            {
                rifa.NumeroGanador = null;  // No se asigna número ganador al crear la rifa
                _context.Rifas.Add(rifa);
                _context.SaveChanges();
                return RedirectToAction("RifaDetalles");
            }
            return View(rifa);
        }

        // Muestra el formulario para editar una rifa existente
        public IActionResult Edit(int id)
        {
            var rifa = _context.Rifas.Find(id);
            if (rifa == null)
            {
                return NotFound();
            }
            return View(rifa);
        }

        // Procesa el formulario de edición de una rifa existente
        [HttpPost]
        public IActionResult Edit(Rifa rifa)
        {
            var rifaExistente = _context.Rifas.Find(rifa.RifaId);
            if (rifaExistente == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Solo se permite actualizar el premio y la fecha
                rifaExistente.FechaRifa = rifa.FechaRifa;
                rifaExistente.Premio = rifa.Premio;

                _context.Rifas.Update(rifaExistente);
                _context.SaveChanges();
                return RedirectToAction("RifaDetalles");
            }

            return View(rifa);
        }

        // Muestra los detalles de una rifa
        public IActionResult Details(int id)
        {
            var rifa = _context.Rifas
                .Include(r => r.RifaEntries)  // Asegúrate de cargar las entradas de la rifa
                .FirstOrDefault(r => r.RifaId == id);

            if (rifa == null)
            {
                return NotFound();
            }

            return View(rifa);
        }


        // Muestra el formulario para eliminar una rifa existente
        public IActionResult Delete(int id)
        {
            var rifa = _context.Rifas.Find(id);
            if (rifa == null)
            {
                return NotFound();
            }
            return View(rifa);
        }

        // Procesa la eliminación de una rifa existente
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var rifa = _context.Rifas.Find(id);
            if (rifa == null)
            {
                return NotFound();
            }

            _context.Rifas.Remove(rifa);
            _context.SaveChanges();
            return RedirectToAction("RifaDetalles");
        }
    }










     


}
