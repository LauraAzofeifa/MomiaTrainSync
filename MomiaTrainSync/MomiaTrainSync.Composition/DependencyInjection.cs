using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MomiaTrainSync.Core.Interfaces.Repositories.Base;
using MomiaTrainSync.Core.Interfaces.Repositories.Calendario;
using MomiaTrainSync.Core.Interfaces.Repositories.EntrenadorAtleta;
using MomiaTrainSync.Core.Interfaces.Repositories.EntrenamientosZonas;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.RutinasEntrenamientos;
using MomiaTrainSync.Core.Interfaces.Repositories.SesionesEntrenamientos;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Core.Interfaces.Services;
using MomiaTrainSync.Core.Mappings;
using MomiaTrainSync.Core.UseCases.AuthenticationUseCase;
using MomiaTrainSync.Core.UseCases.Calendario;
using MomiaTrainSync.Core.UseCases.Home;
using MomiaTrainSync.Core.UseCases.RolesPermisos.Permiso;
using MomiaTrainSync.Core.UseCases.RolesPermisos.Rol;
using MomiaTrainSync.Core.UseCases.RolesPermisos.RolPermiso;
using MomiaTrainSync.Core.UseCases.RutinasEntrenamientos.Entrenamientos;
using MomiaTrainSync.Core.UseCases.RutinasEntrenamientos.Rutinas;
using MomiaTrainSync.Core.UseCases.RutinasEntrenamientos.TipoSesion;
using MomiaTrainSync.Core.UseCases.TrainerAthleteUseCase;
using MomiaTrainSync.Core.UseCases.UsersUseCases;
using MomiaTrainSync.Core.UseCases.ZonaEntrenamiento;
using MomiaTrainSync.Infrastructure.Persistence;
using MomiaTrainSync.Infrastructure.Repositories.Base;
using MomiaTrainSync.Infrastructure.Repositories.Calendario;
using MomiaTrainSync.Infrastructure.Repositories.EntrenadorAtleta;
using MomiaTrainSync.Infrastructure.Repositories.EntrenamientosZonas;
using MomiaTrainSync.Infrastructure.Repositories.Logging;
using MomiaTrainSync.Infrastructure.Repositories.RutinasEntrenamientos;
using MomiaTrainSync.Infrastructure.Repositories.SesionesEntrenamientos;
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
            services.AddScoped<ITipoSesionRepository, TipoSesionRepository>();
            services.AddScoped<IEntrenamientoRepository, EntrenamientoRepository>();

            // Calendario
            services.AddScoped<ICalendarioRepository, CalendarioRepository>();

            // EntrenamientoZonas
            services.AddScoped<IZonaEntrenamientoRepository, ZonaEntrenamientoRepository>();
            services.AddScoped<IDetalleZonaPlanRepository, DetalleZonaPlanRepository>();

            // SesionesEntrenamiento
            services.AddScoped<ISesionesEntrenamientoRepository, SesionesEntrenamientoRepository>();
            services.AddScoped<IDetalleZonaSesionRepository, DetalleZonaSesionRepository>();

            // Errores
            services.AddScoped<ILogErrorRepository, LogErrorRepository>();

            #endregion

            // Services
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            services.AddTransient<IEmailService, EmailService>();

            // Use Cases

            #region Home
            services.AddScoped<GetHomeUseCase>();
            #endregion

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

            services.AddScoped<GetTipoSesionUseCase>();

            services.AddScoped<AddEntrenamientoUseCase>();
            services.AddScoped<GetEntrenamientoUseCase>();
            services.AddScoped<UpdateEntrenamientoUseCase>();
            #endregion

            #region Calendario
            services.AddScoped<GetCalendarioUseCase>();
            #endregion

            #region ZonasEntrenamientos
            services.AddScoped<GetZonaEntrenamientoUseCase>();
            #endregion
            // AutoMapper
            services.AddAutoMapper(cfg => { cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxNzkzMTQ1NjAwIiwiaWF0IjoiMTc2MTY3NjIwNCIsImFjY291bnRfaWQiOiIwMTlhMmMxM2YyZDM3ODIyOTg2NDdkMTUwNmMzNWI5OCIsImN1c3RvbWVyX2lkIjoiY3RtXzAxazhwMWI2YmJuYmJ2YXc1M25jZGZhZjZwIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.JnCO3esdM9K9Wh3nSfwHsZcVeHc_2wYKStqrHpNpm0Lh9jHhmm7s8ij1WqVtEHaJ10kseYQMHqaZQBMa6mJuyRbQlNOuqv_RzYtGx1Gp9SwiN5oanJKEyW4BNhuiLrgAQPuAiGESN9-YHnbSLIrZZcwrmdVbtPJGLN18oYSwlt7W-flhWG3yZpbp4TOggz5Wx1gamLEhIHGycZLTqF7oGV9xZQ5hZ_1lhEd8Wr6l1D1bBq-ZtN2OaUccN7Y5vOauQvxVwDSPDpEkIubGTdabSWZf2aHRyEwcTFqfebb5EZaoKp108HP9g1dq39IJd9H_MOdS3QrRZk4bU_Yjm2Ycag"; }
            ,typeof(MappingProfile));

            return services;
        }
    }
}
