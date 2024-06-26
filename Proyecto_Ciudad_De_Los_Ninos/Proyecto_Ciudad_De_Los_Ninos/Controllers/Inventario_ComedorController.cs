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
            return View(await _context.Inventario_Comedor.ToListAsync());
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,nombre_alimento,cantidad_disponible,fecha_ultima_reposicion,proveedor,imagen")] Inventario_Comedor inventario_Comedor)
        {
           

                if (id != inventario_Comedor.Id)
                {
                    return NotFound();
                }        

                if (ModelState.IsValid)
                {              
                    try
                    {
                        _context.Update(inventario_Comedor);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!Inventario_ComedorExists(inventario_Comedor.Id))
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
                return View(inventario_Comedor);
            }
        

        public async Task<IActionResult> Details(int? id)
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventario_Comedor = await _context.Inventario_Comedor.FindAsync(id);
            if (inventario_Comedor != null)
            {
                _context.Inventario_Comedor.Remove(inventario_Comedor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
