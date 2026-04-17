using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Core.UseCases.Base;
using System;

namespace MomiaTrainSync.Core.UseCases.UsersUseCases
{
    public class UpdateUsuarioUseCase : BaseUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UpdateUsuarioUseCase(
            IUsuarioRepository usuarioRepository,
            ILogErrorRepository logError,
            IMapper mapper
            ) : base(mapper, logError)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Response<UsuarioDto>> ExecuteAsync(UsuarioDto _usuarioDto)
        {
            return await HandleAsync(
                async () =>
                {
                    // 1️⃣ Buscar el usuario actual en la base de datos
                    var existingUser = await _usuarioRepository.GetByIdAsync(_usuarioDto.Id);
                    if (existingUser == null)
                    {
                        return Response<UsuarioDto>.Fail("Usuario no encontrado.");
                    }

                    existingUser.ActualizarPerfil(
                        _usuarioDto.Telefono,
                        _usuarioDto.FechaNacimiento,
                        _usuarioDto.Biografia);

                    // Guardar cambios
                    var result = await _usuarioRepository.UpdateAsync(existingUser);

                    if (result == null)
                    {
                        return Response<UsuarioDto>.Fail("Ocurrió un error al actualizar el usuario.");
                    }

                    // 4️⃣ Retornar DTO actualizado
                    return Response<UsuarioDto>.Success(_usuarioDto, $"Sus datos han sido actualizados {_usuarioDto.Nombre}");
                }
            );
        }

        public async Task<Response<UsuarioDto>> CambiarRolAsync(int userId, int nuevoRolId)
        {
            return await HandleAsync(
                async () =>
                {
                    // Validar que el rol no este vacio
                    if (nuevoRolId == 0)
                        return Response<UsuarioDto>.Fail("Rol Invalido.");

                    // Obtener usuario
                    var existingUser = await _usuarioRepository.GetByIdAsync(userId);
                    if (existingUser == null)
                        return Response<UsuarioDto>.Fail("Usuario no encontrado.");

                    var previousRolId = existingUser.RolId;

                    // Validar si no hay cambios
                    if (previousRolId == nuevoRolId)
                        return Response<UsuarioDto>.Fail("El usuario ya tiene asignado ese rol.");

                    // Actualizar solo el Rol
                    existingUser.RolId = nuevoRolId;

                    // Guardar cambios
                    var result = await _usuarioRepository.UpdateAsync(existingUser);

                    if (result == null)
                        return Response<UsuarioDto>.Fail("Ocurrió un error al actualizar el rol del usuario.");

                    string mensaje = $"Rol del usuario '{existingUser.Nombre}' actualizado correctamente.";

                    return Response<UsuarioDto>.Success(new UsuarioDto
                    {
                        Id = existingUser.Id,
                        RolId = existingUser.RolId,
                        Nombre = existingUser.Nombre
                    }, mensaje);
                }
            );
        }

        public async Task<Response<UsuarioDto>> CambiarEstadoAsync(int userId)
        {
            return await HandleAsync(
                async () =>
                {
                    // Obtener usuario
                    var existingUser = await _usuarioRepository.GetByIdAsync(userId);
                    if (existingUser == null)
                        return Response<UsuarioDto>.Fail("Usuario no encontrado.");

                    existingUser.Estado = !existingUser.Estado;

                    var result = await _usuarioRepository.UpdateAsync(existingUser);

                    if (result == null)
                        return Response<UsuarioDto>.Fail("Ocurrió un error al actualizar el estado del usuario.");

                    string mensaje = existingUser.Estado
                        ? $"Usuario '{existingUser.Nombre}' activado correctamente."
                        : $"Usuario '{existingUser.Nombre}' desactivado correctamente.";

                    return Response<UsuarioDto>.Success(new UsuarioDto
                    {
                        Id = existingUser.Id,
                        Estado = existingUser.Estado,
                        Nombre = existingUser.Nombre
                    }, mensaje);
                }
            );
        }
    }
}
