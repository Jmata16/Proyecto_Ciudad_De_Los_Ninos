using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Ciudad_De_Los_Ninos.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_Ciudad_De_Los_Ninos.Models;
using Microsoft.AspNetCore.Authorization;

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

        // GET: Incidentes
        public async Task<IActionResult> Index()
        {
            var incidentes = await _context.Incidentes
                .Include(i => i.Usuario)
                .Include(i => i.Joven)
                .ToListAsync();

            return View(incidentes);
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
            var incidente = await _context.Incidentes.FindAsync(id);
            _context.Incidentes.Remove(incidente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IncidenteExists(int id)
        {
            return _context.Incidentes.Any(e => e.Id == id);
        }
    }
}
