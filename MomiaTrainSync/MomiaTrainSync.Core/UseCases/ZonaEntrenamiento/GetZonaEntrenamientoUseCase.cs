using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.EntrenamientoZonas;
using MomiaTrainSync.Core.Interfaces.Repositories.EntrenamientosZonas;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.UseCases.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Core.UseCases.ZonaEntrenamiento
{
    public class GetZonaEntrenamientoUseCase : BaseUseCase
    {
        private readonly IZonaEntrenamientoRepository _zonaEntrenamientoRepository;

        public GetZonaEntrenamientoUseCase(
            IZonaEntrenamientoRepository zonaEntrenamientoRepository,
            IMapper? mapper, 
            ILogErrorRepository logger
            ) : base(mapper, logger)
        {
            _zonaEntrenamientoRepository = zonaEntrenamientoRepository;
        }

        public async Task<Response<IEnumerable<ZonaEntrenamientoDto>>> ExecuteAsync()
        {
            return await HandleAsync(
                async () =>
                {
                    var zonas = await _zonaEntrenamientoRepository.GetAllAsync();

                    if (zonas == null || !zonas.Any())
                    {
                        return Response<IEnumerable<ZonaEntrenamientoDto>>.Fail("No se encontraron zonas de entrenamiento.");
                    }

                    var dto = _mapper!.Map<IEnumerable<ZonaEntrenamientoDto>>(zonas);

                    return Response<IEnumerable<ZonaEntrenamientoDto>>.Success(dto, "Zonas de entrenamiento obtenidas exitosamente.");
                });
        }
    }
}
