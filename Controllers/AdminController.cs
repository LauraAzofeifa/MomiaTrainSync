using Microsoft.AspNetCore.Mvc;

namespace MomiaTrainSync.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard() => View();
        public IActionResult Usuarios() => View();
        public IActionResult CrearUsuario() => View();
        public IActionResult Roles() => View();
        public IActionResult CrearRol() => View();
        public IActionResult Permisos() => View();
        public IActionResult Metricas() => View();

        [HttpGet]
        public IActionResult EditarUsuario(int id)
        {
            
            ViewBag.UsuarioId = id;
            return View();
        }

        [HttpGet]
        public IActionResult EditarRol(int id)
        {
            // Simulación: podrías usar este ID para cargar datos reales en el futuro
            ViewBag.RolId = id;
            return View();
        }
    }
}
