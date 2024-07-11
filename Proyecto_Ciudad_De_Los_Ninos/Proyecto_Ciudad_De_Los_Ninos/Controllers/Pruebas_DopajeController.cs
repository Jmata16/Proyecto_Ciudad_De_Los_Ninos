﻿using System;
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
    public class Pruebas_DopajeController : Controller
    {
        private readonly ApplicationDBContext _context;

        public Pruebas_DopajeController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Pruebas_Dopaje
        public async Task<IActionResult> Index()
        {
            var pruebasDopaje = _context.Pruebas_Dopaje
                .Include(p => p.Usuario)
                .Include(p => p.Joven);

            return View(await pruebasDopaje.ToListAsync());
        }

        // GET: Pruebas_Dopaje/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pruebasDopaje = await _context.Pruebas_Dopaje
                .Include(p => p.Usuario)
                .Include(p => p.Joven)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pruebasDopaje == null)
            {
                return NotFound();
            }

            return View(pruebasDopaje);
        }

        // GET: Pruebas_Dopaje/Create
        public IActionResult Create()
        {
            // Cargar usuarios y jóvenes en ViewBag
            ViewBag.Usuarios = new SelectList(_context.Users, "Id", "nombre_usuario");
            ViewBag.Jovenes = new SelectList(_context.Jovenes, "Id", "nombre");
            return View();
        }

        // POST: Pruebas_Dopaje/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,id_usuario,id_joven,fecha,lugar,resultado,observaciones")] Pruebas_Dopaje pruebas_Dopaje)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pruebas_Dopaje);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Si hay errores de validación, recargar las listas de ViewBag
            ViewBag.Usuarios = new SelectList(_context.Users, "Id", "nombre_usuario");
            ViewBag.Jovenes = new SelectList(_context.Jovenes, "Id", "nombre");
            return View(pruebas_Dopaje);
        }

        // GET: Pruebas_Dopaje/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pruebas_Dopaje = await _context.Pruebas_Dopaje.FindAsync(id);
            if (pruebas_Dopaje == null)
            {
                return NotFound();
            }

            // Cargar usuarios y jóvenes en ViewBag
            ViewBag.Usuarios = new SelectList(_context.Users, "Id", "nombre_usuario");
            ViewBag.Jovenes = new SelectList(_context.Jovenes, "Id", "nombre");
            return View(pruebas_Dopaje);
        }

        // POST: Pruebas_Dopaje/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,id_usuario,id_joven,fecha,lugar,resultado,observaciones")] Pruebas_Dopaje pruebas_Dopaje)
        {
            if (id != pruebas_Dopaje.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pruebas_Dopaje);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Pruebas_DopajeExists(pruebas_Dopaje.Id))
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

            // Si hay errores de validación, recargar las listas de ViewBag
            ViewBag.Usuarios = new SelectList(_context.Users, "Id", "nombre_usuario");
            ViewBag.Jovenes = new SelectList(_context.Jovenes, "Id", "nombre");
            return View(pruebas_Dopaje);
        }
        // GET: Pruebas_Dopaje/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pruebas_Dopaje = await _context.Pruebas_Dopaje
                .Include(p => p.Usuario)
                .Include(p => p.Joven)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pruebas_Dopaje == null)
            {
                return NotFound();
            }

            return View(pruebas_Dopaje);
        }

        // POST: Pruebas_Dopaje/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pruebas_Dopaje = await _context.Pruebas_Dopaje.FindAsync(id);
            if (pruebas_Dopaje != null)
            {
                _context.Pruebas_Dopaje.Remove(pruebas_Dopaje);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool Pruebas_DopajeExists(int id)
        {
            return _context.Pruebas_Dopaje.Any(e => e.Id == id);
        }
    }
}