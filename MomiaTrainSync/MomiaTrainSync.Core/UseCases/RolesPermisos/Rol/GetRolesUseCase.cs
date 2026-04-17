using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Core.UseCases.Base;

namespace MomiaTrainSync.Core.UseCases.RolesPermisos.Rol
{
    public class GetRolesUseCase : BaseUseCase
    {
        private readonly IRolRepository _rolRepository;

        public GetRolesUseCase(
            IRolRepository rolRepository, 
            ILogErrorRepository logError, 
            IMapper mapper
            ) : base(mapper, logError)
        {
            _rolRepository = rolRepository;
        }

        public async Task<Response<List<RolDto>>> ExecuteAsync(bool incluirInactivos)
        {
            return await HandleAsync(
                async () =>
                {
                    var roles = await _rolRepository.GetAllAsync(includeInactive: true);

                    if (roles == null || !roles.Any())
                        return Response<List<RolDto>>.Fail("No hay roles registrados.");

                    var rolesDto = _mapper!.Map<List<RolDto>>(roles);
                    return Response<List<RolDto>>.Success(rolesDto);

                }
            );
        }
    }
}
