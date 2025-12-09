using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.RutinasEntrenamientos;
using MomiaTrainSync.Core.Helpers;
using MomiaTrainSync.Core.Interfaces.Repositories.EntrenadorAtleta;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.RutinasEntrenamientos;
using MomiaTrainSync.Core.UseCases.Base;
using MomiaTrainSync.Domain.Entities.RutinasAsignaciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Core.UseCases.RutinasEntrenamientos.Rutinas
{
    public class AddRutinaUseCase : BaseUseCase
    {
        private readonly IRutinaRepository _repo;
        private readonly IEntrenadorAtletaRepository _entrenadorAtletaRepository;

        public AddRutinaUseCase(
            IRutinaRepository repo,
            IEntrenadorAtletaRepository entrenadorAtletaRepository,
            ILogErrorRepository logError,
            IMapper mapper
            ) : base (mapper, logError)
        {
            _repo = repo;
            _entrenadorAtletaRepository = entrenadorAtletaRepository;
        }

        public async Task<Response<RutinaDto>> ExecuteAsync(RutinaDto dto)
        {
            return await HandleAsync(async () =>
            {
                var missing = ValidationHelper.ValidationRequired(
                    ("Nombre", dto.Nombre),
                    ("Descripcion", dto.Descripcion)
                );

                if (missing.Any())
                    return Response<RutinaDto>.Fail(
                        $"Campos obligatorios: {string.Join(", ", missing)}"
                    );

                dto.FechaCreacion = DateTime.Now;

                var entity = _mapper!.Map<RutinaEnt>(dto);

                // Validamos que el entrenador-atleta exista y esté activo ya que si no, no se puede crear la rutina
                var relacion = await _entrenadorAtletaRepository.GetByIdAsync(entity.IdRelacion);
                if (relacion == null || !relacion.Estado)
                    return Response<RutinaDto>.Fail("La relación entrenador-atleta no existe o no está activa.");
                if (relacion.Atleta == null || !relacion.Atleta.Estado)
                    return Response<RutinaDto>.Fail("El atleta asociado a la relación no existe o no está activo.");
                if (relacion.Entrenador == null || !relacion.Entrenador.Estado)
                    return Response<RutinaDto>.Fail("El entrenador asociado a la relación no existe o no está activo.");

                var created = await _repo.AddAsync(entity);
                if (created == null)
                    return Response<RutinaDto>.Fail("No se pudo crear la rutina.");

                var resultDto = _mapper.Map<RutinaDto>(created);

                return Response<RutinaDto>.Success(resultDto, "Rutina creada exitosamente.");
            });
        }
    }
}
