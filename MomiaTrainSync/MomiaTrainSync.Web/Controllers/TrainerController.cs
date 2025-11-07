using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MomiaTrainSync.Core.UseCases.UsersUseCases;
using MomiaTrainSync.Web.Security;
using System.Security.Claims;

namespace MomiaTrainSync.Web.Controllers
{
    [Authorize]
    public class TrainerController : Controller
    {
        private readonly GetUsuariosUseCase _getUsuariosUseCase;
        
        public TrainerController(GetUsuariosUseCase getUsuariosUseCase)
        {
            _getUsuariosUseCase = getUsuariosUseCase;
        }

        private int? GetCurrentUserId()
        {
            var idValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(idValue, out var id) ? id : null;
        }

        [HttpGet]
        [Permiso]
        public IActionResult ManageAthletes()
        {
            var userId = GetCurrentUserId();
            if (userId is null)
                return Unauthorized();

            var response = _getUsuariosUseCase.ExecuteAsync(entrenadorId: userId.Value, incluirInactivos: true).Result;

            return View(response.Datos);
        }

        [HttpGet]
        public IActionResult Trainings()
        {
            return View();
        }
    }
}
