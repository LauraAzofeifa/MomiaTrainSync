using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.UseCases.RolesPermisos.Rol;
using MomiaTrainSync.Core.UseCases.TrainerAthleteUseCase;
using MomiaTrainSync.Core.UseCases.UsersUseCases;
using MomiaTrainSync.Web.Security;
using System.Security.Claims;

namespace MomiaTrainSync.Web.Controllers
{
    public class UsersController : Controller
    {
        #region Dependencias

        private readonly GetRolesUseCase _getRolesUseCase;
        private readonly GetUsuariosUseCase _getUsuariosUseCase;
        private readonly UpdateUsuarioUseCase _updateUsuarioUseCase;
        private readonly GetEntrenadorAtletaUseCase _getEntrenadorAtletaUseCase;

        #endregion

        #region Constructor

        public UsersController(
            GetRolesUseCase getRolesUseCase,
            GetUsuariosUseCase getUsuariosUseCase,
            UpdateUsuarioUseCase updateUsuarioUseCase,
            GetEntrenadorAtletaUseCase getEntrenadorAtletaUseCase)
        {
            _getRolesUseCase = getRolesUseCase;
            _getUsuariosUseCase = getUsuariosUseCase;
            _updateUsuarioUseCase = updateUsuarioUseCase;
            _getEntrenadorAtletaUseCase = getEntrenadorAtletaUseCase;
        }

        #endregion

        #region Métodos Privados Auxiliares

        private int? GetCurrentUserId()
        {
            var idValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(idValue, out var id) ? id : null;
        }

        private async Task<List<SelectListItem>> GetAthletesSelectListItemsAsync()
        {
            var response = await _getUsuariosUseCase.ExecuteAsync(rol: "Atleta");

            if (!response.Exito || response.Datos is null)
                return new List<SelectListItem>();

            return response.Datos
                .OrderBy(a => a.Nombre)
                .Select(a => new SelectListItem
                {
                    Text = $"{a.Nombre} {a.Apellido}".Trim(),
                    Value = a.Id.ToString()
                })
                .ToList();
        }

        private async Task LoadRolesAsync()
        {
            var response = await _getRolesUseCase.ExecuteAsync(incluirInactivos: true);

            ViewBag.Roles = response.Exito && response.Datos != null
                ? response.Datos.Select(r => new SelectListItem
                {
                    Text = r.Nombre,
                    Value = r.IdRol.ToString()
                }).ToList()
                : new List<SelectListItem>();
        }

        #endregion

        #region Administración de Usuarios (Admin)

        [HttpGet]
        [Permiso]
        public async Task<IActionResult> ManageUsers()
        {
            await LoadRolesAsync(); // <<--- Cargar roles para el modal

            var result = await _getUsuariosUseCase.ExecuteAsync(incluirInactivos: true);
            return View(result.Datos);
        }

        [HttpPost]
        [Permiso]
        public async Task<IActionResult> UpdateUserRoleAsync(int Id, int RolId)
        {
            // Validamos que si el usuario actual es administrador no se pueda cambiar su propio rol
            var currentUserId = GetCurrentUserId();

            if (currentUserId == Id)
            {
                TempData["ErrorMessage"] = "No se puede cambiar el rol del usuario actual.";
                return RedirectToAction("ManageUsers");
            }

            var result = await _updateUsuarioUseCase.CambiarRolAsync(Id, RolId);

            TempData[result.Exito ? "SuccessMessage" : "ErrorMessage"] = result.Mensaje;
            return RedirectToAction("ManageUsers");
        }

        [HttpPost]
        [Permiso]
        public async Task<IActionResult> ToggleEstadoUser(int Id) 
        { 
            // Validamos que el usuario actual no se pueda desactivar
            var currentUserId = GetCurrentUserId();
            if (currentUserId == Id)
            {
                TempData["ErrorMessage"] = "No se puede cambiar el estado del usuario actual.";
                return RedirectToAction("ManageUsers");
            }

            var result = await _updateUsuarioUseCase.CambiarEstadoAsync(Id);

            TempData[result.Exito ? "SuccessMessage" : "ErrorMessage"] = result.Mensaje;
            return RedirectToAction("ManageUsers");
        }

        #endregion

        #region Gestión de Atletas (Entrenador)

        [HttpGet]
        [Permiso]
        public async Task<IActionResult> ManageAthletes()
        {
            var userId = GetCurrentUserId();
            if (userId is null)
                return Unauthorized();

            var response = await _getEntrenadorAtletaUseCase.ExecuteAsync(
               entrenadorId: userId.Value,
               incluirInactivos: true
            );

            ViewBag.Athletes = await GetAthletesSelectListItemsAsync();

            return View(response.Datos);
        }

        #endregion
    }
}
