using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Core.Interfaces.Services;
using MomiaTrainSync.Core.UseCases.Base;
using System;

namespace MomiaTrainSync.Core.UseCases.AuthenticationUseCase
{
    public class LoginUseCase : BaseUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasherService _passwordHasherService;

        public LoginUseCase(
            IUsuarioRepository usuarioRepository,
            IPasswordHasherService passwordHasherService,
            ILogErrorRepository logErrorRepository,
            IMapper mapper
        ) : base(mapper, logErrorRepository)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasherService = passwordHasherService;
        }

        public async Task<Response<UsuarioDto>> ExecuteAsync(string correo, string contrasena)
        {
            return await HandleAsync(async () =>
            {
                var usuario = await _usuarioRepository.GetByEmailAsync(correo);

                if (usuario == null ||
                    !_passwordHasherService.VerifyPassword(usuario.ContrasennaHash, contrasena))
                {
                    return Response<UsuarioDto>.Fail("Correo o contraseña incorrectos.");
                }

                if (!usuario.Estado)
                    return Response<UsuarioDto>.Fail("Acceso denegado.");

                var usuarioDto = _mapper.Map<UsuarioDto>(usuario);

                return Response<UsuarioDto>.Success(usuarioDto, "Inicio de sesión exitoso.");
            });
        }
    }
}
