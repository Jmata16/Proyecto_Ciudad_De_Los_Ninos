using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using API_Ciudad_De_Los_Ninos.Models;
using Proyecto_Ciudad_De_Los_Ninos.Models;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
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
            return View(await _context.Incidentes.ToListAsync());
        }

        // GET: Incidentes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidentes = await _context.Incidentes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (incidentes == null)
            {
                return NotFound();
            }

            return View(incidentes);
        }

        // GET: Incidentes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Incidentes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,id_usuario,id_joven,fecha_hora,descripcion")] Incidentes incidentes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(incidentes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(incidentes);
        }

        // GET: Incidentes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidentes = await _context.Incidentes.FindAsync(id);
            if (incidentes == null)
            {
                return NotFound();
            }
            return View(incidentes);
        }

        // POST: Incidentes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,id_usuario,id_joven,fecha_hora,descripcion")] Incidentes incidentes)
        {
            if (id != incidentes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incidentes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncidentesExists(incidentes.Id))
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
            return View(incidentes);
        }

        // GET: Incidentes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidentes = await _context.Incidentes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (incidentes == null)
            {
                return NotFound();
            }

            return View(incidentes);
        }

        // POST: Incidentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incidentes = await _context.Incidentes.FindAsync(id);
            if (incidentes != null)
            {
                _context.Incidentes.Remove(incidentes);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IncidentesExists(int id)
        {
            return _context.Incidentes.Any(e => e.Id == id);
        }
    }
}
