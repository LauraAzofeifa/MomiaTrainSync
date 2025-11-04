using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MomiaTrainSync.Core.UseCases.UsersUseCases;
using MomiaTrainSync.Domain.Enums;
using MomiaTrainSync.Web.Security;

namespace MomiaTrainSync.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly GetUsuariosUseCase _getUsuariosUseCase;

        public AdminController(GetUsuariosUseCase getUsuariosUseCase)
        {
            _getUsuariosUseCase = getUsuariosUseCase;
        }

        public ActionResult Index() {
            return View();
        }

        [HttpGet]
        [Permiso]
        public async Task<IActionResult> ManageUsers()
        {
            var result = await _getUsuariosUseCase.ExecuteAsync();

            return View(result.Datos);
        }

        [HttpGet]
        public IActionResult Calendario()
        {
            return View();
        }
    }
}
