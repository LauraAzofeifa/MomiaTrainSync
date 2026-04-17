using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Core.Interfaces.Services;
using MomiaTrainSync.Core.UseCases.Base;
using System;

namespace MomiaTrainSync.Core.UseCases.UsersUseCases
{
    public class ChangePasswordUsuarioUseCase : BaseUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasherService _passwordHasherService;

        public ChangePasswordUsuarioUseCase(
            IUsuarioRepository usuarioRepository, 
            ILogErrorRepository logError, 
            IPasswordHasherService passwordHasherService,
            IMapper mapper
            ) : base(mapper, logError)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasherService = passwordHasherService;
        }

        public async Task<Response<bool>> ExecuteAsync(
            int usuarioId,
            string oldPassword,
            string newPassword)
        {
            return await HandleAsync(async () =>
            {
                var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);

                if (usuario == null)
                    return Response<bool>.Fail("Usuario no encontrado");

                // Validar contraseña actual
                if (!_passwordHasherService.VerifyPassword(
                        usuario.ContrasennaHash,
                        oldPassword))
                {
                    return Response<bool>.Fail("La contraseña antigua es incorrecta");
                }

                // Generar hash
                var nuevoHash = _passwordHasherService.HashPassword(newPassword);

                // 🔹 Usar método de la entidad
                usuario.CambiarContrasenna(nuevoHash);

                var updated = await _usuarioRepository.UpdateAsync(usuario);

                if (updated == null)
                    return Response<bool>.Fail("No se pudo actualizar la contraseña");

                return Response<bool>.Success(true, "Contraseña cambiada con éxito");
            });
        }
    }
}
