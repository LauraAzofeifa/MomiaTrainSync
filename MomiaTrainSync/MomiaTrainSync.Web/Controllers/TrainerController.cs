using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MomiaTrainSync.Core.DTOs.EntrenadorAtleta;
using MomiaTrainSync.Core.UseCases.TrainerAthleteUseCase;
using MomiaTrainSync.Core.UseCases.UsersUseCases;
using MomiaTrainSync.Web.Security;
using System.Security.Claims;

namespace MomiaTrainSync.Web.Controllers
{
    [Authorize]
    public class TrainerController : Controller
    {
        private readonly GetUsuariosUseCase _getUsuariosUseCase;
        private readonly AddAthleteUseCase _addAthleteUseCase;
        private readonly DeleteAthleteUseCase _deleteAthleteUseCase;

        public TrainerController(
            GetUsuariosUseCase getUsuariosUseCase,
            AddAthleteUseCase addAthleteUseCase,
            DeleteAthleteUseCase deleteAthleteUseCase)
        {
            _getUsuariosUseCase = getUsuariosUseCase;
            _addAthleteUseCase = addAthleteUseCase;
            _deleteAthleteUseCase = deleteAthleteUseCase;
        }

        private int? GetCurrentUserId()
        {
            var idValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(idValue, out var id) ? id : null;
        }

        // Asignar atleta a entrenador
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permiso]
        public async Task<IActionResult> AddAthlete(EntrenadorAtletaDto entrenadorAtletaDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Los datos proporcionados no son válidos.";
                return RedirectToAction("ManageAthletes", "Users");
            }

            var idEntrenador = GetCurrentUserId();
            if (idEntrenador == null)
            {
                TempData["ErrorMessage"] = "No se pudo identificar al entrenador actual.";
                return RedirectToAction("ManageAthletes", "Users");
            }

            entrenadorAtletaDto.IdEntrenador = idEntrenador.Value;

            var result = await _addAthleteUseCase.ExecuteAsync(entrenadorAtletaDto);

            if (!result.Exito)
            {
                TempData["ErrorMessage"] = result.Mensaje;
                return RedirectToAction("ManageAthletes", "Users");
            }

            TempData["SuccessMessage"] = result.Mensaje;
            return RedirectToAction("ManageAthletes", "Users");
        }

        // Eliminar (desactivar) relación entrenador–atleta
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permiso]
        public async Task<IActionResult> DeleteAthlete(int idRelacion)
        {
            if (idRelacion <= 0)
            {
                TempData["ErrorMessage"] = "Identificador de relación inválido.";
                return RedirectToAction("ManageAthletes", "Users");
            }

            var result = await _deleteAthleteUseCase.ExecuteAsync(idRelacion);

            if (!result.Exito)
            {
                TempData["ErrorMessage"] = result.Mensaje;
                return RedirectToAction("ManageAthletes", "Users");
            }

            TempData["SuccessMessage"] = result.Mensaje;
            return RedirectToAction("ManageAthletes", "Users");
        }
    }
}
