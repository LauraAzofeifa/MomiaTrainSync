using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.RutinasEntrenamientos;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.RutinasEntrenamientos;
using MomiaTrainSync.Core.UseCases.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Core.UseCases.RutinasEntrenamientos.TipoSesion
{
    public class GetTipoSesionUseCase : BaseUseCase
    {
        private readonly ITipoSesionRepository _tipoSesionRepository;

        public GetTipoSesionUseCase(
            ITipoSesionRepository tipoSesionRepository,
            IMapper? mapper, 
            ILogErrorRepository logger
            ) : base(mapper, logger)
        {
            _tipoSesionRepository = tipoSesionRepository;
        }

        public async Task<Response<List<TipoSesionDto>>> ExecuteAsync(bool incluirInactivos)
        {
            return await HandleAsync(async () =>
            {
                var listTipoSesion = await _tipoSesionRepository.GetAllAsync(
                    includeInactive: incluirInactivos
                );

                if (listTipoSesion == null || !listTipoSesion.Any())
                {
                    return Response<List<TipoSesionDto>>.Fail("No se encontraron tipos de sesión.");
                }

                var dto = _mapper!.Map<List<TipoSesionDto>>(listTipoSesion);

                return Response<List<TipoSesionDto>>.Success(dto);
            });
        }
    }
}
