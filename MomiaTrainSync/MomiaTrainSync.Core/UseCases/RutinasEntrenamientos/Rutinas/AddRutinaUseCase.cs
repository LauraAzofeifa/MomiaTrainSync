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
    public class AddRutinaUseCase : BaseUseCase
    {
        private readonly IRutinaRepository _repo;

        public AddRutinaUseCase(
            IRutinaRepository repo,
            ILogErrorRepository logError,
            IMapper mapper
            ) : base (mapper, logError)
        {
            _repo = repo;
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

                var created = await _repo.AddAsync(entity);
                if (created == null)
                    return Response<RutinaDto>.Fail("No se pudo crear la rutina.");

                var resultDto = _mapper.Map<RutinaDto>(created);

                return Response<RutinaDto>.Success(resultDto, "Rutina creada exitosamente.");
            });
        }
    }
}
