using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MomiaTrainSync.Core.UseCases.UsersUseCases;
using MomiaTrainSync.Domain.Enums;
using MomiaTrainSync.Web.Security;

namespace MomiaTrainSync.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly GetUsuariosUseCase _getUsuariosUseCase;

        public AdminController(GetUsuariosUseCase getUsuariosUseCase)
        {
            _getUsuariosUseCase = getUsuariosUseCase;
        }

        [HttpGet]
        [Authorize]
        [Permiso("GESTIONAR_USUARIOS")]
        public async Task<IActionResult> Users()
        {
            var result = await _getUsuariosUseCase.ExecuteAsync();

            return View(result.Datos);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Calendario()
        {
            return View();
        }
    }
}
