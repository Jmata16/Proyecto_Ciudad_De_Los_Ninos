﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Ciudad_De_Los_Ninos.Models;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDBContext _context;

        public LoginController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Buscamos al usuario que coincida con el nombre de usuario y la contraseña
                var loginUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.nombre_usuario == model.nombre_usuario && u.contraseña == model.contraseña);

                // Verificamos si el usuario existe y está activo
                if (loginUser != null)
                {
                    if (loginUser.estado != "Activo") // Cambia "Activo" si el valor es diferente en tu base de datos
                    {
                        ModelState.AddModelError(string.Empty, "Tu cuenta no está activa.");
                        return View(model);
                    }

                    // Crear las reclamaciones para el usuario
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.nombre_usuario),
                new Claim(ClaimTypes.Role, loginUser.id_rol.ToString()),
                new Claim("UserId", loginUser.Id.ToString()),
                new Claim(ClaimTypes.Email, loginUser.correo)
            };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Home"); // Redirige a la página de inicio después del inicio de sesión
                }
                ModelState.AddModelError(string.Empty, "Nombre de usuario o contraseña incorrectos.");
            }
            return View(model);
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


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home"); // Redirige a la página de inicio después del cierre de sesión
        }
    }
}
