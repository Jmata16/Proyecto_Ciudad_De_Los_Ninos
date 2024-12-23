﻿using System;
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
    [Authorize(Policy = "Rol134")]
    public class JovenesController : Controller
    {
        private readonly ApplicationDBContext _context;

        public JovenesController(ApplicationDBContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            try
            {
                // Filtrar solo los jóvenes con estado "Activo"
                var jovenes = await _context.Jovenes
                                             .Where(j => j.estado == "Activo") // Filtra por estado activo
                                             .ToListAsync();
                return View(jovenes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        public async Task<IActionResult> Desactivado()
        {
            try
            {
                // Filtrar solo los jóvenes con estado "Desactivado"
                var jovenes = await _context.Jovenes
                                             .Where(j => j.estado == "Desactivado") // Filtra por estado activo
                                             .ToListAsync();
                return View(jovenes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jovenes = await _context.Jovenes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jovenes == null)
            {
                return NotFound();
            }

            return View(jovenes);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,cedula,nombre,edad,direccion,telefono_contacto,Localizacion")] Jovenes jovenes)
        {
            // Verificar si la cédula ya existe en la base de datos
            var existeCedula = await _context.Jovenes.AnyAsync(j => j.cedula == jovenes.cedula);

            if (existeCedula)
            {
                // Si la cédula ya existe, agregar un mensaje de error
                ModelState.AddModelError("cedula", "La cédula ya está registrada.");
                return View(jovenes);
            }

            // Validaciones
            if (jovenes.cedula < 0)
            {
                ModelState.AddModelError("cedula", "La cédula no puede ser negativa.");
                return View(jovenes);
            }

            if (jovenes.direccion.Length > 254)
            {
                ModelState.AddModelError("direccion", "La dirección no puede tener más de 254 caracteres.");
                return View(jovenes);
            }

            if (ModelState.IsValid)
            {
                _context.Add(jovenes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jovenes);
        }
    



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jovenes = await _context.Jovenes.FindAsync(id);
            if (jovenes == null)
            {
                return NotFound();
            }
            return View(jovenes);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,cedula,nombre,edad,direccion,telefono_contacto,Localizacion")] Jovenes jovenes)
        {
            //Validaciones
            if (jovenes.cedula < 0)
            {

                return View();
            }
            if (jovenes.direccion.Length > 254)
            {
                return View();
            }
            if (id != jovenes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jovenes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JovenesExists(jovenes.Id))
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
            return View(jovenes);
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jovenes = await _context.Jovenes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jovenes == null)
            {
                return NotFound();
            }

            return View(jovenes);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var joven = await _context.Jovenes.FindAsync(id);
                if (joven == null)
                {
                    return RedirectToAction(nameof(Index), new { errorMessage = "El registro de jóvenes no se encontró." });
                }

                // Cambiar el estado a "Desactivado"
                joven.estado = "Desactivado"; // Ajusta según cómo manejes los estados
                _context.Jovenes.Update(joven); // Actualiza el registro
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                var errorViewModel = new ErrorViewModel
                {
                    StatusCode = 500,
                    Message = "Ocurrió un error al intentar desactivar el registro de jóvenes.",
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                };

                return View("Error", errorViewModel);
            }
        }



        private bool JovenesExists(int id)
        {
            return _context.Jovenes.Any(e => e.Id == id);
        }
    }
}
