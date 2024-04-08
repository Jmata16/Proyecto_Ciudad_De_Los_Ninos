using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    public class AdministracionDeInventarioController : Controller
    {
        public IActionResult Alimentos()
        {
            return View();
        }

        public IActionResult HigienePersonal()
        {
            return View();
        }

        public IActionResult AgregarProducto()
        {
            return View();
        }

        public IActionResult GuardarProducto()
        {
            return View();
        }
        public IActionResult EliminarProducto()
        {
            return View();
        }
        public IActionResult EditarProducto(int id)
        {
            var producto = (id);
            return View(producto);
        }
    }
}
