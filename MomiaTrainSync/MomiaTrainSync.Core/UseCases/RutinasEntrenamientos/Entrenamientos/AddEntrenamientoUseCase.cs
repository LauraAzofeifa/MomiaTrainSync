using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.EntrenamientoZonas;
using MomiaTrainSync.Core.Helpers;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.RutinasEntrenamientos;
using MomiaTrainSync.Core.UseCases.Base;
using MomiaTrainSync.Domain.Entities.EntrenamientosZonas;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Core.UseCases.RutinasEntrenamientos.Entrenamientos
{
    public class AddEntrenamientoUseCase : BaseUseCase
    {
        private readonly IEntrenamientoRepository _repository;

        public AddEntrenamientoUseCase
            (
                IEntrenamientoRepository repository,
                ILogErrorRepository logError,
                IMapper mapper
            ) : base(mapper, logError)
        {
            _repository = repository;
        }

        public async Task<Response<EntrenamientoDto>> ExecuteAsync(EntrenamientoDto dto)
        {
            return await HandleAsync(
                async () =>
                {
                    var missing = ValidationHelper.ValidationRequired(
                        ("Nombre", dto.Nombre),
                        ("Tipo Sesion", dto.TipoSesion),
                        ("Objetivo", dto.Objetivo)
                    );

                    if (missing.Any())
                        return Response<EntrenamientoDto>.Fail(
                            $"Campos obligatorios: {string.Join(", ", missing)}"
                        );

                    dto.FechaCreacion = DateTime.Now;

                    var entity = _mapper!.Map<EntrenamientoEnt>(dto);

                    var created = await _repository.AddAsync(entity);

                    if (created == null)
                        return Response<EntrenamientoDto>.Fail("No se puede crear el entrenamiento.");

                    var resultDto = _mapper.Map<EntrenamientoDto>(created);

                    return Response<EntrenamientoDto>.Success(resultDto, "Entrenamiento creado existosamente.");
                }
            );
        }
    }
}
