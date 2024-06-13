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
    public class Inventario_ComedorController : Controller
    {
        private readonly ApplicationDBContext _context;

        public Inventario_ComedorController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Inventario_Comedor
        public async Task<IActionResult> Index()
        {
            return View(await _context.Inventario_Comedor.ToListAsync());
        }

        // GET: Inventario_Comedor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario_Comedor = await _context.Inventario_Comedor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventario_Comedor == null)
            {
                return NotFound();
            }

            return View(inventario_Comedor);
        }

    
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,nombre_alimento,cantidad_disponible,fecha_ultima_reposicion,proveedor")] Inventario_Comedor inventario_Comedor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventario_Comedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inventario_Comedor);
        }

        // GET: Inventario_Comedor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario_Comedor = await _context.Inventario_Comedor.FindAsync(id);
            if (inventario_Comedor == null)
            {
                return NotFound();
            }
            return View(inventario_Comedor);
        }

        // POST: Inventario_Comedor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,nombre_alimento,cantidad_disponible,fecha_ultima_reposicion,proveedor")] Inventario_Comedor inventario_Comedor)
        {
            if (id != inventario_Comedor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventario_Comedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Inventario_ComedorExists(inventario_Comedor.Id))
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
            return View(inventario_Comedor);
        }

        // GET: Inventario_Comedor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario_Comedor = await _context.Inventario_Comedor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventario_Comedor == null)
            {
                return NotFound();
            }

            return View(inventario_Comedor);
        }

        // POST: Inventario_Comedor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventario_Comedor = await _context.Inventario_Comedor.FindAsync(id);
            if (inventario_Comedor != null)
            {
                _context.Inventario_Comedor.Remove(inventario_Comedor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Inventario_ComedorExists(int id)
        {
            return _context.Inventario_Comedor.Any(e => e.Id == id);
        }
    }
}
