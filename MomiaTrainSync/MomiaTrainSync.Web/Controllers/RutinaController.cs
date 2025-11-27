using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MomiaTrainSync.Core.DTOs.RutinasEntrenamientos;
using MomiaTrainSync.Core.UseCases.RutinasEntrenamientos.Rutinas;
using System.Collections.ObjectModel;

namespace MomiaTrainSync.Web.Controllers
{
    [Authorize]
    public class RutinaController : Controller
    {
        private readonly GetRutinaUseCase _getRutinaUse;

        public RutinaController(
            GetRutinaUseCase getRutinaUseCase
            )
        {
            _getRutinaUse = getRutinaUseCase;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _getRutinaUse.ExecuteAsync(incluirInactivos: true);
            return View(result.Datos);
        }


    }
}
