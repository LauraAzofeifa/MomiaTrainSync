using Microsoft.AspNetCore.Mvc;
using MomiaTrainSync.Core.UseCases.ZonaEntrenamiento;
using MomiaTrainSync.Web.ViewModels.ZonasEntrenamiento;

namespace MomiaTrainSync.Web.Controllers
{
    public class ZonaEntrenamientoController : Controller
    {
        private readonly GetZonaEntrenamientoUseCase _getZonaEntrenamientoUseCase;

        public ZonaEntrenamientoController(
            GetZonaEntrenamientoUseCase getZonaEntrenamientoUseCase
        )
        {
            _getZonaEntrenamientoUseCase = getZonaEntrenamientoUseCase;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _getZonaEntrenamientoUseCase.ExecuteAsync();

            if (result.Exito && result.Datos != null)
            {
                ViewBag.ZonasEntrenamiento = result.Datos;
            }
            else
            {
                ViewBag.ZonasEntrenamiento = new List<object>();
                TempData["ErrorMessage"] = result.Mensaje ?? "Error al cargar las zonas de entrenamiento.";
            }

            var vm = new ZonaEntrenameintoViewModel
            {
                ZonasEntrenamientos = result.Datos!
            };

            return View(vm);
        }
    }
}
