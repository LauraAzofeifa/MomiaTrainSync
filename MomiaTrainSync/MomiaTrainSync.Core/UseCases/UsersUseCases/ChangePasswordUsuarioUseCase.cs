using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.Interfaces.Repositories;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Services;
using System;

namespace MomiaTrainSync.Core.UseCases.UsersUseCases
{
    public class ChangePasswordUsuarioUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogErrorRepository _logErrorRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IMapper _mapper;

        public ChangePasswordUsuarioUseCase(
            IUsuarioRepository usuarioRepository, 
            ILogErrorRepository logErrorRepository, 
            IPasswordHasherService passwordHasherService,
            IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _logErrorRepository = logErrorRepository;
            _passwordHasherService = passwordHasherService;
            _mapper = mapper;
        }

        public async Task<Response<bool>> ExecuteAsync(int usuarioId, string oldPassword, string newPassword)
        {
            try
            {
                var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
                if (usuario == null)
                    return Response<bool>.Fail("Usuario no encontrado");
                
                if (!_passwordHasherService.VerifyPassword(usuario.ContrasennaHash, oldPassword))
                    return Response<bool>.Fail("La contraseña antigua es incorrecta");

                usuario.ContrasennaHash = _passwordHasherService.HashPassword(newPassword);
                var result = await _usuarioRepository.UpdateAsync(usuario);
                return Response<bool>.Success(result, "Contraseña cambiada con éxito");
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync("ChangePasswordUsuarioUseCase", ex);
                return Response<bool>.Fail("Error al cambiar la contraseña");
            }
        }
    }
}
