using Microsoft.AspNetCore.Mvc;
using MomiaTrainSync.Core.DTOs.EntrenamientoZonas;
using MomiaTrainSync.Core.DTOs.RutinasEntrenamientos;
using MomiaTrainSync.Core.Interfaces.Repositories.RutinasEntrenamientos;
using MomiaTrainSync.Core.UseCases.RutinasEntrenamientos.Entrenamientos;
using MomiaTrainSync.Core.UseCases.RutinasEntrenamientos.Rutinas;
using MomiaTrainSync.Web.ViewModels.RutinasEntrenamientos;
using System.Collections.ObjectModel;

namespace MomiaTrainSync.Web.Controllers
{
    public class EntrenamientosController : Controller
    {
        private readonly GetEntrenamientoUseCase _getEntrenamientosUseCase;
        private readonly AddEntrenamientoUseCase _addEntrenamientoUseCase;
        private readonly UpdateEntrenamientoUseCase _updateEntrenamientoUseCase;

        public EntrenamientosController(
            GetEntrenamientoUseCase getEntrenamientosUseCase,
            AddEntrenamientoUseCase addEntrenamientoUseCase,
            UpdateEntrenamientoUseCase updateEntrenamientoUseCase
        )
        {
            _getEntrenamientosUseCase = getEntrenamientosUseCase;
            _addEntrenamientoUseCase = addEntrenamientoUseCase;
            _updateEntrenamientoUseCase = updateEntrenamientoUseCase;
        }

        #region Private Methods
        // Construye el viewmodel del Index (con rutinas y id relación)
        private async Task<EntrenamientosViewModel> BuildRutinasViewModel(int idRutina)
        {
            var result = await _getEntrenamientosUseCase.ExecuteAsync(
                IdRutina: idRutina,
                incluirInactivos: true
            );

            return new EntrenamientosViewModel
            {
                IdRutina = idRutina,
                Entrenamientos = result.Exito ? result.Datos : new Collection<EntrenamientoDto>()
            };
        }

        // Retorna la vista Index cuando hay errores, cargando nuevamente rutinas
        private async Task<IActionResult> ReturnIndexViewWithData(int idRutina, EntrenamientosViewModel vm)
        {
            var recargado = await BuildRutinasViewModel(idRutina);

            vm.Entrenamientos = recargado.Entrenamientos;
            vm.IdRutina = idRutina;

            return View(nameof(Index), vm);
        }

        #endregion

        public async Task<IActionResult> Index(int idRutina)
        {
            var vm = await BuildRutinasViewModel(idRutina);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEntrenamiento(EntrenamientosViewModel vm)
        {
            ModelState.Clear();

            if (!TryValidateModel(vm.EntrenamientoFormCreate, nameof(vm.EntrenamientoFormCreate)))
            {
                TempData["ShowModal"] = "addEntrenamientoModal";
                return await ReturnIndexViewWithData(vm.EntrenamientoFormCreate.IdRutina, vm);
            }

            var dto = new EntrenamientoDto
            {
                IdRutina = vm.EntrenamientoFormCreate.IdRutina,
                Nombre = vm.EntrenamientoFormCreate.Nombre,
                TipoSesion = vm.EntrenamientoFormCreate.TipoSesion,
                Objetivo = vm.EntrenamientoFormCreate.Objetivo,
                DuracionEstimada = vm.EntrenamientoFormCreate.DuracionEstimada,
                NivelEsfuerzoEsperado = vm.EntrenamientoFormCreate.NivelEsfuerzoEsperado,
                FechaProgramada = vm.EntrenamientoFormCreate.FechaProgramada,
                Estado = vm.EntrenamientoFormCreate.Estado
            };

            var response = await _addEntrenamientoUseCase.ExecuteAsync(dto);

            TempData[response.Exito ? "SuccessMessage" : "ErrorMessage"] =
                response.Mensaje ?? (response.Exito ? "Entrenamiento agregado." : "Error al agregar el entrenamiento.");

            return RedirectToAction("Index", new { idRutina = vm.EntrenamientoFormCreate.IdRutina });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEntrenamiento()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEntrenamiento(int id)
        {
            return View();
        }
    }
}
