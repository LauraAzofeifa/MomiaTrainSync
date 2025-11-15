using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;

namespace MomiaTrainSync.Core.UseCases.RolesPermisos.Rol
{
    public class GetRolesUseCase
    {
        private readonly IRolRepository _rolRepository;
        private readonly ILogErrorRepository _logErrorRepository;
        private readonly IMapper _mapper;

        public GetRolesUseCase(IRolRepository rolRepository, ILogErrorRepository logErrorRepository, IMapper mapper)
        {
            _rolRepository = rolRepository;
            _logErrorRepository = logErrorRepository;
            _mapper = mapper;
        }

        public async Task<Response<List<RolDto>>> ExecuteAsync()
        {
            try
            {
                var roles = await _rolRepository.GetAllAsync();

                if (roles == null || !roles.Any())
                    return Response<List<RolDto>>.Fail("No hay roles registrados.");

                var rolesDto = _mapper.Map<List<RolDto>>(roles);
                return Response<List<RolDto>>.Success(rolesDto);
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(GetRolesUseCase)}.{nameof(ExecuteAsync)}", ex);
                return Response<List<RolDto>>.Fail("Ocurrió un error al obtener los roles.");
            }
        }
    }
}
