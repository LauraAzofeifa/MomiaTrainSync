using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using System;

namespace MomiaTrainSync.Core.UseCases.RolesPermisos.Rol
{
    public class AddRolUseCase
    {
        private readonly IRolRepository _rolRepository;
        private readonly ILogErrorRepository _logErrorRepository;
        private readonly IMapper _mapper;

        public AddRolUseCase(IRolRepository rolRepository, ILogErrorRepository logErrorRepository, IMapper mapper)
        {
            _rolRepository = rolRepository;
            _logErrorRepository = logErrorRepository;
            _mapper = mapper;
        }

        public async Task<Response<RolDto>> ExecuteAsync(RolDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Nombre))
                    return Response<RolDto>.Fail("El nombre del rol es obligatorio.");

                var entity = _mapper.Map<RolEnt>(dto);
                var result = await _rolRepository.AddAsync(entity);

                if (result == null)
                    return Response<RolDto>.Fail("No se pudo crear el rol.");

                var rolDto = _mapper.Map<RolDto>(result);
                return Response<RolDto>.Success(rolDto, "Rol creado correctamente.");
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(AddRolUseCase)}.{nameof(ExecuteAsync)}", ex);
                return Response<RolDto>.Fail("Ocurrió un error al crear el rol.");
            }
        }
    }
}
