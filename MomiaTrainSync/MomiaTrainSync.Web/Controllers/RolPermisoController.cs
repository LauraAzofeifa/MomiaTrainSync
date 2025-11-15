using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.UseCases.RolesPermisos.Permiso;
using MomiaTrainSync.Core.UseCases.RolesPermisos.Rol;
using MomiaTrainSync.Core.UseCases.RolesPermisos.RolPermiso;
using MomiaTrainSync.Web.ViewModels.UsuariosRoles;

namespace MomiaTrainSync.Web.Controllers
{
    [Authorize]
    public class RolPermisoController : Controller
    {
        #region Dependencias
        private readonly AddRolUseCase _addRolUseCase;
        private readonly GetRolesUseCase _getRolesUseCase;
        private readonly UpdateRolUseCase _updateRolUseCase;

        private readonly AddPermisoUseCase _addPermisoUseCase;
        private readonly GetPermisosUseCase _getPermisosUseCase;
        private readonly UpdatePermisoUseCase _updatePermisoUseCase;

        private readonly GetPermisosPorRolUseCase _getPermisosPorRolUseCase;
        private readonly AsignarPermisosUseCase _asignarPermisosUseCase;
        #endregion

        public RolPermisoController(
            AddRolUseCase addRolUseCase,
            GetRolesUseCase getRolesUseCase,
            UpdateRolUseCase updateRolUseCase,
            AddPermisoUseCase addPermisoUseCase,
            GetPermisosUseCase getPermisosUseCase,
            UpdatePermisoUseCase updatePermisoUseCase,
            GetPermisosPorRolUseCase getPermisosPorRolUseCase,
            AsignarPermisosUseCase asignarPermisosUseCase)
        {
            _addRolUseCase = addRolUseCase;
            _getRolesUseCase = getRolesUseCase;
            _updateRolUseCase = updateRolUseCase;

            _addPermisoUseCase = addPermisoUseCase;
            _getPermisosUseCase = getPermisosUseCase;
            _updatePermisoUseCase = updatePermisoUseCase;

            _getPermisosPorRolUseCase = getPermisosPorRolUseCase;
            _asignarPermisosUseCase = asignarPermisosUseCase;
        }

        #region Métodos privados reutilizables

        private async Task LoadIndexDataAsync(RolPermisosViewModel vm, int? idRol)
        {
            vm.Roles = (await _getRolesUseCase.ExecuteAsync()).Datos ?? new();
            vm.TodosPermisos = (await _getPermisosUseCase.ExecuteAsync(incluirInactivos: true)).Datos?.ToList() ?? new();
            vm.RolSeleccionadoId = idRol;

            // Si no hay rol seleccionado → NO hay que cargar RolForm
            if (!idRol.HasValue)
            {
                vm.PermisosAsignadosIds = new List<int>();
                return;
            }

            // Buscar el rol seleccionado en la lista cargada
            var rolSeleccionado = vm.Roles.FirstOrDefault(r => r.IdRol == idRol.Value);

            if (rolSeleccionado != null)
            {
                vm.RolForm = new RolFormViewModel
                {
                    IdRol = rolSeleccionado.IdRol,
                    Nombre = rolSeleccionado.Nombre,
                    Descripcion = rolSeleccionado.Descripcion,
                    Estado = rolSeleccionado.Estado
                };
            }

            // Permisos asignados
            var permisosPorRol = await _getPermisosPorRolUseCase.ExecuteAsync(idRol.Value);

            if (permisosPorRol.Exito && permisosPorRol.Datos != null)
            {
                vm.PermisosAsignadosIds = permisosPorRol.Datos
                    .Select(p => Math.Abs(p.IdPermiso))
                    .Distinct()
                    .ToList();
            }
        }

        #endregion

        #region Vista principal

        public async Task<IActionResult> Index(int? idRol = null)
        {
            var vm = new RolPermisosViewModel();
            await LoadIndexDataAsync(vm, idRol);

            return View(vm);
        }

        #endregion


        #region RolPermisos

        [HttpPost]
        public async Task<IActionResult> AssignPermissionToRole(RolPermisosViewModel model)
        {
            if (model.RolSeleccionadoId == null)
                return RedirectToAction("Index");

            var response = await _asignarPermisosUseCase.ExecuteAsync(
                model.RolSeleccionadoId.Value,
                model.PermisosSeleccionados
            );

            if (!response.Exito)
            {
                TempData["ErrorMessage"] = response.Mensaje;
                return RedirectToAction("Index", new { idRol = model.RolSeleccionadoId });
            }

            TempData["Mensaje"] = response.Mensaje;
            return RedirectToAction("Index", new { idRol = model.RolSeleccionadoId });
        }

