using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Ciudad_De_Los_Ninos.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_Ciudad_De_Los_Ninos.Models;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    public class ExpedientesController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ExpedientesController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Expedientes
        public async Task<IActionResult> Index()
        {
            var expedientes = await _context.Expedientes
                .Include(e => e.Joven)
               
                .ToListAsync();

            return View(expedientes);
        }

        // GET: Expedientes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var expediente = await _context.Expedientes
                .Include(e => e.Joven)
                
                .FirstOrDefaultAsync(m => m.Id == id);

            if (expediente == null)
            {
                return NotFound();
            }

            return View(expediente);
        }

        // GET: Expedientes/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Jovenes"] = new SelectList(_context.Jovenes, "Id", "nombre");
            return View();
        }

        // POST: Expedientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,id_joven,nombre_joven,edad,fecha_ingreso,direccion,telefono_contacto,tutor_legal,antecedentes_medicos,historial_academico,notas_adicionales")] Expedientes expediente)
        {
            if (ModelState.IsValid)
            {
                // Verificar si ya existe un expediente para el id_joven
                var existingExpediente = await _context.Expedientes.FirstOrDefaultAsync(e => e.id_joven == expediente.id_joven);

                if (existingExpediente != null)
                {
                    ModelState.AddModelError("id_joven", "Ya existe un expediente para este joven.");
                    ViewData["Jovenes"] = new SelectList(_context.Jovenes, "Id", "nombre", expediente.id_joven);
                    return View(expediente);
                }

                // Si no existe, agregar el expediente al contexto y guardar los cambios
                _context.Add(expediente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Jovenes"] = new SelectList(_context.Jovenes, "Id", "nombre", expediente.id_joven);
            return View(expediente);
        }
        // GET: Expedientes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var expediente = await _context.Expedientes.FindAsync(id);
            if (expediente == null)
            {
                return NotFound();
            }

            ViewData["Jovenes"] = new SelectList(_context.Jovenes, "Id", "nombre", expediente.id_joven);
            return View(expediente);
        }

        // POST: Expedientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,id_joven,nombre_joven,edad,fecha_ingreso,direccion,telefono_contacto,tutor_legal,antecedentes_medicos,historial_academico,notas_adicionales")] Expedientes expediente)
        {
            if (id != expediente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expediente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpedienteExists(expediente.Id))
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

            ViewData["Jovenes"] = new SelectList(_context.Jovenes, "Id", "nombre", expediente.id_joven);
            return View(expediente);
        }

        // GET: Expedientes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var expediente = await _context.Expedientes
                .Include(e => e.Joven)
               
                .FirstOrDefaultAsync(m => m.Id == id);

            if (expediente == null)
            {
                return NotFound();
            }

            return View(expediente);
        }

        // POST: Expedientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expediente = await _context.Expedientes.FindAsync(id);
            _context.Expedientes.Remove(expediente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpedienteExists(int id)
        {
            return _context.Expedientes.Any(e => e.Id == id);
        }
    }
}
