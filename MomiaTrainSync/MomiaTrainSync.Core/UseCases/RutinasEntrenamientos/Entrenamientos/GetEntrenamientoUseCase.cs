using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.EntrenamientoZonas;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.RutinasEntrenamientos;
using MomiaTrainSync.Core.UseCases.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Core.UseCases.RutinasEntrenamientos.Entrenamientos
{
    public class GetEntrenamientoUseCase : BaseUseCase
    {
        private readonly IEntrenamientoRepository _repository;
        private readonly IRutinaRepository _rutinaRepository;

        public GetEntrenamientoUseCase
            (
                IEntrenamientoRepository repository,
                IRutinaRepository rutinaRepository,
                ILogErrorRepository logError,
                IMapper mapper
            ) : base(mapper, logError)
        {
            _repository = repository;
            _rutinaRepository = rutinaRepository;
        }

        public async Task<Response<IEnumerable<EntrenamientoDto>>> ExecuteAsync(
            int? IdEntrenamiento = null, 
            int? IdRutina = null,
            bool incluirInactivos = false
            )
        {
            return await HandleAsync(async () =>
            {
                // Validamos que la rutina este activa
                if (IdRutina.HasValue)
                {
                    var rutina = await _rutinaRepository.GetByIdAsync(IdRutina.Value);

                    if (rutina == null || (!incluirInactivos && !rutina.Estado))
                    {
                        return Response<IEnumerable<EntrenamientoDto>>.Fail("La rutina no existe o no está activa.");
                    }
                }

                var entrenamientos = await _repository.GetEntrenamientosAsync(
                    IdEntrenamiento: IdEntrenamiento,
                    IdRutina: IdRutina,
                    incluirInactivos: incluirInactivos
                    );

                if (entrenamientos is null || !entrenamientos.Any())
                {
                    return Response<IEnumerable<EntrenamientoDto>>.Fail("No se encontraron entrenamientos.");
                }

                var entrenamientosDto = _mapper!.Map<IEnumerable<EntrenamientoDto>>(entrenamientos);

                return Response<IEnumerable<EntrenamientoDto>>.Success(entrenamientosDto);
            });
        }
    }
}
