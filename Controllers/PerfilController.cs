using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Data;
using MomiaTrainSync.Models;
using MomiaTrainSync.Models.ViewModels;

namespace MomiaTrainSync.Controllers
{
    public class PerfilController : Controller
    {
        private readonly AppDbContext _context;

        public PerfilController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUserId = int.TryParse(HttpContext.Session.GetString("UsuarioId"), out var id) ? id : 0;

            if (currentUserId == 0)
                return RedirectToAction("Login", "Account");

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == currentUserId);
            if (usuario == null)
                return NotFound();

            var objetivo = await _context.Objetivos.FirstOrDefaultAsync(o => o.IdUsuario == currentUserId && o.Estado);

            var model = new PerfilViewModel
            {
                UsuarioId = usuario.Id,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                Contrasena = usuario.Contrasena,
                ObjetivoId = objetivo?.Id,
                ObjetivoTexto = objetivo?.TextoObjetivo
            };

            return View("Perfil", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(PerfilViewModel model)
        {
            if (!model.CambiarContrasena)
                ModelState.Remove(nameof(model.Contrasena));

            if (!ModelState.IsValid)
                return View("Perfil", model);

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == model.UsuarioId);
            if (usuario == null)
                return NotFound();

            usuario.Nombre = model.Nombre;
            usuario.Correo = model.Correo;

            if (model.CambiarContrasena)
                usuario.Contrasena = model.Contrasena;

            var objetivo = await _context.Objetivos
                .FirstOrDefaultAsync(o => o.IdUsuario == model.UsuarioId && o.Estado);

            if (objetivo == null)
            {
                _context.Objetivos.Add(new Objetivo
                {
                    IdUsuario = model.UsuarioId,
                    TextoObjetivo = model.ObjetivoTexto ?? "",
                    Estado = true,
                    FechaCreacion = DateTime.Now
                });
            }
            else
            {
                objetivo.TextoObjetivo = model.ObjetivoTexto ?? "";
                objetivo.FechaModificacion = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Perfil actualizado correctamente.";
            return RedirectToAction("Index");
        }

    }
}
