using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MomiaTrainSync.Core.Interfaces.Repositories;
using MomiaTrainSync.Core.Interfaces.Services;
using MomiaTrainSync.Core.UseCases;
using MomiaTrainSync.Infrastructure.Persistence;
using MomiaTrainSync.Infrastructure.Repositories;
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
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            // Services
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();

            // Use Cases
            services.AddScoped<LoginUseCase>();
            services.AddScoped<RegisterUseCase>();

            return services;
        }
    }
}
