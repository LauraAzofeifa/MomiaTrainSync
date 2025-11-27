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

        public UpdateRutinaUseCase(
            IRutinaRepository repository,
            ILogErrorRepository logError,
            IMapper mapper
            ) : base (mapper, logError)
        {
            _repository = repository;
        }

        public async Task<Response<RutinaDto>> ExecuteAsync(RutinaDto dto)
        {
            return await HandleAsync(async () =>
            {
                if (dto.IdRutina <= 0)
                    return Response<RutinaDto>.Fail("No se encontro la rutina");

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
                _mapper.Map(dto, entity);

                var updated = await _repository.UpdateAsync(entity);

                if (!updated)
                    return Response<RutinaDto>.Fail("No se pudo actualizar la rutina.");

                var resultDto = _mapper.Map<RutinaDto>(updated);

                return Response<RutinaDto>.Success(resultDto, "Rutina actualizada exitosamente.");
            });
        }

    }
}
