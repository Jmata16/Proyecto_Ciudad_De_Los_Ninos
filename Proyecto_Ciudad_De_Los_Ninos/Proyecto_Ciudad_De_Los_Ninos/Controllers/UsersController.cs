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
            var users = await _context.Users.ToListAsync();
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
            <p>¡Gracias por formar parte de la instirucion Ciudad De Los Niños! 
            <p>Estamos emocionados de tenerte con nosotros.</p>A continuación, encontrarás tus detalles de usuario:</p>
            <ul>
                <li><strong>Nombre de usuario:</strong> {user.nombre_usuario}</li>
                <li><strong>Nombre:</strong> {user.nombre}</li>
                <li><strong>Apellidos:</strong> {user.apellidos}</li>
                <li><strong>Correo:</strong> {user.correo}</li>
            </ul>
            <p>Si tienes alguna pregunta, no dudes en contactarnos.</p>
            <p>Saludos,<br>El equipo del Proyecto Ciudad De Los Niños</p>";

                _emailService.SendEmail(user.correo, subject, body);

                return RedirectToAction(nameof(Index));
            }
            ViewData["Roles"] = new SelectList(_context.Roles, "Id", "nombre_rol", user.id_rol);
            return View(user);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["Roles"] = new SelectList(_context.Roles, "Id", "nombre_rol");
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

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
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
