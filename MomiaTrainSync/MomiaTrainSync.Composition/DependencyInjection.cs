using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.Interfaces.Repositories;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Services;
using MomiaTrainSync.Core.Mappings;
using MomiaTrainSync.Core.UseCases.AuthenticationUseCase;
using MomiaTrainSync.Core.UseCases.UsersUseCases;
using MomiaTrainSync.Infrastructure.Persistence;
using MomiaTrainSync.Infrastructure.Repositories;
using MomiaTrainSync.Infrastructure.Repositories.Logging;
using MomiaTrainSync.Infrastructure.Services;

namespace MomiaTrainSync.Composition
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMomiaTrainSyncServices
            (this IServiceCollection services,
             IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<MomiaTrainSyncDbContext> (options =>
            options.UseSqlServer(connectionString));

            // Repositories
            services.AddScoped<ILogErrorRepository, LogErrorRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IPermisoRepository, PermisoRepository>();

            // Services
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            services.AddTransient<IEmailService, EmailService>();

            // Use Cases
            #region Usuarios
            services.AddScoped<LoginUseCase>();
            services.AddScoped<RegisterUseCase>();
            services.AddScoped<RecoverPasswordUseCase>();

            services.AddScoped<GetUsuariosUseCase>();
            services.AddScoped<UpdateUsuarioUseCase>();
            services.AddScoped<ChangePasswordUsuarioUseCase>();
            
            #endregion

            // AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}
