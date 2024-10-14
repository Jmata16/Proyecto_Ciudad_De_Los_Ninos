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
using System.Diagnostics;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    [Authorize(Policy = "AdminPolicy")] 
    public class UsersController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly EmailService _emailService;

        public UsersController(ApplicationDBContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            // Filtrar solo los usuarios cuyo estado sea "Activo"
            var users = await _context.Users
                                      .Where(u => u.estado == "Activo")
                                      .ToListAsync();

            ViewData["Roles"] = new SelectList(_context.Roles, "Id", "nombre_rol");

            return View(users);
        }

        public async Task<IActionResult> Desactivado()
        {
            // Filtrar solo los usuarios cuyo estado sea "Activo"
            var users = await _context.Users
                                      .Where(u => u.estado == "Desactivado")
                                      .ToListAsync();

            ViewData["Roles"] = new SelectList(_context.Roles, "Id", "nombre_rol");

            return View(users);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            ViewData["Roles"] = new SelectList(_context.Roles, "Id", "nombre_rol", user.id_rol);
            return View(user);
        }

        public IActionResult Create()
        {
            ViewData["Roles"] = new SelectList(_context.Roles, "Id", "nombre_rol");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,nombre_usuario,nombre,apellidos,correo,fecha_nacimiento,cedula,contraseña,id_rol")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();

                string subject = "¡Bienvenido al Equipo de Ciudad De Los Niños!";
                string body = $@"
        <p>Hola {user.nombre},</p>
        <p>¡Gracias por formar parte de la institución Ciudad De Los Niños!</p>
        <p>Estamos emocionados de tenerte con nosotros.</p>
        <p>A continuación, encontrarás tus detalles de usuario:</p>
        <ul>
            <li><strong>Nombre de usuario:</strong> {user.nombre_usuario}</li>
            <li><strong>Nombre:</strong> {user.nombre}</li>
            <li><strong>Apellidos:</strong> {user.apellidos}</li>
            <li><strong>Correo:</strong> {user.correo}</li>
        </ul>
        <p>Si tienes alguna pregunta, no dudes en contactarnos.</p>
        <p>Saludos,<br>El equipo del Proyecto Ciudad De Los Niños</p>";

                // Enviar el correo electrónico
                _emailService.SendEmail(user.correo, subject, body);

                return RedirectToAction(nameof(Index));
            }

            ViewData["Roles"] = new SelectList(_context.Roles, "Id", "nombre_rol", user.id_rol);
            return View(user);
        }



        // GET: Editar Usuario
        public async Task<IActionResult> Edit(int? id)
        {
            // Cargar los roles para el dropdown
            ViewData["Roles"] = new SelectList(_context.Roles, "Id", "nombre_rol");

            if (id == null)
            {
                return NotFound();
            }

            // Buscar el usuario por su ID
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Mostrar la vista con los datos del usuario
            return View(user);
        }

        // POST: Editar Usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,nombre_usuario,nombre,apellidos,correo,fecha_nacimiento,cedula,contraseña,id_rol")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Actualizar el usuario en el contexto
                    _context.Update(user);
                    // Guardar los cambios en la base de datos
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Manejar errores de concurrencia aquí si es necesario
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Si ModelState no es válido, cargar los roles nuevamente para la vista
            ViewData["Roles"] = new SelectList(_context.Roles, "Id", "nombre_rol", user.id_rol);
            return View(user);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["Roles"] = new SelectList(_context.Roles, "Id", "nombre_rol");
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // Acción para confirmar la desactivación del usuario
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "El usuario no se encontró o ya ha sido eliminado.";
                    return RedirectToAction(nameof(Index));
                }

                // En lugar de eliminar el usuario, se desactiva cambiando el estado
                user.estado = "Desactivado";

                // Actualiza los cambios en la base de datos
                _context.Update(user);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "El usuario se desactivó correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var errorViewModel = new ErrorViewModel
                {
                    StatusCode = 500,
                    Message = "Ocurrió un error al intentar desactivar el usuario. Por favor, inténtelo de nuevo más tarde.",
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                };

                return View("Error", errorViewModel);
            }
        }


        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
