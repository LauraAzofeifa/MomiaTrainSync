using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using System;

namespace MomiaTrainSync.Core.UseCases.RolesPermisos.RolPermiso
{
    public class GetPermisosPorRolUseCase
    {
        private readonly IRolRepository _rolRepository;
        private readonly ILogErrorRepository _logErrorRepository;
        private readonly IMapper _mapper;

        public GetPermisosPorRolUseCase(
            IRolRepository rolRepository,
            ILogErrorRepository logErrorRepository,
            IMapper mapper)
        {
            _rolRepository = rolRepository;
            _logErrorRepository = logErrorRepository;
            _mapper = mapper;
        }

        public async Task<Response<List<PermisoDto>>> ExecuteAsync(int idRol)
        {
            try
            {
                if (idRol <= 0)
                    return Response<List<PermisoDto>>.Fail("El ID del rol no es válido.");

                var permisos = await _rolRepository.GetPermisosPorRolAsync(idRol);

                if (permisos == null || !permisos.Any())
                    return Response<List<PermisoDto>>.Fail("Este rol no tiene permisos asignados.");

                var permisosDto = _mapper.Map<List<PermisoDto>>(permisos);

                return Response<List<PermisoDto>>.Success(permisosDto);
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(GetPermisosPorRolUseCase)}.{nameof(ExecuteAsync)}", ex);
                return Response<List<PermisoDto>>.Fail("Ocurrió un error al obtener los permisos del rol.");
            }
        }
    }
}
