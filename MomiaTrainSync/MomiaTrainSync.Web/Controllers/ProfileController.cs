using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MomiaTrainSync.Core.DTOs;
using MomiaTrainSync.Core.UseCases.UsersUseCases;
using MomiaTrainSync.Web.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MomiaTrainSync.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly GetUsuariosUseCase _getUsuariosUseCase;
        private readonly UpdateUsuarioUseCase _updateUsuarioUseCase;

        public ProfileController(
            GetUsuariosUseCase getUsuariosUseCase, 
            UpdateUsuarioUseCase updateUsuarioUseCase)
        {
            _getUsuariosUseCase = getUsuariosUseCase;
            _updateUsuarioUseCase = updateUsuarioUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> MiPerfil()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var response = await _getUsuariosUseCase.ExecuteAsync(id: int.Parse(userId));

            if (!response.Exito || response.Datos == null)
                return NotFound();

            var usuario = response.Datos.FirstOrDefault();

            if (usuario == null)
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
                },
                ChangePassword = new ChangePasswordViewModel()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult CambiarContrasenna()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarPerfil(ProfileViewModel vm)
        {
            // Limpiar estado previo por si hay datos no relacionados
            ModelState.Clear();

            // Validar solo la parte UpdateProfileViewModel
            TryValidateModel(vm.Update, nameof(vm.Update));

            if (!ModelState.IsValid)
                return View(nameof(MiPerfil), vm);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null || vm.Update.Id != int.Parse(userId))
                return Unauthorized(); // evita que un usuario cambie otro perfil

            var dto = new UsuarioDto
            {
                Id = vm.Update.Id,
                Nombre = vm.Update.Nombre,
                Apellido = vm.Update.Apellido,
                Correo = vm.Update.Correo,
                Telefono = vm.Update.Telefono,
                FechaCumpleannos = vm.Update.FechaCumpleannos,
            };

            var response = await _updateUsuarioUseCase.ExecuteAsync(dto);

            if (!response.Exito)
            {
                TempData["ErrorMessage"] = response.Mensaje ?? "Error al actualizar el perfil.";
                return RedirectToAction(nameof(MiPerfil));
            }

            TempData["SuccessMessage"] = response.Mensaje ?? "Perfil actualizado correctamente.";
            return RedirectToAction(nameof(MiPerfil));
        }
    }
}
