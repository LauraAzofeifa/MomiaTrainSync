using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.EntrenamientoZonas;
using MomiaTrainSync.Core.Helpers;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.RutinasEntrenamientos;
using MomiaTrainSync.Core.UseCases.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Core.UseCases.RutinasEntrenamientos.Entrenamientos
{
    public class UpdateEntrenamientoUseCase : BaseUseCase
    {
        private readonly IEntrenamientoRepository _repository;
        private readonly IRutinaRepository _rutinaRepository;

        public UpdateEntrenamientoUseCase(
            IEntrenamientoRepository repository,
            IRutinaRepository rutinaRepository,
            ILogErrorRepository logError,
            IMapper mapper
            ) : base(mapper, logError)
        {
            _repository = repository;
            _rutinaRepository = rutinaRepository;
        }

        public async Task<Response<EntrenamientoDto>> ExecuteAsync(EntrenamientoDto dto)
        {
            return await HandleAsync(async () =>
            {
                if (dto.IdRutina <= 0)
                    return Response<EntrenamientoDto>.Fail("No se encontro el entrenamiento.");

                var missing = ValidationHelper.ValidationRequired(
                    ("Nombre", dto.Nombre),
                    ("Tipo Sesión", dto.IdTipoSesion),
                    ("Objetivo", dto.Objetivo),
                    ("Duración Estimada", dto.DuracionEstimada),
                    ("Nivel de Esfuerzo", dto.NivelEsfuerzoEsperado),
                    ("Fecha Programada", dto.FechaProgramada)
                );

                if (missing.Any())
                    return Response<EntrenamientoDto>.Fail(
                        $"Campos obligatorios: {string.Join(", ", missing)}"
                    );

                var entity = await _repository.GetByIdAsync(dto.IdEntrenamiento);

                if (entity == null)
                    return Response<EntrenamientoDto>.Fail("El entrenamiento no existe.");

                dto.FechaCreacion = entity.FechaCreacion;

                _mapper!.Map(dto, entity);

                var updated = await _repository.UpdateAsync(entity);

                if (updated == null)
                    return Response<EntrenamientoDto>.Fail("No se pudo actualizar la rutina.");

                var resultDto = _mapper.Map<EntrenamientoDto>(updated);

                return Response<EntrenamientoDto>.Success(resultDto, "Entrenamiento actualizado correctamente.");
            });
        }

        public async Task<Response<EntrenamientoDto>> StatusExecuteAsync(int entrenamientoId)
        {
            return await HandleAsync(
                async () =>
                {
                    var existing = await _repository.GetByIdAsync(entrenamientoId);

                    if (existing == null)
                        return Response<EntrenamientoDto>.Fail("Entrenamiento no encontrado.");

                    var toggled = await _repository.ToggleEstadoAsync(entrenamientoId);

                    if (!toggled)
                        return Response<EntrenamientoDto>.Fail("No se pudo actualizar el estado de la rutina.");

                    var updated = await _repository.GetByIdAsync(entrenamientoId);

                    if (updated == null)
                        return Response<EntrenamientoDto>.Fail("Error al obtener la rutina actualizada.");

                    var dto = _mapper!.Map<EntrenamientoDto>(updated);

                    var mensaje = updated!.Estado
                        ? "Entrenamiento activado correctamente."
                        : "Entrenamiento desactivado correctamente.";

                    return Response<EntrenamientoDto>.Success(dto, mensaje);
                }
            );
        }
    }
}
