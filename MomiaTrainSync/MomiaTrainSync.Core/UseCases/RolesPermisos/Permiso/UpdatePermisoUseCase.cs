using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Core.UseCases.RolesPermisos.Rol;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Core.UseCases.RolesPermisos.Permiso
{
    public class UpdatePermisoUseCase
    {
        private readonly IPermisoRepository _permisoRepository;
        private readonly ILogErrorRepository _logErrorRepository;
        private readonly IMapper _mapper;

        public UpdatePermisoUseCase(
            IPermisoRepository permisoRepository,
            ILogErrorRepository logErrorRepository,
            IMapper mapper)
        {
            _permisoRepository = permisoRepository;
            _logErrorRepository = logErrorRepository;
            _mapper = mapper;
        }

        public async Task<Response<PermisoDto>> ExecuteAsync(PermisoDto dto)
        {
            try
            {
                var existing = await _permisoRepository.GetByIdAsync(dto.IdPermiso);
                if (existing == null)
                    return Response<PermisoDto>.Fail("Permiso no encontrado.");

                existing.Codigo = dto.Codigo;
                existing.Descripcion = dto.Descripcion;
                existing.Categoria = dto.Categoria;
                existing.Ruta = dto.Ruta;

                var updated = await _permisoRepository.UpdateAsync(existing);
                if (!updated)
                    return Response<PermisoDto>.Fail("No se pudo actualizar el permiso.");

                return Response<PermisoDto>.Success(dto, "Permiso actualizado correctamente.");

            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(UpdatePermisoUseCase)}.{nameof(ExecuteAsync)}", ex);
                return Response<PermisoDto>.Fail("Ocurrió un error al actualizar el rol.");
            }
        }

        public async Task<Response<PermisoDto>> StatusExecuteAsync(int IdPermiso)
        {
            try
            {
                var existing = await _permisoRepository.GetByIdAsync(IdPermiso);
                if (existing == null)
                    return Response<PermisoDto>.Fail("Permiso no encontrado.");

                existing.Estado = !existing.Estado;

                var updated = await _permisoRepository.UpdateAsync(existing);
                if (!updated)
                    return Response<PermisoDto>.Fail("No se pudo actualizar el permiso.");

                var updatedDto = new PermisoDto
                {
                    IdPermiso = existing.IdPermiso,
                    Codigo = existing.Codigo,
                    Descripcion = existing.Descripcion,
                    Ruta = existing.Ruta,
                    Estado = existing.Estado,
                };

                var mensaje = existing.Estado
                    ? "Permiso activado correctamente."
                    : "Permiso desactivado correctamente.";

                return Response<PermisoDto>.Success(updatedDto, mensaje);

            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(UpdatePermisoUseCase)}.{nameof(ExecuteAsync)}", ex);
                return Response<PermisoDto>.Fail("Ocurrió un error al actualizar el rol.");
            }
        }
    }
}
