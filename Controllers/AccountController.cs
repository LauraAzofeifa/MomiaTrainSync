using Microsoft.AspNetCore.Mvc;
using MomiaTrainSync.Data;
using MomiaTrainSync.Models;

namespace MomiaTrainSync.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string correo, string contrasena)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Correo == correo
                                  && u.Contrasena == contrasena
                                  && u.Estado == "Activo");

            if (usuario != null)
            {
                HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
                HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
                HttpContext.Session.SetString("UsuarioRol", usuario.Rol);

                return usuario.Rol switch
                {
                    "Administrador" => RedirectToAction("Dashboard", "Admin"),
                    "Entrenador" => RedirectToAction("Dashboard", "Entrenador"),
                    "Atleta" => RedirectToAction("Dashboard", "Atleta"),
                    _ => RedirectToAction("Index", "Home")
                };
            }

            ViewBag.Error = "Correo, contraseña incorrectos o el usuario está inactivo.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string correo)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Correo == correo);

            if (usuario == null)
            {
                ViewBag.Error = "El correo no está registrado.";
                return View();
            }

            string tempPassword = "Temp1234!";
            usuario.Contrasena = tempPassword;

            _context.Usuarios.Update(usuario);
            _context.SaveChanges();

            ViewBag.Success = "Se ha restablecido su contraseña. Revise su correo para más instrucciones.";
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string nombre, string correo, string contrasena)
        {
            if (_context.Usuarios.Any(u => u.Correo == correo))
            {
                ViewBag.Error = "El correo ya está registrado.";
                return View();
            }

            var usuario = new Usuario
            {
                Nombre = nombre,
                Correo = correo,
                Contrasena = contrasena,
                Rol = "Atleta",
                Estado = "Activo"
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return RedirectToAction("Login", "Account");
        }
    }
}
