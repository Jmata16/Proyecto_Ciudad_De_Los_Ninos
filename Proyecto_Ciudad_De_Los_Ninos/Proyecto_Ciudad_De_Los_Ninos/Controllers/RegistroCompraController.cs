﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Ciudad_De_Los_Ninos.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    
    public class RegistroCompraController : Controller
    {
        private readonly ApplicationDBContext _context;

        public RegistroCompraController(ApplicationDBContext context)
        {
            _context = context;
        }
        [Authorize(Policy = "Rol14")]
        public async Task<IActionResult> Index()
        {
            var registroCompras = await _context.RegistroCompra
                .Include(rc => rc.User)
                .Include(rc => rc.Inventario_Higiene)
                .ToListAsync();

            return View(registroCompras);
        }
        [Authorize(Policy = "Rol15")]
        public async Task<IActionResult> MisCompras()
        {
            var userId = User.FindFirstValue("UserId"); // Obtener el ID del usuario autenticado

            var registroCompras = await _context.RegistroCompra
                .Where(rc => rc.UserId == int.Parse(userId))
                .Include(rc => rc.User)
                .Include(rc => rc.Inventario_Higiene)
                .ToListAsync();

            return View(registroCompras);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var registroCompra = await _context.RegistroCompra.FindAsync(id);
            if (registroCompra == null)
            {
                return NotFound();
            }

            registroCompra.estado = "Entregado";
            _context.Update(registroCompra);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
