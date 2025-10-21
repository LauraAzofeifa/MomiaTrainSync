using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Data;
using MomiaTrainSync.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MomiaTrainSync.Controllers
{
    public class EntrenadorController : Controller
    {
        private readonly AppDbContext _context;

        public EntrenadorController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard() => View();

        public IActionResult Biblioteca() => View();

        public async Task<IActionResult> Atleta()
        {
            var currentUserId = int.TryParse(HttpContext.Session.GetString("UsuarioId"), out var id) ? id : 0;
            var currentUserRole = HttpContext.Session.GetString("UsuarioRol");

            if (currentUserId == 0 || string.IsNullOrEmpty(currentUserRole))
                return RedirectToAction("Login", "Account");

            List<Usuario> atletas;

            if (currentUserRole == "Entrenador")
            {
                atletas = await _context.EntrenadoresAtletas
                    .Where(ea => ea.EntrenadorId == currentUserId)
                    .Include(ea => ea.Atleta)
                    .Select(ea => ea.Atleta)
                    .ToListAsync();
            }
            else if (currentUserRole == "Administrador")
            {
                atletas = await _context.Usuarios
                    .Where(u => u.Rol == "Atleta")
                    .ToListAsync();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            foreach (var atleta in atletas)
            {
                atleta.ObjetivoTexto = await _context.Objetivos
                    .Where(o => o.IdUsuario == atleta.Id && o.Estado)
                    .Select(o => o.TextoObjetivo)
                    .FirstOrDefaultAsync();
            }

            return View(atletas);
        }


        public IActionResult Calendario() => View();
        public async Task<IActionResult> Detalle(int id)
        {
            var currentUserId = int.TryParse(HttpContext.Session.GetString("UsuarioId"), out var userId) ? userId : 0;
            if (currentUserId == 0)
                return RedirectToAction("Login", "Account");

            var atleta = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id && u.Rol == "Atleta");

            if (atleta == null)
                return NotFound();

            atleta.ObjetivoTexto = await _context.Objetivos
                .Where(o => o.IdUsuario == atleta.Id && o.Estado)
                .Select(o => o.TextoObjetivo)
                .FirstOrDefaultAsync();

            ViewBag.Planes = await _context.PlanesEntrenamiento
                .Include(p => p.Creador)
                .Where(p => p.IdAtleta == atleta.Id)
                .ToListAsync();

            return View(atleta);
        }

    }
}
