using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Data;
using MomiaTrainSync.Models;

namespace MomiaTrainSync.Controllers
{
    public class PlanEntrenamientoController : Controller
    {
        private readonly AppDbContext _context;

        public PlanEntrenamientoController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var currentUserId = int.TryParse(HttpContext.Session.GetString("UsuarioId"), out var id) ? id : 0;
            var currentUserRole = HttpContext.Session.GetString("UsuarioRol");

            if (currentUserId == 0 || string.IsNullOrEmpty(currentUserRole))
                return RedirectToAction("Login", "Account");

            IQueryable<PlanEntrenamiento> planesQuery = _context.PlanesEntrenamiento
                .Include(p => p.Atleta)
                .Include(p => p.Creador);

            if (currentUserRole == "Entrenador")
            {
                planesQuery = planesQuery.Where(p => p.IdCreador == currentUserId);
            }

            var planes = await planesQuery.ToListAsync();
            return View(planes);
        }

        public async Task<IActionResult> Create()
        {
            var currentUserId = int.TryParse(HttpContext.Session.GetString("UsuarioId"), out var userId) ? userId : 0;
            var currentUserRole = HttpContext.Session.GetString("UsuarioRol");

            if (currentUserId == 0 || string.IsNullOrEmpty(currentUserRole))
                return RedirectToAction("Login", "Account");

            if (currentUserRole == "Entrenador")
            {
                ViewBag.Atletas = await _context.EntrenadoresAtletas
                    .Where(ea => ea.EntrenadorId == currentUserId)
                    .Include(ea => ea.Atleta)
                    .Select(ea => ea.Atleta)
                    .ToListAsync();
            }
            else if (currentUserRole == "Administrador")
            {
                ViewBag.Atletas = await _context.Usuarios
                    .Where(u => u.Rol == "Atleta" && u.Estado == "Activo")
                    .ToListAsync();
            }

            return View(new PlanEntrenamiento());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlanEntrenamiento plan, int IdAtleta)
        {
            var currentUserId = int.TryParse(HttpContext.Session.GetString("UsuarioId"), out var id) ? id : 0;

            if (currentUserId == 0)
                return RedirectToAction("Login", "Account");

            if (IdAtleta == 0)
            {
                ModelState.AddModelError("IdAtleta", "Debe seleccionar un atleta.");
            }

            if (!ModelState.IsValid)
                return View(plan);

            plan.IdAtleta = IdAtleta;
            plan.IdCreador = currentUserId;
            plan.FechaCreacion = DateTime.Now;
            plan.Estado = true;

            _context.PlanesEntrenamiento.Add(plan);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Plan creado correctamente.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Clone(int id)
        {
            var currentUserId = int.TryParse(HttpContext.Session.GetString("UsuarioId"), out var userId) ? userId : 0;
            var currentUserRole = HttpContext.Session.GetString("UsuarioRol");

            if (currentUserId == 0 || string.IsNullOrEmpty(currentUserRole))
                return RedirectToAction("Login", "Account");

            var originalPlan = await _context.PlanesEntrenamiento
                .Include(p => p.Atleta)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (originalPlan == null)
                return NotFound();

            if (currentUserRole == "Entrenador" && originalPlan.IdCreador != currentUserId)
                return Unauthorized();

            var clonPlan = new PlanEntrenamiento
            {
                Objetivo = originalPlan.Objetivo,
                Detalle = originalPlan.Detalle
            };

            if (currentUserRole == "Entrenador")
            {
                ViewBag.Atletas = await _context.EntrenadoresAtletas
                    .Where(ea => ea.EntrenadorId == currentUserId)
                    .Include(ea => ea.Atleta)
                    .Select(ea => ea.Atleta)
                    .ToListAsync();
            }
            else if (currentUserRole == "Administrador")
            {
                ViewBag.Atletas = await _context.Usuarios
                    .Where(u => u.Rol == "Atleta" && u.Estado == "Activo")
                    .ToListAsync();
            }

            return View(clonPlan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Clone(PlanEntrenamiento plan, int IdAtleta)
        {
            var currentUserId = int.TryParse(HttpContext.Session.GetString("UsuarioId"), out var id) ? id : 0;

            if (currentUserId == 0)
                return RedirectToAction("Login", "Account");

            if (IdAtleta == 0)
            {
                ModelState.AddModelError("IdAtleta", "Debe seleccionar un atleta.");
            }

            if (!ModelState.IsValid)
                return View(plan);

            plan.Id = 0;
            plan.IdAtleta = IdAtleta;
            plan.IdCreador = currentUserId;
            plan.FechaCreacion = DateTime.Now;
            plan.Estado = true;

            _context.PlanesEntrenamiento.Add(plan);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Plan clonado correctamente.";
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Details(int id)
        {
            var plan = await _context.PlanesEntrenamiento
                .Include(p => p.Atleta)
                .Include(p => p.Creador)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plan == null)
                return NotFound();

            return View(plan);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var currentUserId = int.TryParse(HttpContext.Session.GetString("UsuarioId"), out var userId) ? userId : 0;
            var currentUserRole = HttpContext.Session.GetString("UsuarioRol");

            var plan = await _context.PlanesEntrenamiento
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plan == null)
                return NotFound();

            if (currentUserRole == "Entrenador" && plan.IdCreador != currentUserId)
                return Unauthorized();

            return View(plan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlanEntrenamiento model)
        {
            var currentUserId = int.TryParse(HttpContext.Session.GetString("UsuarioId"), out var userId) ? userId : 0;
            var currentUserRole = HttpContext.Session.GetString("UsuarioRol");

            var plan = await _context.PlanesEntrenamiento.FirstOrDefaultAsync(p => p.Id == id);

            if (plan == null)
                return NotFound();

            if (currentUserRole == "Entrenador" && plan.IdCreador != currentUserId)
                return Unauthorized();

            plan.Objetivo = model.Objetivo;
            plan.Detalle = model.Detalle;
            plan.Estado = model.Estado;
            plan.FechaModificacion = DateTime.Now;

            _context.Update(plan);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Plan actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var currentUserId = int.TryParse(HttpContext.Session.GetString("UsuarioId"), out var userId) ? userId : 0;
            var currentUserRole = HttpContext.Session.GetString("UsuarioRol");

            var plan = await _context.PlanesEntrenamiento
                .Include(p => p.Atleta)
                .Include(p => p.Creador)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plan == null)
                return NotFound();

            if (currentUserRole == "Entrenador" && plan.IdCreador != currentUserId)
                return Unauthorized();

            return View(plan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currentUserId = int.TryParse(HttpContext.Session.GetString("UsuarioId"), out var userId) ? userId : 0;
            var currentUserRole = HttpContext.Session.GetString("UsuarioRol");

            var plan = await _context.PlanesEntrenamiento.FirstOrDefaultAsync(p => p.Id == id);

            if (plan == null)
                return NotFound();

            if (currentUserRole == "Entrenador" && plan.IdCreador != currentUserId)
                return Unauthorized();

            _context.PlanesEntrenamiento.Remove(plan);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Plan eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
