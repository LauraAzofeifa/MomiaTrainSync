using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.Helpers;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Core.UseCases.Base;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using System;

namespace MomiaTrainSync.Core.UseCases.RolesPermisos.Rol
{
    public class AddRolUseCase : BaseUseCase
    {
        private readonly IRolRepository _rolRepository;

        public AddRolUseCase(
            IRolRepository rolRepository,
            ILogErrorRepository logErrorRepository,
            IMapper mapper
        ) : base(mapper, logErrorRepository)
        {
            _rolRepository = rolRepository;
        }

        public async Task<Response<RolDto>> ExecuteAsync(RolDto dto)
        {
            return await HandleAsync(
                async () =>
                {
                    // 🔎 Validación con helper estándar
                    var missing = ValidationHelper.ValidationRequired(
                        ("Nombre del Rol", dto.Nombre)
                    );

                    if (missing.Any())
                        return Response<RolDto>.Fail(
                            $"Los siguientes campos son obligatorios: {string.Join(", ", missing)}."
                        );

                    // Mapear y guardar
                    var entity = _mapper!.Map<RolEnt>(dto);
                    var created = await _rolRepository.AddAsync(entity);

                    if (created == null)
                        return Response<RolDto>.Fail("No se pudo crear el rol.");

                    var resultDto = _mapper.Map<RolDto>(created);

                    return Response<RolDto>.Success(resultDto, "Rol creado correctamente.");
                }
            );
        }
    }
}
