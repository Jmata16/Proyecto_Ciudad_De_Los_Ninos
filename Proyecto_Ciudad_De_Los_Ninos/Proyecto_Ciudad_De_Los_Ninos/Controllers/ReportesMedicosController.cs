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
    [Authorize(Policy = "Rol1234")]
    public class ReportesMedicosController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ReportesMedicosController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: ReportesMedicos
        public async Task<IActionResult> Index()
        {
            var reportesMedicos = await _context.Reportes_Medicos
                .Include(r => r.Usuario)
                .Include(r => r.Joven)
                .Where(r => r.estado == "Activo")  
                .ToListAsync();

            return View(reportesMedicos);
        }

        public async Task<IActionResult> Desactivado()
        {
            var reportesMedicos = await _context.Reportes_Medicos
                .Include(r => r.Usuario)
                .Include(r => r.Joven)
                .Where(r => r.estado == "Desactivado")
                .ToListAsync();

            return View(reportesMedicos);
        }
        // GET: ReportesMedicos/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var reporteMedico = await _context.Reportes_Medicos
                .Include(r => r.Usuario)
                .Include(r => r.Joven)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reporteMedico == null)
            {
                return NotFound();
            }

            return View(reporteMedico);
        }

        // GET: ReportesMedicos/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Usuarios"] = new SelectList(_context.Users.Where(user => user.id_rol != 4 && user.id_rol != 5).ToList(), "Id", "nombre_usuario");
            ViewData["Jovenes"] = new SelectList(_context.Jovenes, "Id", "nombre");
            return View();
        }

        // POST: ReportesMedicos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,id_usuario,id_joven,fecha_creacion,contenido")] Reportes_Medicos reporteMedico)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reporteMedico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Usuarios"] = new SelectList(_context.Users, "Id", "nombre_usuario", reporteMedico.id_usuario);
            ViewData["Jovenes"] = new SelectList(_context.Jovenes, "Id", "nombre", reporteMedico.id_joven);
            return View(reporteMedico);
        }

        // GET: ReportesMedicos/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var reporteMedico = await _context.Reportes_Medicos.FindAsync(id);
            if (reporteMedico == null)
            {
                return NotFound();
            }
            ViewData["Usuarios"] = new SelectList(_context.Users, "Id", "nombre_usuario", reporteMedico.id_usuario);
            ViewData["Jovenes"] = new SelectList(_context.Jovenes, "Id", "nombre", reporteMedico.id_joven);
            return View(reporteMedico);
        }

        // POST: ReportesMedicos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,id_usuario,id_joven,fecha_creacion,contenido")] Reportes_Medicos reporteMedico)
        {
            if (id != reporteMedico.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reporteMedico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReporteMedicoExists(reporteMedico.Id))
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
            ViewData["Usuarios"] = new SelectList(_context.Users, "Id", "nombre_usuario", reporteMedico.id_usuario);
            ViewData["Jovenes"] = new SelectList(_context.Jovenes, "Id", "nombre", reporteMedico.id_joven);
            return View(reporteMedico);
        }

        // GET: ReportesMedicos/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var reporteMedico = await _context.Reportes_Medicos
                .Include(r => r.Usuario)
                .Include(r => r.Joven)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reporteMedico == null)
            {
                return NotFound();
            }

            return View(reporteMedico);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var reporteMedico = await _context.Reportes_Medicos.FindAsync(id);
                if (reporteMedico == null)
                {
                    return RedirectToAction(nameof(Index), new { errorMessage = "El reporte médico no se encontró." });
                }

                reporteMedico.estado = "Desactivado";
                _context.Update(reporteMedico);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                var errorViewModel = new ErrorViewModel
                {
                    StatusCode = 500,
                    Message = "Ocurrió un error al intentar desactivar el reporte médico.",
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                };

                return View("Error", errorViewModel);
            }
        }



        private bool ReporteMedicoExists(int id)
        {
            return _context.Reportes_Medicos.Any(e => e.Id == id);
        }
    }
}
