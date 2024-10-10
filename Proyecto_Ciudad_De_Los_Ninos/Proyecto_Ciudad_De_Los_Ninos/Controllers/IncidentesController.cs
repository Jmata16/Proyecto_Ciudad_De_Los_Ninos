using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Ciudad_De_Los_Ninos.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_Ciudad_De_Los_Ninos.Models;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace API_Ciudad_De_Los_Ninos.Controllers
{
    [Authorize(Policy = "Rol134")]
    public class IncidentesController : Controller
    {

        private readonly ApplicationDBContext _context;

        public IncidentesController(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var incidentes = await _context.Incidentes
                .Include(i => i.Usuario)
                .Include(i => i.Joven)
                .Where(i => i.estado == "Activo") // Solo registros activos
                .ToListAsync();

            return View(incidentes);
        }
        public async Task<IActionResult> Desactivado()
        {
            var incidentesDesactivados = await _context.Incidentes
                .Include(i => i.Usuario)
                .Include(i => i.Joven)
                .Where(i => i.estado == "Desactivado") // Solo registros desactivados
                .ToListAsync();

            return View(incidentesDesactivados);
        }


        // GET: Incidentes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var incidente = await _context.Incidentes
                .Include(i => i.Usuario)
                .Include(i => i.Joven)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (incidente == null)
            {
                return NotFound();
            }

            return View(incidente);
        }

        // GET: Incidentes/Create
        public IActionResult Create()
        {
            ViewData["Usuarios"] = new SelectList(_context.Users, "Id", "nombre_usuario");
            ViewData["Jovenes"] = new SelectList(_context.Jovenes, "Id", "nombre");
            return View();
        }

        // POST: Incidentes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,id_usuario,id_joven,fecha_hora,descripcion")] Incidentes incidente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(incidente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Usuarios"] = new SelectList(_context.Users, "Id", "nombre_usuario", incidente.id_usuario);
            ViewData["Jovenes"] = new SelectList(_context.Jovenes, "Id", "nombre", incidente.id_joven);
            return View(incidente);
        }

        // GET: Incidentes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var incidente = await _context.Incidentes.FindAsync(id);
            if (incidente == null)
            {
                return NotFound();
            }
            ViewData["Usuarios"] = new SelectList(_context.Users, "Id", "nombre_usuario", incidente.id_usuario);
            ViewData["Jovenes"] = new SelectList(_context.Jovenes, "Id", "nombre", incidente.id_joven);
            return View(incidente);
        }

        // POST: Incidentes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,id_usuario,id_joven,fecha_hora,descripcion")] Incidentes incidente)
        {
            if (id != incidente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incidente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncidenteExists(incidente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Usuarios"] = new SelectList(_context.Users, "Id", "nombre_usuario", incidente.id_usuario);
            ViewData["Jovenes"] = new SelectList(_context.Jovenes, "Id", "nombre", incidente.id_joven);
            return View(incidente);
        }

        // GET: Incidentes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var incidente = await _context.Incidentes
                .Include(i => i.Usuario)
                .Include(i => i.Joven)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (incidente == null)
            {
                return NotFound();
            }

            return View(incidente);
        }

        // POST: Incidentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var incidente = await _context.Incidentes.FindAsync(id);
                if (incidente == null)
                {
                    return RedirectToAction(nameof(Index), new { errorMessage = "El incidente no se encontró." });
                }

                incidente.estado = "Desactivado"; // Cambiar el estado a "Desactivado"
                _context.Update(incidente); // Actualizar el estado
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                var errorViewModel = new ErrorViewModel
                {
                    StatusCode = 500,
                    Message = "Ocurrió un error al intentar desactivar el incidente.",
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                };

                return View("Error", errorViewModel);
            }
        }




        private bool IncidenteExists(int id)
        {
            return _context.Incidentes.Any(e => e.Id == id);
        }
    }
}
