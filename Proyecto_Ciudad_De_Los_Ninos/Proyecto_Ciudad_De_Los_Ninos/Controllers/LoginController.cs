using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult LogIn()
        {
            return View();
        }
    }
}
