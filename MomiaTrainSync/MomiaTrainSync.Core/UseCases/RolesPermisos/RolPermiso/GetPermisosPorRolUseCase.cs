using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Core.UseCases.Base;
using System;

namespace MomiaTrainSync.Core.UseCases.RolesPermisos.RolPermiso
{
    public class GetPermisosPorRolUseCase : BaseUseCase
    {
        private readonly IRolRepository _rolRepository;

        public GetPermisosPorRolUseCase(
            IRolRepository rolRepository,
            ILogErrorRepository logError,
            IMapper mapper
            ) : base(mapper, logError)
        {
            _rolRepository = rolRepository;
        }

        public async Task<Response<List<PermisoDto>>> ExecuteAsync(int idRol)
        {
            return await HandleAsync(
                async () =>
                {
                    if (idRol <= 0)
                        return Response<List<PermisoDto>>.Fail("El ID del rol no es válido.");

                    var permisos = await _rolRepository.GetPermisosPorRolAsync(idRol);

                    if (permisos == null || !permisos.Any())
                        return Response<List<PermisoDto>>.Fail("Este rol no tiene permisos asignados.");

                    var permisosDto = _mapper!.Map<List<PermisoDto>>(permisos);

                    return Response<List<PermisoDto>>.Success(permisosDto);
                }
            );
        }
    }
}
