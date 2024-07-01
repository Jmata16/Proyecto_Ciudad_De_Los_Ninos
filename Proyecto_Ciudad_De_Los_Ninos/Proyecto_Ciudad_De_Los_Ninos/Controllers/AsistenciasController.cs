using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Ciudad_De_Los_Ninos.Models;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    public class AsistenciasController : Controller
    {
        private readonly ApplicationDBContext _context;

        public AsistenciasController(ApplicationDBContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(DateTime? fecha)
        {
            var asistenciasQuery = _context.Asistencia
                .Include(a => a.User)
                .OrderByDescending(a => a.fecha)
                .AsQueryable();

            if (fecha.HasValue)
            {
                asistenciasQuery = asistenciasQuery.Where(a => a.fecha.Date == fecha.Value.Date);
            }

            var asistencias = await asistenciasQuery.ToListAsync();
            ViewData["fecha"] = fecha?.ToString("yyyy-MM-dd");
            return View(asistencias);
        }




        public IActionResult Create()
        {
            ViewData["id_usuario"] = new SelectList(_context.Users.Select(u => new { u.Id, FullName = u.nombre + " " + u.apellidos }), "Id", "FullName");
            return View(new Asistencia { fecha = DateTime.Today, horaRegistro = DateTime.Now });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,id_usuario,estado,fecha,horaRegistro,horaSalida,justificacion")] Asistencia asistencia)
        {
            if (ModelState.IsValid)
            {
               
                var existingAsistencia = await _context.Asistencia
                    .FirstOrDefaultAsync(a => a.id_usuario == asistencia.id_usuario && a.fecha == asistencia.fecha);

                if (existingAsistencia != null)
                {
                    ModelState.AddModelError(string.Empty, "Ya existe una asistencia registrada para este usuario en la fecha actual.");
                    ViewData["id_usuario"] = new SelectList(_context.Users, "Id", "apellidos", asistencia.id_usuario);
                    return View(asistencia);
                }

               
                asistencia.horaRegistro = DateTime.Now;

                
                if (asistencia.horaSalida == default(DateTime))
                {
                    asistencia.horaSalida = null;
                }

                
                _context.Add(asistencia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var selectedUserId = asistencia.id_usuario;
            ViewData["id_usuario"] = new SelectList(_context.Users.Select(u => new {
                Id = u.Id,
                FullName = u.nombre + " " + u.apellidos
            }), "Id", "FullName", selectedUserId);
            return View(asistencia);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistencia = await _context.Asistencia
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.ID == id);

            if (asistencia == null)
            {
                return NotFound();
            }

            var selectedUserId = asistencia.id_usuario;

           
            ViewData["id_usuario"] = new SelectList(_context.Users.Select(u => new {
                Id = u.Id,
                FullName = u.nombre + " " + u.apellidos
            }), "Id", "FullName", selectedUserId);

            return View(asistencia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,id_usuario,estado,fecha,horaRegistro,horaSalida,justificacion")] Asistencia asistencia)
        {
            if (id != asistencia.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(asistencia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Asistencia.Any(e => e.ID == asistencia.ID))
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

            ViewData["id_usuario"] = new SelectList(_context.Users, "Id", "apellidos", asistencia.id_usuario);
            return View(asistencia);
        }





        public async Task<IActionResult> AgregarHoraSalida(int id)
        {
            var asistencia = await _context.Asistencia.FindAsync(id);
            if (asistencia == null)
            {
                return NotFound();
            }

            asistencia.horaSalida = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistencia = await _context.Asistencia
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (asistencia == null)
            {
                return NotFound();
            }

            return View(asistencia);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var asistencia = await _context.Asistencia.FindAsync(id);
            _context.Asistencia.Remove(asistencia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool AsistenciaExists(int id)
        {
            return _context.Asistencia.Any(e => e.ID == id);
        }
    }
}
