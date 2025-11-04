using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MomiaTrainSync.Core.DTOs;
using MomiaTrainSync.Core.UseCases.UsersUseCases;
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
        public async Task<IActionResult> MiPerfil()
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
                    FechaCumpleannos = usuario.FechaCumpleannos
                }
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarContrasenna(ProfileViewModel vm)
        {
            ModelState.Clear();
            if (!TryValidateModel(vm.ChangePassword, nameof(vm.ChangePassword)))
            {
                TempData["ShowModal"] = "changePasswordModal";
                return View(nameof(MiPerfil), vm);
            }

            var userId = GetCurrentUserId();
            if (userId is null)
                return Unauthorized();

            var response = await _changePasswordUsuarioUseCase.ExecuteAsync(
                usuarioId: userId.Value,
                oldPassword: vm.ChangePassword.CurrentPassword,
                newPassword: vm.ChangePassword.NewPassword
            );

            if (!response.Exito)
            {
                TempData["ErrorMessage"] = response.Mensaje ?? "Error al cambiar la contraseña.";
                TempData["ShowModal"] = "changePasswordModal"; // <--- vuelve a abrir modal
                return View(nameof(MiPerfil), vm);
            }

            TempData[response.Exito ? "SuccessMessage" : "ErrorMessage"] =
                response.Mensaje ?? (response.Exito
                    ? "Contraseña actualizada correctamente."
                    : "Error al actualizar la contraseña.");

            return RedirectToAction(nameof(MiPerfil));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarPerfil(ProfileViewModel vm)
        {
            var userId = GetCurrentUserId();
            if (userId is null || vm.Update.Id != userId)
                return Unauthorized();

            ModelState.Clear();
            if (!TryValidateModel(vm.Update, nameof(vm.Update)))
                return RedirectToAction(nameof(MiPerfil));

            var dto = new UsuarioDto
            {
                Id = vm.Update.Id,
                Nombre = vm.Update.Nombre,
                Apellido = vm.Update.Apellido,
                Correo = vm.Update.Correo,
                Telefono = vm.Update.Telefono,
                FechaCumpleannos = vm.Update.FechaCumpleannos
            };

            var response = await _updateUsuarioUseCase.ExecuteAsync(dto);

            TempData[response.Exito ? "SuccessMessage" : "ErrorMessage"] =
                response.Mensaje ?? (response.Exito
                    ? "Perfil actualizado correctamente."
                    : "Error al actualizar el perfil.");

            return RedirectToAction(nameof(MiPerfil));
        }
    }
}
