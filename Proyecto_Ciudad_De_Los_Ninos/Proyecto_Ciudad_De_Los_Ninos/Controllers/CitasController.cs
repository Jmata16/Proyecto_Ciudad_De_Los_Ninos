using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Ciudad_De_Los_Ninos.Models;
using Proyecto_Ciudad_De_Los_Ninos.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    [Authorize(Policy = "Rol134")]
    public class CitasController : Controller
    {
        private readonly ApplicationDBContext _context;

        public CitasController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: CitasController
        public async Task<IActionResult> Index()
        {
            var citas = await _context.Citas
                .Include(c => c.Usuario)
                .Include(c => c.Joven)
                .Where(c => c.Estado == "Activo") // Filtrar solo citas activas
                .ToListAsync();

            return View(citas);
        }

        public async Task<IActionResult> Desactivado()
        {
            var citas = await _context.Citas
                .Include(c => c.Usuario)
                .Include(c => c.Joven)
                .Where(c => c.Estado == "Desactivado")
                .ToListAsync();

            return View(citas);
        }

        // GET: CitasController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var cita = await _context.Citas
                .Include(c => c.Usuario)
                .Include(c => c.Joven)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }

        // GET: Citas/Create
        public async Task<IActionResult> Create()
        {
            // Filtrar solo usuarios con roles 3 y 4
            var users = await _context.Users
                                      .Where(u => u.id_rol == 3 || u.id_rol == 4)
                                      .Select(u => new
                                      {
                                          u.Id,
                                          u.nombre_usuario, // Asegúrate de que este sea el campo correcto que contiene el nombre
                                          u.id_rol
                                      })
                                      .ToListAsync();

            // En lugar de usar SelectList directamente, pasamos la lista de objetos al ViewBag
            ViewBag.Users = users;

            ViewData["Jovenes"] = new SelectList(await _context.Jovenes.ToListAsync(), "Id", "nombre");

            return View();
        }

        // POST: Citas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,id_usuario,id_joven,fecha,detalles")] Citas cita) // Elimina tipo_usuario
        {
            if (ModelState.IsValid)
            {
                // Verificar si ya existe una cita para el usuario o joven en la misma fecha y hora
                var existingCita = await _context.Citas
                    .FirstOrDefaultAsync(c => (c.id_usuario == cita.id_usuario || c.id_joven == cita.id_joven) && c.fecha == cita.fecha);

                if (existingCita != null)
                {
                    ModelState.AddModelError("", "Ya existe una cita programada para esta fecha y hora para este usuario o joven.");
                    ViewData["Users"] = new SelectList(await _context.Users.Where(u => u.id_rol == 3 || u.id_rol == 4).ToListAsync(), "Id", "nombre_usuario", cita.id_usuario);
                    ViewData["Jovenes"] = new SelectList(await _context.Jovenes.ToListAsync(), "Id", "nombre", cita.id_joven);
                    return View(cita);
                }

                _context.Add(cita);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Si el modelo no es válido, volver a cargar los SelectList
            ViewData["Users"] = new SelectList(await _context.Users.Where(u => u.id_rol == 3 || u.id_rol == 4).ToListAsync(), "Id", "nombre_usuario", cita.id_usuario);
            ViewData["Jovenes"] = new SelectList(await _context.Jovenes.ToListAsync(), "Id", "nombre", cita.id_joven);
            return View(cita);
        }

        // Método para editar una cita
        public async Task<IActionResult> Edit(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null)
            {
                return NotFound();
            }

            // Filtrar usuarios con rol 3 y 4
            var users = await _context.Users
                .Where(u => u.id_rol == 3 || u.id_rol == 4)
                .Select(u => new
                {
                    Id = u.Id,
                    Nombre = u.nombre_usuario, // Asegúrate de que este sea el campo correcto que contiene el nombre
                    RolId = u.id_rol
                })
                .ToListAsync();

            ViewBag.Users = users; // Pasar la lista filtrada a la vista

            // Convertir la lista de jóvenes a un SelectList
            ViewBag.Jovenes = new SelectList(await _context.Jovenes.ToListAsync(), "Id", "nombre");

            return View(cita);
        }

        // Método para actualizar una cita
        [HttpPost]
        public async Task<IActionResult> Edit(Citas cita)
        {
            if (ModelState.IsValid)
            {
                _context.Update(cita);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index"); // Redirigir a la lista de citas después de guardar
            }

            // Si la validación falla, volver a cargar los usuarios
            var users = await _context.Users
                .Where(u => u.id_rol == 3 || u.id_rol == 4)
                .Select(u => new
                {
                    Id = u.Id,
                    Nombre = u.nombre_usuario,
                    RolId = u.id_rol
                })
                .ToListAsync();

            ViewBag.Users = users;
            ViewBag.Jovenes = await _context.Jovenes.ToListAsync();

            return View(cita);
        }

        // GET: CitasController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var cita = await _context.Citas
                .Include(c => c.Usuario)
                .Include(c => c.Joven)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var cita = await _context.Citas.FindAsync(id);
                if (cita == null)
                {
                    return RedirectToAction(nameof(Index), new { errorMessage = "La cita no se encontró." });
                }

                // Cambiar el estado de la cita a "Desactivado"
                cita.Estado = "Desactivado"; // Asegúrate de que la propiedad Estado exista en tu modelo
                _context.Update(cita);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                var errorViewModel = new ErrorViewModel
                {
                    StatusCode = 500,
                    Message = "Ocurrió un error al intentar cambiar el estado de la cita.",
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                };

                return View("Error", errorViewModel);
            }
        }

        private bool CitaExists(int id)
        {
            return _context.Citas.Any(e => e.Id == id);
        }
    }
}
