using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MomiaTrainSync.Core.UseCases;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MomiaTrainSync.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly GetUsuariosUseCase _getUsuariosUseCase;

        public ProfileController(GetUsuariosUseCase getUsuariosUseCase)
        {
            _getUsuariosUseCase = getUsuariosUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> MiPerfil()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = await _getUsuariosUseCase.ExecuteAsync(id: int.Parse(userId!));

            if (!response.Exito || response.Datos == null)
            {
                return NotFound();
            }

            var usuario = response.Datos.FirstOrDefault();

            if (usuario == null) {
                return NotFound();
            }

            return View(usuario);
        }
    }
}
