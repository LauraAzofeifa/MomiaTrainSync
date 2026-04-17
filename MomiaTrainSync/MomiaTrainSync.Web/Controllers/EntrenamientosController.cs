using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MomiaTrainSync.Core.DTOs.EntrenamientoZonas;
using MomiaTrainSync.Core.DTOs.RutinasEntrenamientos;
using MomiaTrainSync.Core.Interfaces.Repositories.RutinasEntrenamientos;
using MomiaTrainSync.Core.UseCases.RutinasEntrenamientos.Entrenamientos;
using MomiaTrainSync.Core.UseCases.RutinasEntrenamientos.Rutinas;
using MomiaTrainSync.Core.UseCases.RutinasEntrenamientos.TipoSesion;
using MomiaTrainSync.Web.ViewModels.RutinasEntrenamientos;
using System.Collections.ObjectModel;

namespace MomiaTrainSync.Web.Controllers
{
    public class EntrenamientosController : Controller
    {
        private readonly GetTipoSesionUseCase _getTipoSesionUseCase;

        private readonly GetEntrenamientoUseCase _getEntrenamientosUseCase;
        private readonly AddEntrenamientoUseCase _addEntrenamientoUseCase;
        private readonly UpdateEntrenamientoUseCase _updateEntrenamientoUseCase;

        public EntrenamientosController(
            GetTipoSesionUseCase getTipoSesionUseCase,
            GetEntrenamientoUseCase getEntrenamientosUseCase,
            AddEntrenamientoUseCase addEntrenamientoUseCase,
            UpdateEntrenamientoUseCase updateEntrenamientoUseCase
        )
        {
            _getTipoSesionUseCase = getTipoSesionUseCase;
            _getEntrenamientosUseCase = getEntrenamientosUseCase;
            _addEntrenamientoUseCase = addEntrenamientoUseCase;
            _updateEntrenamientoUseCase = updateEntrenamientoUseCase;
        }

        #region Private Methods
        // Cargamos el Dropdown
        private async Task LoadTipoSesionDropdown()
        {
            var result = await _getTipoSesionUseCase.ExecuteAsync(
                incluirInactivos: false
            );

            var lista = result.Exito && result.Datos != null
                ? result.Datos
                : new List<TipoSesionDto>();

            ViewBag.TiposSesion = new SelectList(lista, "IdTipoSesion", "Nombre");
        }

        // Construye el viewmodel del Index (con rutinas y id relación)
        private async Task<EntrenamientosViewModel> BuildRutinasViewModel(int idRutina)
        {
            await LoadTipoSesionDropdown();

            var result = await _getEntrenamientosUseCase.ExecuteAsync(
                IdRutina: idRutina,
                incluirInactivos: true
            );

            TempData[result.Exito ? "SuccessMessage" : "ErrorMessage"] =
                result.Mensaje ?? (result.Exito ? "Estado de la rutina actualizado." : "Error al actualizar el estado de la rutina.");

            return new EntrenamientosViewModel
            {
                IdRutina = idRutina,
                Entrenamientos = result.Exito ? result.Datos : new Collection<EntrenamientoDto>()
            };
        }

        // Retorna la vista Index cuando hay errores, cargando nuevamente rutinas
        private async Task<IActionResult> ReturnIndexViewWithData(int idRutina, EntrenamientosViewModel vm)
        {
            await LoadTipoSesionDropdown();

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
                IdTipoSesion = vm.EntrenamientoFormCreate.IdTipoSesion,
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
        public async Task<IActionResult> UpdateEntrenamiento(EntrenamientosViewModel vm)
        {
            ModelState.Clear();

            if (!TryValidateModel(vm.EntrenamientoFormUpdate, nameof(vm.EntrenamientoFormUpdate)))
            {
                TempData["ShowModal"] = "editEntrenamientoModal";
                return await ReturnIndexViewWithData(vm.EntrenamientoFormUpdate.IdRutina, vm);
            }

            var dto = new EntrenamientoDto
            {
                IdEntrenamiento = vm.EntrenamientoFormUpdate.IdEntrenamiento!.Value,
                IdRutina = vm.EntrenamientoFormUpdate.IdRutina,
                Nombre = vm.EntrenamientoFormUpdate.Nombre,
                IdTipoSesion = vm.EntrenamientoFormUpdate.IdTipoSesion,
                Objetivo = vm.EntrenamientoFormUpdate.Objetivo,
                DuracionEstimada = vm.EntrenamientoFormUpdate.DuracionEstimada,
                NivelEsfuerzoEsperado = vm.EntrenamientoFormUpdate.NivelEsfuerzoEsperado,
                FechaProgramada = vm.EntrenamientoFormUpdate.FechaProgramada,
                Estado = vm.EntrenamientoFormUpdate.Estado
            };

            var response = await _updateEntrenamientoUseCase.ExecuteAsync(dto);

            TempData[response.Exito ? "SuccessMessage" : "ErrorMessage"] =
                response.Mensaje ?? (response.Exito ? "Entrenamiento agregado." : "Error al agregar el entrenamiento.");

            return RedirectToAction("Index", new { idRutina = vm.EntrenamientoFormUpdate.IdRutina });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleEstadoEntrenamiento(int id, int idRutina)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Entrenamiento no encontrado.";
                return RedirectToAction("Index", "Home");
            }

            if (idRutina <= 0)
            {
                TempData["ErrorMessage"] = "Rutina no encontrada.";
                return RedirectToAction("Index", "Home");
            }

            var response = await _updateEntrenamientoUseCase.StatusExecuteAsync(id);

            TempData[response.Exito ? "SuccessMessage" : "ErrorMessage"] =
                response.Mensaje ?? (response.Exito ? "Estado del entrenamiento actualizado." : "Error al actualizar el estado del entrenamiento.");

            return RedirectToAction(nameof(Index), new { idRutina = idRutina });
        }
    }
}
