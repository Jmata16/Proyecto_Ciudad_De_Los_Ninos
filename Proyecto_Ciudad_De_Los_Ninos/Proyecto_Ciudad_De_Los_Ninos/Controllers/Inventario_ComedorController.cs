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
using PagedList.EntityFramework;
using System.Diagnostics;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    [Authorize(Policy = "Rol16")]
    public class Inventario_ComedorController : Controller
    {
        private readonly ApplicationDBContext _context;

        public Inventario_ComedorController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Inventario_Comedor
        public async Task<IActionResult> Index()
        {
            var inventarios = await _context.Inventario_Comedor
                .Where(i => i.estado == "Activo") // Filtrar solo los elementos activos
                .ToListAsync();

            return View(inventarios);
        }

        public async Task<IActionResult> Desactivado()
        {
            var inventariosDesactivados = await _context.Inventario_Comedor
                .Where(i => i.estado == "Desactivado") // Filtrar solo los elementos desactivados
                .ToListAsync();

            return View(inventariosDesactivados);
        }


        public IActionResult Create()
        {
            return View();
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,nombre_alimento,cantidad_disponible,fecha_ultima_reposicion,proveedor,imagen")] Inventario_Comedor inventario_Comedor,IFormFile imagen)
        {
            if (ModelState.IsValid)
            {

                using(var memoryStream = new MemoryStream()) 
                {

                    imagen.CopyTo(memoryStream);
                    inventario_Comedor.imagen = memoryStream.ToArray();
                }




                _context.Add(inventario_Comedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inventario_Comedor);
        }

       
        public async Task<IActionResult> Edit(int? id)
        {


                if (id == null)
            {
                return NotFound();
            }

            var inventario_Comedor = await _context.Inventario_Comedor.FindAsync(id);
            if (inventario_Comedor == null)
            {
                return NotFound();
            }
            return View(inventario_Comedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,nombre_alimento,cantidad_disponible,fecha_ultima_reposicion,proveedor,imagen")] Inventario_Comedor inventario_Comedor, IFormFile? newImagen)
        {
            if (id != inventario_Comedor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (newImagen != null && newImagen.Length > 0)
                    {
                        // Verificar si la imagen es PNG o JPEG
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                        var extension = Path.GetExtension(newImagen.FileName).ToLower();
                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("newImagen", "Solo se permiten archivos de imagen en formato PNG o JPEG.");
                            return View(inventario_Comedor);
                        }

                        using (var memoryStream = new MemoryStream())
                        {
                            await newImagen.CopyToAsync(memoryStream);
                            inventario_Comedor.imagen = memoryStream.ToArray();
                        }
                    }
                    else
                    {
                        // Si newImagen está vacío, asignar la imagen actual a newImagen
                        if (newImagen == null)
                        {
                            newImagen = new FormFile(new MemoryStream(inventario_Comedor.imagen), 0, inventario_Comedor.imagen.Length, "imagen", "imagen.png");
                        }
                    }

                    _context.Update(inventario_Comedor);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!Inventario_ComedorExists(inventario_Comedor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "La operación de actualización ha fallado debido a un problema de concurrencia.");
                        return View(inventario_Comedor);
                    }
                }
            }
            return View(inventario_Comedor);
        }



        public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var inventarioComedor = await _context.Inventario_Comedor
            .FirstOrDefaultAsync(m => m.Id == id);
        if (inventarioComedor == null)
        {
            return NotFound();
        }

        return View(inventarioComedor);
    }

        // GET: Inventario_Comedor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario_Comedor = await _context.Inventario_Comedor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventario_Comedor == null)
            {
                return NotFound();
            }

            return View(inventario_Comedor);
        }

        // POST: Inventario_Comedor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var inventario_Comedor = await _context.Inventario_Comedor.FindAsync(id);
                if (inventario_Comedor == null)
                {
                    return RedirectToAction(nameof(Index), new { errorMessage = "El inventario de comedor no se encontró." });
                }

                // Cambiar el estado a "Desactivado"
                inventario_Comedor.estado = "Desactivado";
                _context.Update(inventario_Comedor);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                var errorViewModel = new ErrorViewModel
                {
                    StatusCode = 500,
                    Message = "Ocurrió un error al intentar desactivar el inventario de comedor.",
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                };

                return View("Error", errorViewModel);
            }
        }




        private bool Inventario_ComedorExists(int id)
        {
            return _context.Inventario_Comedor.Any(e => e.Id == id);
        }


     //Parte nueva de la Imagen



        public async Task<IActionResult>GetImage(int id){

            var inventario_Comedor = await _context.Inventario_Comedor.FirstOrDefaultAsync(i => i.Id == id);

            if (inventario_Comedor == null)
            {
                return NotFound();
            }

            return File(inventario_Comedor.imagen,"image/png");

        }




         




    }
}
