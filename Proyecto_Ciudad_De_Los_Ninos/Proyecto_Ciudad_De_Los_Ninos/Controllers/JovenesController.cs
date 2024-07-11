using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using API_Ciudad_De_Los_Ninos.Models;
using Proyecto_Ciudad_De_Los_Ninos.Models;
using Microsoft.AspNetCore.Authorization;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    [Authorize]
    public class JovenesController : Controller
    {
        private readonly ApplicationDBContext _context;

        public JovenesController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Jovenes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Jovenes.ToListAsync());
        }

        // GET: Jovenes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jovenes = await _context.Jovenes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jovenes == null)
            {
                return NotFound();
            }

            return View(jovenes);
        }

        // GET: Jovenes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jovenes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,cedula,nombre,edad,direccion,telefono_contacto")] Jovenes jovenes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jovenes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jovenes);
        }

        // GET: Jovenes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jovenes = await _context.Jovenes.FindAsync(id);
            if (jovenes == null)
            {
                return NotFound();
            }
            return View(jovenes);
        }

        // POST: Jovenes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,cedula,nombre,edad,direccion,telefono_contacto")] Jovenes jovenes)
        {
            if (id != jovenes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jovenes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JovenesExists(jovenes.Id))
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
            return View(jovenes);
        }

        // GET: Jovenes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jovenes = await _context.Jovenes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jovenes == null)
            {
                return NotFound();
            }

            return View(jovenes);
        }

        // POST: Jovenes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jovenes = await _context.Jovenes.FindAsync(id);
            if (jovenes != null)
            {
                _context.Jovenes.Remove(jovenes);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JovenesExists(int id)
        {
            return _context.Jovenes.Any(e => e.Id == id);
        }
    }
}
