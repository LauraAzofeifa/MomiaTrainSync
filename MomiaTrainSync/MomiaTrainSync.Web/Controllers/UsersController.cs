using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.UseCases.TrainerAthleteUseCase;
using MomiaTrainSync.Core.UseCases.UsersUseCases;
using MomiaTrainSync.Web.Security;
using System.Security.Claims;

namespace MomiaTrainSync.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly GetUsuariosUseCase _getUsuariosUseCase;
        private readonly GetEntrenadorAtletaUseCase _getEntrenadorAtletaUseCase;

        public UsersController(
            GetUsuariosUseCase getUsuariosUseCase, 
            GetEntrenadorAtletaUseCase getEntrenadorAtletaUseCase)
        {
            _getUsuariosUseCase = getUsuariosUseCase;
            _getEntrenadorAtletaUseCase = getEntrenadorAtletaUseCase;
        }

        private int? GetCurrentUserId()
        {
            var idValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(idValue, out var id) ? id : null;
        }

        private async Task<List<SelectListItem>> GetAthletesSelectListItemsAsync()
        {
            var response = await _getUsuariosUseCase.ExecuteAsync(rol: "Atleta");

            if (!response.Exito || response.Datos == null || !response.Datos.Any())
                return new List<SelectListItem>();

            return response.Datos
                .OrderBy(a => a.Nombre) // opcional: ordena alfabéticamente
                .Select(a => new SelectListItem
                {
                    Text = $"{a.Nombre ?? ""} {a.Apellido ?? ""}".Trim(),
                    Value = a.Id.ToString()
                })
                .ToList();
        }


        [HttpGet]
        [Permiso]
        public IActionResult ManageUsers()
        {
            var result = _getUsuariosUseCase.ExecuteAsync(incluirInactivos: true).Result;

            return View(result.Datos);
        }

        [HttpGet]
        [Permiso]
        public async Task<IActionResult> ManageAthletes()
        {
            var userId = GetCurrentUserId();
            if (userId is null)
                return Unauthorized();

            var response = _getEntrenadorAtletaUseCase.ExecuteAsync(entrenadorId: userId.Value, incluirInactivos: true).Result;
            
            ViewBag.Athletes = await GetAthletesSelectListItemsAsync();

            return View(response.Datos);
        }
    }
}