        #endregion


        #region Rol

        [HttpPost]
        public async Task<IActionResult> AddRole(RolPermisosViewModel vm)
        {
            ModelState.Clear();

            if (!TryValidateModel(vm.RolForm, nameof(vm.RolForm)))
            {
                TempData["ShowModal"] = "addRolModal";
                await LoadIndexDataAsync(vm, null);
                return View("Index", vm);
            }

            var rol = new RolDto
            {
                Nombre = vm.RolForm.Nombre,
                Descripcion = vm.RolForm.Descripcion,
                Estado = vm.RolForm.Estado
            };

            var response = await _addRolUseCase.ExecuteAsync(rol);

            if (!response.Exito)
            {
                TempData["ErrorMessage"] = response.Mensaje;
                return RedirectToAction("Index");
            }

            TempData["SuccessMesage"] = response.Mensaje;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(RolPermisosViewModel vm)
        {
            ModelState.Clear();

            if (!TryValidateModel(vm.RolForm, nameof(vm.RolForm)))
            {
                TempData["ShowModal"] = "editRolModal";
                await LoadIndexDataAsync(vm, vm.RolSeleccionadoId);
                return View("Index", vm);
            }

            var rol = new RolDto
            {
                IdRol = vm.RolForm.IdRol,
                Nombre = vm.RolForm.Nombre,
                Descripcion = vm.RolForm.Descripcion
            };

            var response = await _updateRolUseCase.ExecuteAsync(rol);

            if (!response.Exito)
            {
                TempData["ErrorMessage"] = response.Mensaje;
                return RedirectToAction("Index", new { idRol = vm.RolForm.IdRol });
            }

            TempData["SuccessMessage"] = response.Mensaje;
            return RedirectToAction("Index", new { idRol = vm.RolForm.IdRol });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleEstadoRol(int id)
        {
            var response = await _updateRolUseCase.StatusExecuteAsync(id);

            if (!response.Exito)
                TempData["ErrorMessage"] = response.Mensaje;
            else
                TempData["SuccessMessage"] = response.Mensaje;

            return RedirectToAction("Index", new { idRol = id });
        }

        #endregion


        #region Permiso
        [HttpPost]
        public async Task<IActionResult> AddPermission(RolPermisosViewModel vm)
        {
            ModelState.Clear();

            if (!TryValidateModel(vm.PermisoForm, nameof(vm.PermisoForm)))
            {
                TempData["ShowModal"] = "addPermisoModal";
                await LoadIndexDataAsync(vm, null);
                return View("Index", vm);
            }

            var permiso = new PermisoDto
            {
                Codigo = vm.PermisoForm.Codigo,
                Descripcion = vm.PermisoForm.Descripcion,
                Categoria = vm.PermisoForm.Categoria,
                Ruta = vm.PermisoForm.Ruta
            };

            var response = await _addPermisoUseCase.ExecuteAsync(permiso);

            if (!response.Exito)
            {
                TempData["ErrorMessage"] = response.Mensaje;
                return RedirectToAction("Index");
            }

            TempData["SuccessMessage"] = response.Mensaje;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditPermission(RolPermisosViewModel vm)
        {
            ModelState.Clear();

            if (!TryValidateModel(vm.PermisoForm, nameof(vm.PermisoForm)))
            {
                TempData["ShowModal"] = "addPermisoModal";
                await LoadIndexDataAsync(vm, null);
                return View("Index", vm);
            }

            var permiso = new PermisoDto
            {
                IdPermiso = vm.PermisoForm.IdPermiso,
                Codigo = vm.PermisoForm.Codigo,
                Descripcion = vm.PermisoForm.Descripcion,
                Categoria = vm.PermisoForm.Categoria,
                Ruta = vm.PermisoForm.Ruta
            };

            var response = await _updatePermisoUseCase.ExecuteAsync(permiso);

            if (!response.Exito)
            {
                TempData["ErrorMessage"] = response.Mensaje;
                return RedirectToAction("Index");
            }

            TempData["SuccessMessage"] = response.Mensaje;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleEstadoPermiso(int id)
        {
            var response = await _updatePermisoUseCase.StatusExecuteAsync(id);

            if (!response.Exito)
                TempData["ErrorMessage"] = response.Mensaje;
            else
                TempData["SuccessMessage"] = response.Mensaje;

            return RedirectToAction("Index");
        }
        #endregion
    }
}
