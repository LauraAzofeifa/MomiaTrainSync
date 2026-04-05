using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.UseCases.UsersUseCases;
using MomiaTrainSync.Web.Security;
using MomiaTrainSync.Web.ViewModels;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MomiaTrainSync.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly GetUsuariosUseCase _getUsuariosUseCase;
        private readonly UpdateUsuarioUseCase _updateUsuarioUseCase;
        private readonly ChangePasswordUsuarioUseCase _changePasswordUsuarioUseCase;

        public ProfileController(
            GetUsuariosUseCase getUsuariosUseCase,
            UpdateUsuarioUseCase updateUsuarioUseCase,
            ChangePasswordUsuarioUseCase changePasswordUsuarioUseCase)
        {
            _getUsuariosUseCase = getUsuariosUseCase;
            _updateUsuarioUseCase = updateUsuarioUseCase;
            _changePasswordUsuarioUseCase = changePasswordUsuarioUseCase;
        }

        private int? GetCurrentUserId()
        {
            var idValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(idValue, out var id) ? id : null;
        }

        [HttpGet]
        [Permiso]
        public async Task<IActionResult> MyProfile()
        {
            var userId = GetCurrentUserId();
            if (userId is null)
                return Unauthorized();

            var response = await _getUsuariosUseCase.ExecuteAsync(id: userId.Value);
            var usuario = response.Datos?.FirstOrDefault();

            if (!response.Exito || usuario is null)
                return NotFound();

            var model = new ProfileViewModel
            {
                Details = usuario,
                Update = new UpdateProfileViewModel
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Correo = usuario.Correo,
                    Telefono = usuario.Telefono,
                    Biografia = usuario.Biografia!,
                    FechaCumpleannos = usuario.FechaCumpleannos
                }
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permiso]
        public async Task<IActionResult> EditProfile(ProfileViewModel vm)
        {
            var userId = GetCurrentUserId();
            if (userId is null || vm.Update.Id != userId)
                return Unauthorized();

            ModelState.Clear();

            if (!TryValidateModel(vm.Update, nameof(vm.Update)))
            {
                // Recargar el usuario completo (incluye Rol)
                var userResponse = await _getUsuariosUseCase.ExecuteAsync(id: userId.Value);
                vm.Details = userResponse.Datos!.First();

                return View(nameof(MyProfile), vm);
            }


            var dto = new UsuarioDto
            {
                Id = vm.Update.Id,
                Nombre = vm.Update.Nombre,
                Apellido = vm.Update.Apellido,
                Correo = vm.Update.Correo,
                Telefono = vm.Update.Telefono,
                Biografia = vm.Update.Biografia,
                FechaCumpleannos = vm.Update.FechaCumpleannos
            };

            var response = await _updateUsuarioUseCase.ExecuteAsync(dto);

            TempData[response.Exito ? "SuccessMessage" : "ErrorMessage"] =
                response.Mensaje ?? (response.Exito
                    ? "Perfil actualizado correctamente."
                    : "Error al actualizar el perfil.");

            return RedirectToAction(nameof(MyProfile));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permiso]
        public async Task<IActionResult> ChangePassword(ProfileViewModel vm)
        {
            ModelState.Clear();

            if (!TryValidateModel(vm.ChangePassword, nameof(vm.ChangePassword)))
            {
                var userId = GetCurrentUserId();
                if (userId is null)
                    return Unauthorized();

                var model = await BuildProfileViewModel(userId.Value);

                TempData["ShowModal"] = "changePasswordModal";
                return View(nameof(MyProfile), model); // ✅ ahora viene completo
            }

            var currentUserId = GetCurrentUserId();
            if (currentUserId is null)
                return Unauthorized();

            var response = await _changePasswordUsuarioUseCase.ExecuteAsync(
                usuarioId: currentUserId.Value,
                oldPassword: vm.ChangePassword.CurrentPassword,
                newPassword: vm.ChangePassword.NewPassword
            );

            if (!response.Exito)
            {
                var model = await BuildProfileViewModel(currentUserId.Value);

                TempData["ErrorMessage"] = response.Mensaje ?? "Error al cambiar la contraseña.";
                TempData["ShowModal"] = "changePasswordModal";
                return View(nameof(MyProfile), model);
            }

            TempData["SuccessMessage"] = response.Mensaje ?? "Contraseña actualizada correctamente.";
            return RedirectToAction(nameof(MyProfile));
        }

        private async Task<ProfileViewModel> BuildProfileViewModel(int userId)
        {
            var response = await _getUsuariosUseCase.ExecuteAsync(id: userId);
            var usuario = response.Datos!.First();

            return new ProfileViewModel
            {
                Details = usuario,
                Update = new UpdateProfileViewModel
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Correo = usuario.Correo,
                    Telefono = usuario.Telefono,
                    Biografia = usuario.Biografia!,
                    FechaCumpleannos = usuario.FechaCumpleannos
                },
                ChangePassword = new ChangePasswordViewModel()
            };
        }
    }
}
