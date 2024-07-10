using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Ciudad_De_Los_Ninos.Models;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    public class CapacitacionesController : Controller
    {
        private readonly ApplicationDBContext _context;

        public CapacitacionesController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Capacitaciones
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.Capacitaciones.Include(c => c.User);
            return View(await applicationDBContext.ToListAsync());
        }

        // GET: Capacitaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var capacitaciones = await _context.Capacitaciones
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (capacitaciones == null)
            {
                return NotFound();
            }

            return View(capacitaciones);
        }

        // GET: Capacitaciones/Create
        public IActionResult Create()
        {
            ViewData["id_usuario"] = new SelectList(_context.Users.Select(u => new { u.Id, FullName = u.nombre + " " + u.apellidos }), "Id", "FullName");
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,id_usuario,nombre_capacitacion,fecha,descripcion")] Capacitaciones capacitaciones)
        {
            if (ModelState.IsValid)
            {
                _context.Add(capacitaciones);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            var selectedUserId = capacitaciones.id_usuario;


            ViewData["id_usuario"] = new SelectList(_context.Users.Select(u => new {
                Id = u.Id,
                FullName = u.nombre + " " + u.apellidos
            }), "Id", "FullName", selectedUserId);

            return View(capacitaciones);
        }


        // GET: Capacitaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var capacitaciones = await _context.Capacitaciones.FindAsync(id);
            if (capacitaciones == null)
            {
                return NotFound();
            }

            // Recuperar el usuario seleccionado para mantener la selección en caso de error
            var selectedUserId = capacitaciones.id_usuario;

            // Construir el SelectList con nombres completos de usuarios
            ViewData["id_usuario"] = new SelectList(_context.Users.Select(u => new {
                Id = u.Id,
                FullName = u.nombre + " " + u.apellidos
            }), "Id", "FullName", selectedUserId);

            return View(capacitaciones);
        }

        // POST: Capacitaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,id_usuario,nombre_capacitacion,fecha,descripcion")] Capacitaciones capacitaciones)
        {
            if (id != capacitaciones.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                    _context.Update(capacitaciones);
                    await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }


            return View(capacitaciones);
        }



        // GET: Capacitacionestest/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var capacitaciones = await _context.Capacitaciones
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (capacitaciones == null)
            {
                return NotFound();
            }

            return View(capacitaciones);
        }

        // POST: Capacitacionestest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var capacitaciones = await _context.Capacitaciones.FindAsync(id);
            if (capacitaciones != null)
            {
                _context.Capacitaciones.Remove(capacitaciones);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CapacitacionesExists(int id)
        {
            return _context.Capacitaciones.Any(e => e.Id == id);
        }
    }
}

