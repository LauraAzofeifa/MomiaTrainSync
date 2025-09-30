using Microsoft.AspNetCore.Mvc;
using MomiaTrainSync.Data;
using MomiaTrainSync.Models;

namespace MomiaTrainSync.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard() => View();

        public IActionResult Usuarios()
        {
            var usuarios = _context.Usuarios.ToList();
            return View(usuarios);
        }

        public IActionResult CrearUsuario() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearUsuario(Usuario usuario)
        {
            // Quitamos validaciones de campos que se generan automáticamente
            ModelState.Remove(nameof(Usuario.Contrasena));
            ModelState.Remove(nameof(Usuario.Estado));
            ModelState.Remove(nameof(Usuario.FechaRegistro));

            if (!ModelState.IsValid)
                return View(usuario);

            // Autogenerar contraseña temporal
            var tempPass = "Temp" + Guid.NewGuid().ToString("N").Substring(0, 6) + "!";

            usuario.Contrasena = tempPass;
            usuario.Estado = "Activo";
            usuario.FechaRegistro = DateTime.Now;

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            TempData["SuccessMessage"] = $"Usuario {usuario.Nombre} creado correctamente con contraseña temporal.";
            return RedirectToAction("Usuarios");
        }

        public IActionResult EditarUsuario(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarUsuario(Usuario usuario)
        {
           
            ModelState.Remove(nameof(Usuario.Contrasena));
            ModelState.Remove(nameof(Usuario.FechaRegistro));

            if (!ModelState.IsValid)
                return View(usuario);

            var dbUser = _context.Usuarios.FirstOrDefault(u => u.Id == usuario.Id);
            if (dbUser == null) return NotFound();

            dbUser.Nombre = usuario.Nombre;
            dbUser.Correo = usuario.Correo;
            dbUser.Rol = usuario.Rol;
            dbUser.Estado = usuario.Estado;

            _context.Update(dbUser);
            _context.SaveChanges();

            TempData["SuccessMessage"] = $"Usuario {usuario.Nombre} actualizado correctamente.";
            return RedirectToAction("Usuarios");
        }


        public IActionResult Roles() => View();
        public IActionResult CrearRol() => View();
        public IActionResult Permisos() => View();
        public IActionResult Metricas() => View();

        [HttpGet]
        public IActionResult EditarRol(int id)
        {
            ViewBag.RolId = id;
            return View();
        }
    }
}
