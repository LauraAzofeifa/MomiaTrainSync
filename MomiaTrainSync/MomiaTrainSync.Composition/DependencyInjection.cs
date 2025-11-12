using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.Interfaces.Repositories.EntrenadorAtleta;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Core.Interfaces.Services;
using MomiaTrainSync.Core.Mappings;
using MomiaTrainSync.Core.UseCases.AuthenticationUseCase;
using MomiaTrainSync.Core.UseCases.RolesPermisos;
using MomiaTrainSync.Core.UseCases.TrainerAthleteUseCase;
using MomiaTrainSync.Core.UseCases.UsersUseCases;
using MomiaTrainSync.Infrastructure.Persistence;
using MomiaTrainSync.Infrastructure.Repositories.EntrenadorAtleta;
using MomiaTrainSync.Infrastructure.Repositories.Logging;
using MomiaTrainSync.Infrastructure.Repositories.RolesPermisos;
using MomiaTrainSync.Infrastructure.Repositories.UsuariosRoles;
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
            services.AddScoped<IEntrenadorAtletaRepository, EntrenadorAtletaRepository>();

            // Services
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            services.AddTransient<IEmailService, EmailService>();

            // Use Cases
            #region Roles y Permisos
            services.AddScoped<GetPermisosUseCase>();
            #endregion

            #region Usuarios
            services.AddScoped<LoginUseCase>();
            services.AddScoped<RegisterUseCase>();
            services.AddScoped<RecoverPasswordUseCase>();

            services.AddScoped<GetUsuariosUseCase>();
            services.AddScoped<UpdateUsuarioUseCase>();
            services.AddScoped<ChangePasswordUsuarioUseCase>();
            #endregion

            #region EntrenadorAtleta
            services.AddScoped<GetEntrenadorAtletaUseCase>();
            services.AddScoped<AddAthleteUseCase>();
            services.AddScoped<DeleteAthleteUseCase>();
            #endregion

            // AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}
