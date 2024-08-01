using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    [Authorize]
    public class AdministracionDeEmpleadosController : Controller
    {
        public IActionResult Capacitaciones()
        {
            return View();
        }
        public IActionResult AsistenciaPersonal()
        {
            return View();
        }
        public IActionResult Lista()
        {
            return View();
        }
        public IActionResult Vacaciones()
        {
            return View();
        }
        public IActionResult VacacionesTabla()
        {
            return View();
        }
        public IActionResult Usuarios()
        {
            return View();
        }
        public IActionResult UsuariosTablas()
        {
            return View();
        }
    }
}
