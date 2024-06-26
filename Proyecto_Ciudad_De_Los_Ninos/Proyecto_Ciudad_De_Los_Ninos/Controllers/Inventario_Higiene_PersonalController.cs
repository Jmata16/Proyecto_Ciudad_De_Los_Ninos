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
    public class Inventario_Higiene_PersonalController : Controller
    {
        private readonly ApplicationDBContext _context;

        public Inventario_Higiene_PersonalController(ApplicationDBContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _context.Inventario_Higiene_Personal.ToListAsync());
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,nombre_producto,cantidad_disponible,fecha_ultima_reposicion,proveedor,imagen,precio_unitario")] Inventario_Higiene_Personal inventario_Higiene_Personal, IFormFile imagen)
        {
            if (ModelState.IsValid)
            {


                using (var memoryStream = new MemoryStream())
                {

                    imagen.CopyTo(memoryStream);
                    inventario_Higiene_Personal.imagen = memoryStream.ToArray();
                }


                _context.Add(inventario_Higiene_Personal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inventario_Higiene_Personal);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario_Higiene_Personal = await _context.Inventario_Higiene_Personal.FindAsync(id);
            if (inventario_Higiene_Personal == null)
            {
                return NotFound();
            }
            return View(inventario_Higiene_Personal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]


        public async Task<IActionResult> Edit(int id, [Bind("Id,nombre_alimento,nombre_producto,cantidad_disponible,fecha_ultima_reposicion,proveedor,imagen,precio_unitario")] Inventario_Higiene_Personal inventario_Higiene_Personal, IFormFile imagen)
        {
            if (id != inventario_Higiene_Personal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (imagen != null && imagen.Length > 0)
                    {
                        // Verificar si la imagen es PNG o JPEG
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                        var extension = Path.GetExtension(imagen.FileName).ToLower();
                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("imagen", "Solo se permiten archivos de imagen en formato PNG o JPEG.");
                            return View(inventario_Higiene_Personal);
                        }

                        using (var memoryStream = new MemoryStream())
                        {
                            imagen.CopyTo(memoryStream);
                            inventario_Higiene_Personal.imagen = memoryStream.ToArray();
                        }
                    }
                    else
                    {
                        var existingInventario = await _context.Inventario_Comedor
                            .AsNoTracking()
                            .FirstOrDefaultAsync(i => i.Id == id);
                        inventario_Higiene_Personal.imagen = existingInventario.imagen;
                    }

                    _context.Update(inventario_Higiene_Personal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Inventario_Higiene_PersonalExists(inventario_Higiene_Personal.Id))
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
            return View(inventario_Higiene_Personal);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario_Higiene_Personal = await _context.Inventario_Higiene_Personal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventario_Higiene_Personal == null)
            {
                return NotFound();
            }

            return View(inventario_Higiene_Personal);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario_Higiene_Personal = await _context.Inventario_Higiene_Personal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventario_Higiene_Personal == null)
            {
                return NotFound();
            }

            return View(inventario_Higiene_Personal);
        }




        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventario_Higiene_Personal = await _context.Inventario_Higiene_Personal.FindAsync(id);
            if (inventario_Higiene_Personal != null)
            {
                _context.Inventario_Higiene_Personal.Remove(inventario_Higiene_Personal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Inventario_Higiene_PersonalExists(int id)
        {
            return _context.Inventario_Higiene_Personal.Any(e => e.Id == id);
        }


        //Parte nueva de la Imagen

        public async Task<IActionResult> GetImage(int id)
        {

            var inventario_Higiene_Personal= await _context.Inventario_Higiene_Personal.FirstOrDefaultAsync(i => i.Id == id);

            if (inventario_Higiene_Personal == null)
            {
                return NotFound();
            }

            return File(inventario_Higiene_Personal.imagen, "image/png");

        }









    }
}

