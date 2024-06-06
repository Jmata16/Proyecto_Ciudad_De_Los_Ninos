using System.Threading.Tasks;
using API_Ciudad_De_Los_Ninos.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
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
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            if (ModelState.IsValid)
            {
                var loginUser = await _context.Users.FirstOrDefaultAsync(u => u.nombre_usuario == user.nombre_usuario && u.contraseña == user.contraseña);
                if (loginUser != null)
                {
                    var authProperties = new AuthenticationProperties
                    {
                        // AllowRefresh = <bool>,
                        // Refreshing the authentication session should be allowed.
                        // AllowRefresh = true,

                        // ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.

                        // IsPersistent = true,
                        // Whether the authentication session is persisted across
                        // multiple requests. Required when setting the 
                        // ExpiresUtc option of CookieAuthenticationOptions 
                        // set with AddCookie. Also required when setting 
                        // the ExpiresUtc option of AuthenticationProperties 
                        // set with SignInAsync.

                        // IssuedUtc = <DateTimeOffset>,
                        // The time at which the authentication ticket was issued.

                        // RedirectUri = <string>
                        // The full path or absolute URI to be used as an http 
                        // redirect response value.
                    };

                    var claims = new[]
                    {
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.nombre_usuario),
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, loginUser.id_rol.ToString())
                    };

                    var identity = new System.Security.Claims.ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new System.Security.Principal.GenericPrincipal(identity, new[] { "User" });

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new System.Security.Claims.ClaimsPrincipal(principal), authProperties);

                    return RedirectToAction("Index", "Home"); // Redirige a la página de inicio después del inicio de sesión
                }
                ModelState.AddModelError(string.Empty, "Nombre de usuario o contraseña incorrectos.");
            }
            return View(user);
        }

        // GET: /Account/Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home"); // Redirige a la página de inicio después del cierre de sesión
        }
    }
}