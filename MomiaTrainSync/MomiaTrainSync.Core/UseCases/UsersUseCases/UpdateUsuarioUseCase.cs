using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs;
using MomiaTrainSync.Core.Interfaces.Repositories;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
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

    }
}
