using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using System;

namespace MomiaTrainSync.Core.UseCases.RolesPermisos.Rol
{
    public class UpdateRolUseCase
    {
        private readonly IRolRepository _rolRepository;
        private readonly ILogErrorRepository _logErrorRepository;
        private readonly IMapper _mapper;

        public UpdateRolUseCase(IRolRepository rolRepository, ILogErrorRepository logErrorRepository, IMapper mapper)
        {
            _rolRepository = rolRepository;
            _logErrorRepository = logErrorRepository;
            _mapper = mapper;
        }

        public async Task<Response<RolDto>> ExecuteAsync(RolDto dto)
        {
            try
            {
                var existing = await _rolRepository.GetByIdAsync(dto.IdRol);
                if (existing == null)
                    return Response<RolDto>.Fail("Rol no encontrado.");

                existing.Nombre = dto.Nombre;
                existing.Descripcion = dto.Descripcion;
                existing.Estado = dto.Estado;

                var updated = await _rolRepository.UpdateAsync(existing);
                if (!updated)
                    return Response<RolDto>.Fail("No se pudo actualizar el rol.");

                return Response<RolDto>.Success(dto, "Rol actualizado correctamente.");
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(UpdateRolUseCase)}.{nameof(ExecuteAsync)}", ex);
                return Response<RolDto>.Fail("Ocurrió un error al actualizar el rol.");
            }
        }

        public async Task<Response<RolDto>> StatusExecuteAsync(int IdRol)
        {
            try
            {
                var existing = await _rolRepository.GetByIdAsync(IdRol);

                if (existing == null)
                    return Response<RolDto>.Fail("Rol no encontrado.");

                // Invertir el estado
                existing.Estado = !existing.Estado;

                var updated = await _rolRepository.UpdateAsync(existing);

                if (!updated)
                    return Response<RolDto>.Fail("No se pudo actualizar el rol.");

                // Convertimos entidad → DTO
                var updatedDto = new RolDto
                {
                    IdRol = existing.IdRol,
                    Nombre = existing.Nombre,
                    Descripcion = existing.Descripcion,
                    Estado = existing.Estado
                };

                var mensaje = existing.Estado
                    ? "Rol activado correctamente."
                    : "Rol desactivado correctamente.";

                return Response<RolDto>.Success(updatedDto, mensaje);
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync(
                    $"{nameof(UpdateRolUseCase)}.{nameof(StatusExecuteAsync)}",
                    ex
                );

                return Response<RolDto>.Fail("Ocurrió un error al actualizar el rol.");
            }
        }

    }
}
