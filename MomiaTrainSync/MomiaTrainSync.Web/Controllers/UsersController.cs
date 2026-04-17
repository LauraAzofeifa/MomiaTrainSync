using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.UseCases.RolesPermisos.Rol;
using MomiaTrainSync.Core.UseCases.TrainerAthleteUseCase;
using MomiaTrainSync.Core.UseCases.UsersUseCases;
using MomiaTrainSync.Web.Security;
using MomiaTrainSync.Web.ViewModels.UsuariosRoles;
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

        // Populate roles into a view model (DRY helper)
        private async Task PopulateRolesInViewModelAsync(ManageUsuariosViewModel model)
        {
            var rolesResponse = await _getRolesUseCase.ExecuteAsync(incluirInactivos: true);

            model.Roles = rolesResponse.Datos?.Select(r => new SelectListItem
            {
                Text = r.Nombre,
                Value = r.IdRol.ToString()
            });
        }

        #endregion

        #region Administración de Usuarios (Admin)

        [HttpGet]
        [Permiso]
        public async Task<IActionResult> ManageUsers()
        {
            var rolesResponse = await _getRolesUseCase.ExecuteAsync(incluirInactivos: true);

            var usuariosResponse = await _getUsuariosUseCase.ExecuteAsync(incluirInactivos: true);

            var vm = new ManageUsuariosViewModel
            {
                Usuarios = usuariosResponse.Datos,
                // Roles will be populated via helper to avoid duplication
                Roles = rolesResponse.Datos?.Select(r => new SelectListItem
                {
                    Text = r.Nombre,
                    Value = r.IdRol.ToString()
                }),
                Filtro = new UserFilterRequest()
            };

            await PopulateRolesInViewModelAsync(vm);

            return View(vm);
        }

        [HttpPost]
        [Permiso]
        public async Task<IActionResult> UpdateUserRole(int Id, int RolId)
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

        // Filtramos usuarios, los activos, inactivos, etc
        [HttpPost]
        [Permiso]
        public async Task<IActionResult> FilterUsers(ManageUsuariosViewModel model)
        {
            bool incluirInactivos = false;
            bool soloInactivos = false;

            switch (model.Filtro.Estado)
            {
                case EstadoUsuarioFiltro.Todos:
                    incluirInactivos = true;
                    break;

                case EstadoUsuarioFiltro.SoloActivos:
                    incluirInactivos = false;
                    break;

                case EstadoUsuarioFiltro.SoloInactivos:
                    incluirInactivos = true;
                    soloInactivos = true;
                    break;
            }

            // The UI posts the selected role as the SelectListItem.Value which is the role Id string.
            // Get roles to resolve the selected role id to its name (the use case filters by role name).
            var rolesResponse = await _getRolesUseCase.ExecuteAsync(incluirInactivos: true);

            string? roleName = null;
            if (!string.IsNullOrWhiteSpace(model.Filtro.RolSeleccionado))
            {
                // Try parse as role id first
                if (int.TryParse(model.Filtro.RolSeleccionado, out var roleId) && rolesResponse.Datos != null)
                {
                    var role = rolesResponse.Datos.FirstOrDefault(r => r.IdRol == roleId);
                    roleName = role?.Nombre;
                }
                else
                {
                    // If value isn't an id, assume it's already the role name
                    roleName = model.Filtro.RolSeleccionado;
                }
            }

            var result = await _getUsuariosUseCase.ExecuteAsync(
                rol: roleName,
                incluirInactivos: incluirInactivos
            );

            var usuarios = result.Datos ?? Enumerable.Empty<Core.DTOs.UsuariosRoles.UsuarioDto>();

            // If the filter requests only inactive users, filter by the Estado flag
            if (soloInactivos)
            {
                usuarios = usuarios.Where(u => !u.Estado);
            }

            // If the filter requests only active users, ensure Estado == true
            if (model.Filtro.Estado == EstadoUsuarioFiltro.SoloActivos)
            {
                usuarios = usuarios.Where(u => u.Estado);
            }

            model.Usuarios = usuarios;

            await PopulateRolesInViewModelAsync(model);

            return View("ManageUsers", model);
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
