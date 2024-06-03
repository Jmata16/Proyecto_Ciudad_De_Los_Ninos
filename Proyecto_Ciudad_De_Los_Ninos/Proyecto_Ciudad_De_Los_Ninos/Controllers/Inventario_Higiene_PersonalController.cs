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
    public class Inventario_Higiene_PersonalController : Controller
    {
        private readonly ApplicationDBContext _context;

        public Inventario_Higiene_PersonalController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Inventario_Higiene_Personal
        public async Task<IActionResult> Index()
        {
            return View(await _context.Inventario_Higiene_Personal.ToListAsync());
        }

        // GET: Inventario_Higiene_Personal/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario_Higiene_Personal = await _context.Inventario_Higiene_Personal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventario_Higiene_Personal == null)
            {
                return NotFound();
            }

            return View(inventario_Higiene_Personal);
        }

        // GET: Inventario_Higiene_Personal/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,nombre_producto,cantidad_disponible,fecha_ultima_reposicion,proveedor")] Inventario_Higiene_Personal inventario_Higiene_Personal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventario_Higiene_Personal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inventario_Higiene_Personal);
        }

        // GET: Inventario_Higiene_Personal/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario_Higiene_Personal = await _context.Inventario_Higiene_Personal.FindAsync(id);
            if (inventario_Higiene_Personal == null)
            {
                return NotFound();
            }
            return View(inventario_Higiene_Personal);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,nombre_producto,cantidad_disponible,fecha_ultima_reposicion,proveedor")] Inventario_Higiene_Personal inventario_Higiene_Personal)
        {
            if (id != inventario_Higiene_Personal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventario_Higiene_Personal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Inventario_Higiene_PersonalExists(inventario_Higiene_Personal.Id))
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
            return View(inventario_Higiene_Personal);
        }

        // GET: Inventario_Higiene_Personal/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario_Higiene_Personal = await _context.Inventario_Higiene_Personal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventario_Higiene_Personal == null)
            {
                return NotFound();
            }

            return View(inventario_Higiene_Personal);
        }

        // POST: Inventario_Higiene_Personal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventario_Higiene_Personal = await _context.Inventario_Higiene_Personal.FindAsync(id);
            if (inventario_Higiene_Personal != null)
            {
                _context.Inventario_Higiene_Personal.Remove(inventario_Higiene_Personal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Inventario_Higiene_PersonalExists(int id)
        {
            return _context.Inventario_Higiene_Personal.Any(e => e.Id == id);
        }
    }
}
