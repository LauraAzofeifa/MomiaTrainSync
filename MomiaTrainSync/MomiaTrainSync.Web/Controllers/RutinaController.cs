using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MomiaTrainSync.Core.DTOs.RutinasEntrenamientos;
using MomiaTrainSync.Core.UseCases.RutinasEntrenamientos.Rutinas;
using MomiaTrainSync.Web.ViewModels.RutinasEntrenamientos;
using System.Collections.ObjectModel;

namespace MomiaTrainSync.Web.Controllers
{
    [Authorize]
    public class RutinaController : Controller
    {
        private readonly AddRutinaUseCase _addRutinaUseCase;
        private readonly GetRutinaUseCase _getRutinaUseCase;
        private readonly UpdateRutinaUseCase _updateRutinaUseCase;

        public RutinaController(
            AddRutinaUseCase addRutinaUseCase,
            GetRutinaUseCase getRutinaUseCase,
            UpdateRutinaUseCase updateRutinaUseCase
            )
        {
            _addRutinaUseCase = addRutinaUseCase;
            _getRutinaUseCase = getRutinaUseCase;
            _updateRutinaUseCase = updateRutinaUseCase;
        }

        #region Private Methods
        // Construye el viewmodel del Index (con rutinas y id relación)
        private async Task<RutinaViewModel> BuildRutinasViewModel(int idRelacion)
        {
            var result = await _getRutinaUseCase.ExecuteAsync(
                idRelacion: idRelacion,
                incluirInactivos: true
            );

            return new RutinaViewModel
            {
                IdRelacion = idRelacion,
                Rutinas = result.Exito ? result.Datos : new Collection<RutinaDto>()
            };
        }

        // Retorna la vista Index cuando hay errores, cargando nuevamente rutinas
        private async Task<IActionResult> ReturnIndexViewWithData(int idRelacion, RutinaViewModel vm)
        {
            var recargado = await BuildRutinasViewModel(idRelacion);

            vm.Rutinas = recargado.Rutinas;
            vm.IdRelacion = idRelacion;

            return View(nameof(Index), vm);
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            var vm = await BuildRutinasViewModel(id);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddRutina(RutinaViewModel vm)
        {
            int idRelacion = vm.RutinaFormCreate.IdRelacion ?? 0;

            ModelState.Clear();

            // Validación: campos del formulario
            if (!TryValidateModel(vm.RutinaFormCreate, nameof(vm.RutinaFormCreate)))
            {
                TempData["ShowModal"] = "addRutinaModal";
                return await ReturnIndexViewWithData(idRelacion, vm);
            }

            // Validación: IdRelacion inválido
            if (idRelacion == 0)
            {
                ModelState.AddModelError("RutinaFormCreate.IdRelacion",
                    "Debe seleccionar una relación válida.");

                TempData["ShowModal"] = "addRutinaModal";
                return await ReturnIndexViewWithData(idRelacion, vm);
            }

            // Guardar rutina
            var dto = new RutinaDto
            {
                IdRelacion = idRelacion,
                Nombre = vm.RutinaFormCreate.Nombre,
                Descripcion = vm.RutinaFormCreate.Descripcion,
                Estado = vm.RutinaFormCreate.Estado,
                FechaCreacion = DateTime.Now
            };

            var response = await _addRutinaUseCase.ExecuteAsync(dto);

            TempData[response.Exito ? "SuccessMessage" : "ErrorMessage"] =
                response.Mensaje ?? (response.Exito ? "Rutina agregada." : "Error al agregar la rutina.");

            return RedirectToAction(nameof(Index), new { id = idRelacion });
        }



        [HttpPost]
        public async Task<IActionResult> UpdateRutina(RutinaViewModel vm)
        {
            int idRelacion = vm.RutinaFormUpdate.IdRelacion ?? 0;

            ModelState.Clear();

            // Validaciones del formulario UPDATE
            if (!TryValidateModel(vm.RutinaFormUpdate, nameof(vm.RutinaFormUpdate)))
            {
                TempData["ShowModal"] = "editRutinaModal";

                return await ReturnIndexViewWithData(idRelacion, vm);
            }

            // Validar IdRelacion (por seguridad)
            if (idRelacion == 0)
            {
                ModelState.AddModelError("RutinaFormUpdate.IdRelacion", "La relación es inválida.");
                TempData["ShowModal"] = "editRutinaModal";

                return await ReturnIndexViewWithData(idRelacion, vm);
            }

            // Construcción del DTO
            var dto = new RutinaDto
            {
                IdRutina = vm.RutinaFormUpdate.IdRutina ?? 0,
                IdRelacion = idRelacion,
                Nombre = vm.RutinaFormUpdate.Nombre,
                Descripcion = vm.RutinaFormUpdate.Descripcion,
                Estado = vm.RutinaFormUpdate.Estado
            };

            var response = await _updateRutinaUseCase.ExecuteAsync(dto);

            TempData[response.Exito ? "SuccessMessage" : "ErrorMessage"] =
                response.Mensaje ?? (response.Exito ? "Rutina actualizada." : "Error al actualizar la rutina.");

            // Redirigir al Index con el id de la relación
            return RedirectToAction(nameof(Index), new { id = idRelacion });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleEstadoRutina(RutinaViewModel vm)
        {
            if (vm.IdRelacion == 0)
            {
                TempData["ErrorMessage"] = "La relación es inválida.";
                return RedirectToAction("ManageAthletes", "Users");
            }

            if (vm.IdRutina == 0)
            {
                TempData["ErrorMessage"] = "La rutina es inválida.";
                return RedirectToAction(nameof(Index), new { id = vm.RutinaFormUpdate.IdRelacion });
            }

            var response = await _updateRutinaUseCase.StatusExecuteAsync((int)vm.IdRutina!);

            TempData[response.Exito ? "SuccessMessage" : "ErrorMessage"] =
                response.Mensaje ?? (response.Exito ? "Estado de la rutina actualizado." : "Error al actualizar el estado de la rutina.");

            return RedirectToAction(nameof(Index), new { id = response.Datos?.IdRelacion } );
        }
    }
}
