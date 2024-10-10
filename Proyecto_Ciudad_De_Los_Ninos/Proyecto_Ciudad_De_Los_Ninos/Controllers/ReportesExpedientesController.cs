using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Ciudad_De_Los_Ninos.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_Ciudad_De_Los_Ninos.Models;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    [Authorize(Policy = "Rol134")]
    public class ReportesExpedientesController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ReportesExpedientesController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: ReportesExpedientes
        public async Task<IActionResult> Index()
        {
            var reportesExpedientes = await _context.Reportes_Expedientes
                .Include(r => r.Expedientes)
                .Include(r => r.Usuario)
                .Where(r => r.estado == "Activo") // Filtrar solo registros activos
                .ToListAsync();

            return View(reportesExpedientes);
        }
        // GET: ReportesExpedientes/Desactivado
        public async Task<IActionResult> Desactivado()
        {
            var reportesDesactivados = await _context.Reportes_Expedientes
                .Include(r => r.Expedientes)
                .Include(r => r.Usuario)
                .Where(r => r.estado == "Desactivado") // Filtrar solo registros desactivados
                .ToListAsync();

            return View(reportesDesactivados);
        }


        // GET: ReportesExpedientes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var reporteExpediente = await _context.Reportes_Expedientes
                .Include(r => r.Expedientes)
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reporteExpediente == null)
            {
                return NotFound();
            }

            return View(reporteExpediente);
        }

        // GET: ReportesExpedientes/Create
        public IActionResult Create()
        {
            ViewData["Expedientes"] = new SelectList(_context.Expedientes, "Id", "nombre_joven");
            ViewData["Usuarios"] = new SelectList(_context.Users, "Id", "nombre_usuario");
            return View();
        }

        // POST: ReportesExpedientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,id_expediente,id_usuario,tipo,contenido,fecha_creacion")] Reportes_Expedientes reporteExpediente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reporteExpediente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Expedientes"] = new SelectList(_context.Expedientes, "Id", "nombre_joven", reporteExpediente.id_expediente);
            ViewData["Usuarios"] = new SelectList(_context.Users, "Id", "nombre_usuario", reporteExpediente.id_usuario);
            return View(reporteExpediente);
        }

        // GET: ReportesExpedientes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var reporteExpediente = await _context.Reportes_Expedientes.FindAsync(id);
            if (reporteExpediente == null)
            {
                return NotFound();
            }
            ViewData["Expedientes"] = new SelectList(_context.Expedientes, "Id", "nombre_joven", reporteExpediente.id_expediente);
            ViewData["Usuarios"] = new SelectList(_context.Users, "Id", "nombre_usuario", reporteExpediente.id_usuario);
            return View(reporteExpediente);
        }

        // POST: ReportesExpedientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,id_expediente,id_usuario,tipo,contenido,fecha_creacion")] Reportes_Expedientes reporteExpediente)
        {
            if (id != reporteExpediente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reporteExpediente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReporteExpedienteExists(reporteExpediente.Id))
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
            ViewData["Expedientes"] = new SelectList(_context.Expedientes, "Id", "nombre_joven", reporteExpediente.id_expediente);
            ViewData["Usuarios"] = new SelectList(_context.Users, "Id", "nombre_usuario", reporteExpediente.id_usuario);
            return View(reporteExpediente);
        }

        // GET: ReportesExpedientes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var reporteExpediente = await _context.Reportes_Expedientes
                .Include(r => r.Expedientes)
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reporteExpediente == null)
            {
                return NotFound();
            }

            return View(reporteExpediente); // Asegúrate de que aquí se pasa el objeto a la vista
        }


        // POST: ReportesExpedientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reporteExpediente = await _context.Reportes_Expedientes.FindAsync(id);

            if (reporteExpediente == null)
            {
                return NotFound();
            }

            // Cambiar el estado a "Desactivado" en lugar de eliminar
            reporteExpediente.estado = "Desactivado";

            // Actualizar el registro en la base de datos
            _context.Update(reporteExpediente);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        private bool ReporteExpedienteExists(int id)
        {
            return _context.Reportes_Expedientes.Any(e => e.Id == id);
        }
    }
}
