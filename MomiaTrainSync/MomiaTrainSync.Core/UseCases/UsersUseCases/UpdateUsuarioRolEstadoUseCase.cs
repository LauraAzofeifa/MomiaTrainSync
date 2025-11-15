using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using System;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.UseCases.UsersUseCases
{
    public class UpdateUsuarioRolEstadoUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogErrorRepository _logErrorRepository;
        private readonly IMapper _mapper;

        public UpdateUsuarioRolEstadoUseCase(
            IUsuarioRepository usuarioRepository,
            ILogErrorRepository logErrorRepository,
            IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _logErrorRepository = logErrorRepository;
            _mapper = mapper;
        }

        public async Task<Response<UsuarioDto>> ExecuteAsync(UsuarioDto usuarioDto)
        {
            try
            {
                // 1️⃣ Obtener usuario existente
                var existingUser = await _usuarioRepository.GetByIdAsync(usuarioDto.Id);
                if (existingUser == null)
                {
                    return Response<UsuarioDto>.Fail("Usuario no encontrado.");
                }

                var previousEstado = existingUser.Estado;
                var previousRolId = existingUser.RolId;

                // 2️⃣ Actualizar solo Estado y Rol
                existingUser.Estado = usuarioDto.Estado;
                existingUser.RolId = usuarioDto.RolId;

                // 3️⃣ Guardar cambios
                var result = await _usuarioRepository.UpdateAsync(existingUser);

                if (!result)
                {
                    return Response<UsuarioDto>.Fail("Ocurrió un error al actualizar el usuario.");
                }

                // Mensaje personalizado usando operador ternario
                string mensaje = previousEstado != usuarioDto.Estado && previousRolId != usuarioDto.RolId
                    ? $"Estado y rol del usuario '{usuarioDto.Nombre}' actualizados correctamente."
                    : previousEstado != usuarioDto.Estado
                        ? $"Estado del usuario '{usuarioDto.Nombre}' actualizado correctamente."
                        : previousRolId != usuarioDto.RolId
                            ? $"Rol del usuario '{usuarioDto.Nombre}' actualizado correctamente."
                            : $"No se realizaron cambios en el usuario '{usuarioDto.Nombre}'.";

                return Response<UsuarioDto>.Success(usuarioDto, mensaje);
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(UpdateUsuarioRolEstadoUseCase)}.{nameof(ExecuteAsync)}", ex);
                return Response<UsuarioDto>.Fail("Ocurrió un error al actualizar el usuario.");
            }
        }
    }
}
