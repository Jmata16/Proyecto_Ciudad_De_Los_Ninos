﻿using Microsoft.AspNetCore.Mvc;
using System;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
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
    }
}
