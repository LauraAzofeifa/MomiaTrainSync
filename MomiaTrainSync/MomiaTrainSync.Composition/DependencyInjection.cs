using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MomiaTrainSync.Core.Interfaces.Repositories.Base;
using MomiaTrainSync.Core.Interfaces.Repositories.EntrenadorAtleta;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.RutinasEntrenamientos;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Core.Interfaces.Services;
using MomiaTrainSync.Core.Mappings;
using MomiaTrainSync.Core.UseCases.AuthenticationUseCase;
using MomiaTrainSync.Core.UseCases.RolesPermisos.Permiso;
using MomiaTrainSync.Core.UseCases.RolesPermisos.Rol;
using MomiaTrainSync.Core.UseCases.RolesPermisos.RolPermiso;
using MomiaTrainSync.Core.UseCases.RutinasEntrenamientos.Entrenamientos;
using MomiaTrainSync.Core.UseCases.RutinasEntrenamientos.Rutinas;
using MomiaTrainSync.Core.UseCases.TrainerAthleteUseCase;
using MomiaTrainSync.Core.UseCases.UsersUseCases;
using MomiaTrainSync.Infrastructure.Persistence;
using MomiaTrainSync.Infrastructure.Repositories.Base;
using MomiaTrainSync.Infrastructure.Repositories.EntrenadorAtleta;
using MomiaTrainSync.Infrastructure.Repositories.Logging;
using MomiaTrainSync.Infrastructure.Repositories.RutinasEntrenamientos;
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
            var connectionString = configuration.GetConnectionString("ServerConnection");

            services.AddDbContext<MomiaTrainSyncDbContext> (options =>
            options.UseSqlServer(connectionString));

            #region Repositories

            // Base
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Roles
            services.AddScoped<IPermisoRepository, PermisoRepository>();
            services.AddScoped<IRolRepository, RolRepository>();
            services.AddScoped<IRolPermisoRepository, RolPermisoRepository>();

            // Usuarios
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IEntrenadorAtletaRepository, EntrenadorAtletaRepository>();

            // RutinaEntrenamiento
            services.AddScoped<IRutinaRepository, RutinaRepository>();
            services.AddScoped<IEntrenamientoRepository, EntrenamientoRepository>();

            // Errores
            services.AddScoped<ILogErrorRepository, LogErrorRepository>();

            #endregion

            // Services
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            services.AddTransient<IEmailService, EmailService>();

            // Use Cases
            #region Roles y Permisos
            // Rol
            services.AddScoped<AddRolUseCase>();
            services.AddScoped<GetRolesUseCase>();
            services.AddScoped<UpdateRolUseCase>();

            // RolPermiso
            services.AddScoped<AsignarPermisosUseCase>();
            services.AddScoped<GetPermisosPorRolUseCase>();

            // Permiso
            services.AddScoped<AddPermisoUseCase>();
            services.AddScoped<GetPermisosUseCase>();
            services.AddScoped<UpdatePermisoUseCase>();
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

            #region RutinasEntrenamientos
            services.AddScoped<AddRutinaUseCase>();
            services.AddScoped<GetRutinaUseCase>();
            services.AddScoped<UpdateRutinaUseCase>();

            services.AddScoped<AddEntrenamientoUseCase>();
            services.AddScoped<GetEntrenamientoUseCase>();
            services.AddScoped<UpdateEntrenamientoUseCase>();
            #endregion

            // AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}
