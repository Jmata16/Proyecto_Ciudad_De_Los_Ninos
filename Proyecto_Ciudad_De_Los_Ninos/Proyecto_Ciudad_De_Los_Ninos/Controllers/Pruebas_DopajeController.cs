using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Ciudad_De_Los_Ninos.Models;
using Proyecto_Ciudad_De_Los_Ninos.Models;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
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
            return View(await _context.Pruebas_Dopaje.ToListAsync());
        }

        // GET: Pruebas_Dopaje/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pruebas_Dopaje = await _context.Pruebas_Dopaje
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pruebas_Dopaje == null)
            {
                return NotFound();
            }

            return View(pruebas_Dopaje);
        }

        // GET: Pruebas_Dopaje/Create
        public IActionResult Create()
        {
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
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Pruebas_DopajeExists(int id)
        {
            return _context.Pruebas_Dopaje.Any(e => e.Id == id);
        }
    }
}
