using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API_Ciudad_De_Los_Ninos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Ciudad_De_Los_Ninos.Models;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    [Authorize(Policy = "Rol16")]
    public class Inventario_Higiene_PersonalController : Controller
    {
        private readonly ApplicationDBContext _context;

        public Inventario_Higiene_PersonalController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Inventario_Higiene_Personal
        public async Task<IActionResult> Index()
        {
            return View(await _context.Inventario_Higiene_Personal.ToListAsync());
        }


        public async Task<IActionResult> Tienda()
        {
            return View(await _context.Inventario_Higiene_Personal.ToListAsync());
        }

        // GET: Inventario_Higiene_Personal/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Inventario_Higiene_Personal/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,nombre_producto,cantidad_disponible,fecha_ultima_reposicion,proveedor,imagen,precio_unitario")] Inventario_Higiene_Personal inventario_Higiene_Personal, IFormFile imagen)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (imagen != null && imagen.Length > 0)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                        var extension = Path.GetExtension(imagen.FileName).ToLower();
                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("imagen", "Solo se permiten archivos de imagen en formato PNG o JPEG.");
                            return View(inventario_Higiene_Personal);
                        }

                        using (var memoryStream = new MemoryStream())
                        {
                            await imagen.CopyToAsync(memoryStream);
                            inventario_Higiene_Personal.imagen = memoryStream.ToArray();
                        }
                    }

                    _context.Add(inventario_Higiene_Personal);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error al guardar: {ex.Message}");
                }
            }
            return View(inventario_Higiene_Personal);
        }

        // GET: Inventario_Higiene_Personal/Edit/5
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

        // POST: Inventario_Higiene_Personal/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,nombre_producto,cantidad_disponible,fecha_ultima_reposicion,proveedor,imagen,precio_unitario")] Inventario_Higiene_Personal inventario_Higiene_Personal, IFormFile? newImagen)
        {
            if (id != inventario_Higiene_Personal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (newImagen != null && newImagen.Length > 0)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                        var extension = Path.GetExtension(newImagen.FileName).ToLower();
                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("imagen", "Solo se permiten archivos de imagen en formato PNG o JPEG.");
                            return View(inventario_Higiene_Personal);
                        }

                        using (var memoryStream = new MemoryStream())
                        {
                            await newImagen.CopyToAsync(memoryStream);
                            inventario_Higiene_Personal.imagen = memoryStream.ToArray();
                        }
                    }
                    else
                    {
                        var existingInventario = await _context.Inventario_Higiene_Personal
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

        // GET: Inventario_Higiene_Personal/Details/5
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

        // GET: Inventario_Higiene_Personal/Delete/5
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

        // POST: Inventario_Higiene_Personal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventario_Higiene_Personal = await _context.Inventario_Higiene_Personal.FindAsync(id);
            if (inventario_Higiene_Personal != null)
            {
                var tickete = _context.Tickete.FirstOrDefault(m => m.id_inventario_higiene_personal == id);

                if (tickete != null)
                {

                    _context.Tickete.Remove(tickete);
                    await _context.SaveChangesAsync();

                   
                }

                _context.Inventario_Higiene_Personal.Remove(inventario_Higiene_Personal);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        private bool Inventario_Higiene_PersonalExists(int id)
        {
            return _context.Inventario_Higiene_Personal.Any(e => e.Id == id);
        }








        // Parte nueva para la imagen

        public async Task<IActionResult> GetImage(int id)
        {
            var inventario_Higiene_Personal = await _context.Inventario_Higiene_Personal.FirstOrDefaultAsync(i => i.Id == id);
            if (inventario_Higiene_Personal == null || inventario_Higiene_Personal.imagen == null)
            {
                return NotFound();
            }
            return File(inventario_Higiene_Personal.imagen, "image/png");
        }
    }
}
