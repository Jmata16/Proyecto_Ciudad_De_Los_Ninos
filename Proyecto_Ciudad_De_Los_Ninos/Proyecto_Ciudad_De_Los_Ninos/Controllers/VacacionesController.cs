using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Ciudad_De_Los_Ninos.Models;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    [Authorize(Policy = "AdminPolicy")] 
    public class VacacionesController : Controller
    {
        private readonly ApplicationDBContext _context;

        public VacacionesController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Vacaciones
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.Vacaciones.Include(v => v.User);
            return View(await applicationDBContext.ToListAsync());
        }

        // GET: Vacaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacaciones = await _context.Vacaciones
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (vacaciones == null)
            {
                return NotFound();
            }

            return View(vacaciones);
        }

        // GET: Vacaciones/Create
        public IActionResult Create()
        {
            ViewData["id_usuario"] = new SelectList(_context.Users.Select(u => new { u.Id, FullName = u.nombre + " " + u.apellidos }), "Id", "FullName");
            return View();
        }

        // POST: Vacaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,id_usuario,estado,fechaInicio,fechaFinaliza,justificacion")] Vacaciones vacaciones)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vacaciones);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var selectedUserId = vacaciones.id_usuario;
            ViewData["id_usuario"] = new SelectList(_context.Users.Select(u => new {
                Id = u.Id,
                FullName = u.nombre + " " + u.apellidos
            }), "Id", "FullName", selectedUserId);
            return View(vacaciones);
        }

        // GET: Vacaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacaciones = await _context.Vacaciones.FindAsync(id);
            if (vacaciones == null)
            {
                return NotFound();
            }
            ViewData["id_usuario"] = new SelectList(_context.Users, "Id", "apellidos", vacaciones.id_usuario);
            return View(vacaciones);
        }

        // POST: Vacaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,id_usuario,estado,fechaInicio,fechaFinaliza,justificacion")] Vacaciones vacaciones)
        {
            if (id != vacaciones.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vacaciones);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VacacionesExists(vacaciones.ID))
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
            var selectedUserId = vacaciones.id_usuario;
            ViewData["id_usuario"] = new SelectList(_context.Users.Select(u => new {
                Id = u.Id,
                FullName = u.nombre + " " + u.apellidos
            }), "Id", "FullName", selectedUserId);
            return View(vacaciones);
        }

        // GET: Vacaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacaciones = await _context.Vacaciones
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (vacaciones == null)
            {
                return NotFound();
            }

            return View(vacaciones);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var vacaciones = await _context.Vacaciones.FindAsync(id);
                if (vacaciones == null)
                {
                    TempData["ErrorMessage"] = "El registro de vacaciones no se encontró o ya ha sido eliminado.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Vacaciones.Remove(vacaciones);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "El registro de vacaciones se eliminó correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var errorViewModel = new ErrorViewModel
                {
                    StatusCode = 500,
                    Message = "Ocurrió un error al intentar eliminar el registro de vacaciones. Por favor, inténtelo de nuevo más tarde.",
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                };

                return View("Error", errorViewModel);
            }
        }


        private bool VacacionesExists(int id)
        {
            return _context.Vacaciones.Any(e => e.ID == id);
        }
    }
}
