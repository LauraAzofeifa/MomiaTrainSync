using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.UseCases.RolesPermisos;
using MomiaTrainSync.Web.Security;
using System.Security.Claims;

namespace MomiaTrainSync.Web.Controllers.Admin
{
    [Authorize]
    public class PermissionsController : Controller
    {
        private readonly GetPermisosUseCase _getPermisosUseCase;

        public PermissionsController(GetPermisosUseCase getPermisosUseCase)
        {
            _getPermisosUseCase = getPermisosUseCase;
        }

        private int? GetCurrentUserId()
        {
            var idValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(idValue, out var id) ? id : null;
        }

        [HttpGet]
        [Permiso]
        public IActionResult Index()
        {
            var userId = GetCurrentUserId();
            if (userId is null)
                return Unauthorized();

            var result = _getPermisosUseCase.ExecuteAsync(incluirInactivos: true).Result;

            return View(result.Datos);
        }

        // Agregar Permisos
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permiso]
        public IActionResult AddPermission(PermisoDto permisoDto)
        {
            // Lógica para agregar permisos
            return RedirectToAction("Index");
        }

        // Editar Permisos
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permiso]
        public IActionResult EditPermission(PermisoDto permisoDto)
        {
            // Lógica para editar permisos
            return RedirectToAction("Index");
        }

        // Eliminar Permisos
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permiso]
        public IActionResult DeletePermission(int IdPermiso)
        {
            // Lógica para eliminar permisos
            return RedirectToAction("Index");
        }
    }
}
