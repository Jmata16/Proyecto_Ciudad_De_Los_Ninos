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
    [Authorize(Policy = "Rol134")]
    public class JovenesController : Controller
    {
        private readonly ApplicationDBContext _context;

        public JovenesController(ApplicationDBContext context)
        {
            _context = context;
        }

       
        public async Task<IActionResult> Index()
        {
            try
            {
                var jovenes = await _context.Jovenes.ToListAsync();
                return View(jovenes);
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


 
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


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,cedula,nombre,edad,direccion,telefono_contacto,Localizacion")] Jovenes jovenes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jovenes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jovenes);
        }



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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,cedula,nombre,edad,direccion,telefono_contacto,Localizacion")] Jovenes jovenes)
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
