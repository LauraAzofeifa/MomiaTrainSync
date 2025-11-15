using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using System;

namespace MomiaTrainSync.Core.UseCases.UsersUseCases
{
    public class UpdateUsuarioUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogErrorRepository _logErrorRepository;
        private readonly IMapper _mapper;

        public UpdateUsuarioUseCase(
            IUsuarioRepository usuarioRepository,
            ILogErrorRepository logErrorRepository,
            IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _logErrorRepository = logErrorRepository;
            _mapper = mapper;
        }

        public async Task<Response<UsuarioDto>> ExecuteAsync(UsuarioDto _usuarioDto)
        {
            try
            {
                // 1️⃣ Buscar el usuario actual en la base de datos
                var existingUser = await _usuarioRepository.GetByIdAsync(_usuarioDto.Id);
                if (existingUser == null)
                {
                    return Response<UsuarioDto>.Fail("Usuario no encontrado.");
                }

                // 2️⃣ Actualizar solo los campos que el usuario puede modificar
                existingUser.Nombre = _usuarioDto.Nombre;
                existingUser.Apellido = _usuarioDto.Apellido;
                existingUser.Correo = _usuarioDto.Correo;
                existingUser.Telefono = _usuarioDto.Telefono;
                existingUser.FechaCumpleannos = _usuarioDto.FechaCumpleannos;

                // 3️⃣ Guardar cambios
                var result = await _usuarioRepository.UpdateAsync(existingUser);

                if (!result)
                {
                    return Response<UsuarioDto>.Fail("Ocurrió un error al actualizar el usuario.");
                }

                // 4️⃣ Retornar DTO actualizado
                return Response<UsuarioDto>.Success(_usuarioDto, $"Sus datos han sido actualizados {_usuarioDto.Nombre}");
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(UpdateUsuarioUseCase)}.{nameof(ExecuteAsync)}", ex);
                return Response<UsuarioDto>.Fail("Ocurrió un error al actualizar el usuario.");
            }
        }

        public async Task<Response<UsuarioDto>> CambiarRolAsync(int userId, int nuevoRolId)
        {
            try
            {
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
                if (!result)
                    return Response<UsuarioDto>.Fail("Ocurrió un error al actualizar el rol del usuario.");

                string mensaje = $"Rol del usuario '{existingUser.Nombre}' actualizado correctamente.";

                return Response<UsuarioDto>.Success(new UsuarioDto
                {
                    Id = existingUser.Id,
                    RolId = existingUser.RolId,
                    Nombre = existingUser.Nombre
                }, mensaje);
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(UpdateUsuarioUseCase)}.{nameof(CambiarRolAsync)}", ex);
                return Response<UsuarioDto>.Fail("Ocurrió un error al actualizar el rol.");
            }
        }

        public async Task<Response<UsuarioDto>> CambiarEstadoAsync(int userId)
        {
            try
            {
                // Obtener usuario
                var existingUser = await _usuarioRepository.GetByIdAsync(userId);
                if (existingUser == null)
                    return Response<UsuarioDto>.Fail("Usuario no encontrado.");

                existingUser.Estado = !existingUser.Estado;

                var result = await _usuarioRepository.UpdateAsync(existingUser);
                if (!result)
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
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(UpdateUsuarioUseCase)}.{nameof(CambiarEstadoAsync)}", ex);
                return Response<UsuarioDto>.Fail("Ocurrió un error al actualizar el estado del usuario.");
            }
        }

    }
}
