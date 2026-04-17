using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.UseCases.AuthenticationUseCase
{
    public class RecoverPasswordUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly ILogErrorRepository _logErrorRepository;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailService;

        public RecoverPasswordUseCase(
            IUsuarioRepository usuarioRepository,
            IPasswordHasherService passwordHasherService,
            ILogErrorRepository logErrorRepository,
            IMapper mapper,
            IEmailSender emailService)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasherService = passwordHasherService;
            _logErrorRepository = logErrorRepository;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<Response<bool>> ExecuteAsync(string email)
        
        
        {
            try
            {
                var user = await _usuarioRepository.GetByEmailAsync(email);

                if (user == null)
                {
                    return Response<bool>.Fail("Usuario no encontrado");
                }

                // Generar una nueva contraseña temporal
                var tempPassword = Guid.NewGuid().ToString().Substring(0, 8);
                user.ContrasennaHash = _passwordHasherService.HashPassword(tempPassword);

                // Enviar la nueva contraseña por correo electrónico
                var subject = "Recuperación de Contraseña";
                var body = $"Su nueva contraseña temporal es: {tempPassword}. Por favor, cambie su contraseña después de iniciar sesión.";
                await _emailService.SendAsync(email, subject, body);

                // Guardamos la nueva contrasenna temporal
                await _usuarioRepository.UpdateAsync(user); 

                return Response<bool>.Success(true, "Correo de Recuperación enviado");
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(RecoverPasswordUseCase)}.{nameof(ExecuteAsync)}", ex);
                return Response<bool>.Fail("Ocurrió un error al recuperar la contraseña");
            }
        }
    }
}
