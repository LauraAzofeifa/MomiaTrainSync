using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.RutinasEntrenamientos;
using MomiaTrainSync.Core.Helpers;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.RutinasEntrenamientos;
using MomiaTrainSync.Core.UseCases.Base;
using MomiaTrainSync.Domain.Entities.RutinasAsignaciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Core.UseCases.RutinasEntrenamientos.Rutinas
{
    public class UpdateRutinaUseCase : BaseUseCase
    {
        private readonly IRutinaRepository _repository;
        private readonly IEntrenamientoRepository _entrenamientoRepository;

        public UpdateRutinaUseCase(
            IRutinaRepository repository,
            IEntrenamientoRepository entrenamientoRepository,
            ILogErrorRepository logError,
            IMapper mapper
            ) : base (mapper, logError)
        {
            _repository = repository;
            _entrenamientoRepository = entrenamientoRepository;
        }

        public async Task<Response<RutinaDto>> ExecuteAsync(RutinaDto dto)
        {
            return await HandleAsync(async () =>
            {
                if (dto.IdRutina <= 0)
                    return Response<RutinaDto>.Fail("No se encontro la rutina.");

                // Validaciones requeridas
                var missing = ValidationHelper.ValidationRequired(
                    ("Nombre", dto.Nombre),
                    ("Descripcion", dto.Descripcion)
                );

                if (missing.Any())
                    return Response<RutinaDto>.Fail(
                        $"Campos obligatorios: {string.Join(", ", missing)}"
                    );

                // Buscar la entidad existente
                var entity = await _repository.GetByIdAsync(dto.IdRutina!);

                if (entity == null)
                    return Response<RutinaDto>.Fail("La rutina no existe.");

                // Mantener fecha original de creación
                dto.FechaCreacion = entity.FechaCreacion;

                // Mapear los cambios (solo campos editables)
                _mapper!.Map(dto, entity);

                var updated = await _repository.UpdateAsync(entity);

                if (updated == null)
                    return Response<RutinaDto>.Fail("No se pudo actualizar la rutina.");

                var resultDto = _mapper.Map<RutinaDto>(updated);

                return Response<RutinaDto>.Success(resultDto, "Rutina actualizada exitosamente.");
            });
        }

        public async Task<Response<RutinaDto>> StatusExecuteAsync(int id)
        {
            return await HandleAsync(
                async () =>
                {
                    var existing = await _repository.GetByIdAsync(id);

                    if (existing == null)
                        return Response<RutinaDto>.Fail("Rutina no encontrada.");

                    var toggled = await _repository.ToggleEstadoAsync(id);

                    if (!toggled)
                        return Response<RutinaDto>.Fail("No se pudo actualizar el estado de la rutina.");

                    var updated = await _repository.GetByIdAsync(id);
                    if (updated == null)
                        return Response<RutinaDto>.Fail("Error al obtener rutina actualizada.");

                    if (!updated.Estado) // false
                        await _entrenamientoRepository.ToggleEstadoByRutinaIdAsync(id, false);

                    if (updated.Estado) // true
                        await _entrenamientoRepository.ToggleEstadoByRutinaIdAsync(id, true);

                    var dto = _mapper!.Map<RutinaDto>(updated);

                    var mensaje = updated!.Estado
                        ? "Rutina activada correctamente."
                        : "Rutina desactivada correctamente junto a sus entrenamientos.";

                    return Response<RutinaDto>.Success(dto, mensaje);
                }
            );
        }

    }
}
