using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MomiaTrainSync.Core.UseCases.Calendario;
using MomiaTrainSync.Core.UseCases.RutinasEntrenamientos.TipoSesion;
using System.Security.Claims;

namespace MomiaTrainSync.Web.Controllers
{

    [Authorize]
    public class CalendarController : Controller
    {
        private readonly GetCalendarioUseCase _getCalendarioUseCase;
        private readonly GetTipoSesionUseCase _getTipoSesionUseCase;

        public CalendarController(
            GetCalendarioUseCase getCalendarioUseCase,
            GetTipoSesionUseCase getTipoSesionUseCase
            )
        {
            _getCalendarioUseCase = getCalendarioUseCase;
            _getTipoSesionUseCase = getTipoSesionUseCase;
        }

        private int? GetCurrentUserId()
        {
            var idValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(idValue, out var id) ? id : null;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> MyCalendar()
        {
            return View();
        }

        [HttpGet("Calendar/Get")]
        public async Task<IActionResult> GetCalendar(
            bool esEntrenador = false,
            int cantidad = 0,
            bool incluirInactivos = false)
        {
            var userId = GetCurrentUserId();

            if (userId == null)
                return Unauthorized("No se pudo identificar al usuario.");

            var result = await _getCalendarioUseCase.ExecuteAsync(
                idUsuario: userId.Value,
                esEntrenador: esEntrenador,
                cantidad: cantidad,
                incluirInactivos: incluirInactivos
            );

            if (!result.Exito)
                return BadRequest(result.Mensaje);

            return Ok(result.Datos);
        }

        [HttpGet("Calendar/TipoSesiones")]
        public async Task<IActionResult> GetTipoSesion()
        {
            var result = await _getTipoSesionUseCase.ExecuteAsync(incluirInactivos: false);
            if (!result.Exito)
                return BadRequest(result.Mensaje);
            return Ok(result.Datos);
        }
    }
}
