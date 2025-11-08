using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs;
using MomiaTrainSync.Core.Interfaces.Repositories;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Services;
using System;

namespace MomiaTrainSync.Core.UseCases.AuthenticationUseCase
{
    public class LoginUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly ILogErrorRepository _logErrorRepository;
        private readonly IMapper _mapper;

        public LoginUseCase(
            IUsuarioRepository usuarioRepository,
            IPasswordHasherService passwordHasherService,
            ILogErrorRepository logErrorRepository,
            IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasherService = passwordHasherService;
            _logErrorRepository = logErrorRepository;
            _mapper = mapper;
        }

        public async Task<Response<UsuarioDto>> ExecuteAsync(string correo, string contrasena)
        {
            try
            {
                var usuario = await _usuarioRepository.GetByEmailAsync(correo);

                if (usuario == null || !_passwordHasherService.VerifyPassword(usuario.ContrasennaHash, contrasena))
                {
                    return Response<UsuarioDto>.Fail("Correo o contraseña incorrectos.");
                }

                var usuarioDto = _mapper.Map<UsuarioDto>(usuario);

                return Response<UsuarioDto>.Success(usuarioDto, "Inicio de sesión exitoso.");
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(LoginUseCase)}.{nameof(ExecuteAsync)}", ex);
                return Response<UsuarioDto>.Fail("Ocurrió un error al procesar el inicio de sesión.");
            }
        }
    }
}
