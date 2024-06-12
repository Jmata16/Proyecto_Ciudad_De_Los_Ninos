﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Ciudad_De_Los_Ninos.Models;
using Proyecto_Ciudad_De_Los_Ninos.Models;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
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
            return View(await _context.Citas.ToListAsync());
        }

        // GET: CitasController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null)
            {
                return NotFound();
            }
            return View(cita);
        }

        // GET: CitasController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CitasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Citas cita)
        {
            if (ModelState.IsValid)
            {
                // Check for duplicate appointment for the same user or young person at the same time
                var existingCita = await _context.Citas
                    .FirstOrDefaultAsync(c => (c.id_usuario == cita.id_usuario || c.id_joven == cita.id_joven) && c.fecha == cita.fecha);

                if (existingCita != null)
                {
                    ModelState.AddModelError("", "Ya existe una cita programada para esta fecha y hora para este usuario o joven.");
                    return View(cita);
                }

                _context.Add(cita);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
            return View(cita);
        }

        // POST: CitasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Citas cita)
        {
            if (id != cita.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Check for duplicate appointment on edit for the same user or young person at the same time
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
            return View(cita);
        }

        // GET: CitasController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
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
