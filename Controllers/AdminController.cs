using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Data;
using MomiaTrainSync.Models;
using MomiaTrainSync.Services;

namespace MomiaTrainSync.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public AdminController(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public IActionResult Dashboard() => View();

        public async Task<IActionResult> Usuarios()
        {
            var currentUserId = int.TryParse(HttpContext.Session.GetString("UsuarioId"), out var id) ? id : (int?)null;
          

            var usuarios = await _context.Usuarios
                .Where(u => u.Id != currentUserId)
                .ToListAsync();

            var relaciones = await _context.EntrenadoresAtletas
                .Include(ea => ea.Entrenador)
                .Include(ea => ea.Atleta)
                .ToListAsync();

            foreach (var atleta in usuarios.Where(u => u.Rol == "Atleta"))
            {
                var relacion = relaciones.FirstOrDefault(r => r.AtletaId == atleta.Id);
                if (relacion != null)
                {
                    atleta.Entrenador = relacion.Entrenador;
                }
            }

            return View(usuarios);
        }

        public IActionResult CrearUsuario() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearUsuario(Usuario usuario)
        {
            
            ModelState.Remove(nameof(Usuario.Contrasena));
            ModelState.Remove(nameof(Usuario.Estado));
            ModelState.Remove(nameof(Usuario.FechaRegistro));

            if (!ModelState.IsValid)
                return View(usuario);

            
            var tempPass = "Temp" + Guid.NewGuid().ToString("N").Substring(0, 6) + "!";

            usuario.Contrasena = tempPass;
            usuario.Estado = "Activo";
            usuario.FechaRegistro = DateTime.Now;

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            TempData["SuccessMessage"] = $"Usuario {usuario.Nombre} creado correctamente con contraseña temporal.";
            creacionUsuario(usuario.Correo,tempPass);
            return RedirectToAction("Usuarios");
        }

        public async void creacionUsuario(string correoDestino, string tempPassword)
        {
            await _emailService.SendEmailAsync(
                correoDestino,
                "Bienvenido a MomiaTrainSync",
                "<h3>Se ha creado un usuario en nuestro sistema con tu email. Tu contrasena de acceso es: <b>"
                + tempPassword + "</b>. Recomendamos cambiarla despues de ingresar.</h3>"
            );
        }

        public IActionResult EditarUsuario(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
                return NotFound();

            ViewBag.Entrenadores = _context.Usuarios
                .Where(u => u.Rol == "Entrenador" && u.Estado == "Activo")
                .ToList();

            if (usuario.Rol == "Atleta")
            {
                var relacion = _context.EntrenadoresAtletas
                    .Include(ea => ea.Entrenador)
                    .FirstOrDefault(ea => ea.AtletaId == usuario.Id);

                if (relacion != null)
                {
                    usuario.Entrenador = relacion.Entrenador;
                }
            }

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarUsuario(Usuario usuario, int? EntrenadorId)
        {
            ModelState.Remove(nameof(Usuario.Contrasena));
            ModelState.Remove(nameof(Usuario.FechaRegistro));

            if (!ModelState.IsValid)
                return View(usuario);

            var dbUser = _context.Usuarios.FirstOrDefault(u => u.Id == usuario.Id);
            if (dbUser == null)
                return NotFound();

            dbUser.Nombre = usuario.Nombre;
            dbUser.Correo = usuario.Correo;
            dbUser.Rol = usuario.Rol;
            dbUser.Estado = usuario.Estado;

            _context.Update(dbUser);
            _context.SaveChanges();

            if (dbUser.Rol == "Atleta")
            {
                var relacionExistente = _context.EntrenadoresAtletas
                    .FirstOrDefault(r => r.AtletaId == dbUser.Id);

                if (EntrenadorId.HasValue && EntrenadorId.Value > 0)
                {
                    if (relacionExistente != null)
                    {
                        relacionExistente.EntrenadorId = EntrenadorId.Value;
                        _context.Update(relacionExistente);
                    }
                    else
                    {
                        var nuevaRelacion = new EntrenadorAtleta
                        {
                            EntrenadorId = EntrenadorId.Value,
                            AtletaId = dbUser.Id
                        };
                        _context.Add(nuevaRelacion);
                    }
                }
                else if (relacionExistente != null)
                {
                    _context.EntrenadoresAtletas.Remove(relacionExistente);
                }

                _context.SaveChanges();
            }
            else
            {
                var relaciones = _context.EntrenadoresAtletas
                    .Where(r => r.AtletaId == dbUser.Id)
                    .ToList();

                if (relaciones.Any())
                {
                    _context.EntrenadoresAtletas.RemoveRange(relaciones);
                    _context.SaveChanges();
                }
            }

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
