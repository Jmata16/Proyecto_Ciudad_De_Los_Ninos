using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Ciudad_De_Los_Ninos.Models;
using Proyecto_Ciudad_De_Los_Ninos.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    [Authorize]
    public class CitasController : Controller
    {
        private readonly ApplicationDBContext _context;

        public CitasController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: CitasController
        public async Task<IActionResult> Index()
        {
            var citas = await _context.Citas
                .Include(c => c.Usuario)
                .Include(c => c.Joven)
                .ToListAsync();

            return View(citas);
        }


        // GET: CitasController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var cita = await _context.Citas
                .Include(c => c.Usuario)
                .Include(c => c.Joven)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }
        // GET: Citas/Create
        public async Task<IActionResult> Create()
        {

            ViewData["Users"] = new SelectList(_context.Users, "Id", "nombre_usuario");
            ViewData["Jovenes"] = new SelectList(_context.Jovenes, "Id", "nombre");

            return View();
        }



        // POST: Citas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,id_usuario,id_joven,fecha,tipo_usuario,detalles")]Citas cita)
        {
            if (ModelState.IsValid)
            {
                // Verificar si ya existe una cita para el usuario o joven en la misma fecha y hora
                var existingCita = await _context.Citas
                    .FirstOrDefaultAsync(c => (c.id_usuario == cita.id_usuario || c.id_joven == cita.id_joven) && c.fecha == cita.fecha);

                if (existingCita != null)
                {
                    ModelState.AddModelError("", "Ya existe una cita programada para esta fecha y hora para este usuario o joven.");
                    ViewData["Users"] = new SelectList(_context.Users, "Id", "nombre_usuario", cita.id_usuario);
                    ViewData["Jovenes"] = new SelectList(_context.Jovenes, "Id", "nombre", cita.id_joven);
                    return View(cita);
                }

                _context.Add(cita);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Si el modelo no es válido, volver a cargar los SelectList
            ViewData["Users"] = new SelectList(_context.Users, "Id", "nombre_usuario", cita.id_usuario);
            ViewData["Jovenes"] = new SelectList(_context.Jovenes, "Id", "nombre", cita.id_joven);
            return View(cita);
        }

        // GET: CitasController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null)
            {
                return NotFound();
            }
            ViewData["Users"] = new SelectList(_context.Users, "Id", "nombre_usuario", cita.id_usuario);
            ViewData["Jovenes"] = new SelectList(_context.Jovenes, "Id", "nombre", cita.id_joven);
            return View(cita);
        }

        // POST: CitasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,id_usuario,id_joven,fecha,tipo_usuario,detalles")] Citas cita)
        {
            if (id != cita.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCita = await _context.Citas
                        .FirstOrDefaultAsync(c => (c.id_usuario == cita.id_usuario || c.id_joven == cita.id_joven) && c.fecha == cita.fecha && c.Id != id);

                    if (existingCita != null)
                    {
                        ModelState.AddModelError("", "Ya existe una cita programada para esta fecha y hora para este usuario o joven.");
                        return View(cita);
                    }

                    _context.Update(cita);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitaExists(cita.Id))
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
            ViewData["Users"] = new SelectList(_context.Users, "ID", "nombre_usuario", cita.id_usuario);
            ViewData["Jovenes"] = new SelectList(_context.Jovenes, "ID", "nombre", cita.id_joven);
            return View(cita);
        }

        // GET: CitasController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var cita = await _context.Citas
               .Include(c => c.Usuario)
               .Include(c => c.Joven)
               .FirstOrDefaultAsync(m => m.Id == id);
            cita = await _context.Citas.FindAsync(id);
            if (cita == null)
            {
                return NotFound();
            }
            return View(cita);
        }

        // POST: CitasController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CitaExists(int id)
        {
            return _context.Citas.Any(e => e.Id == id);
        }
    }
}
