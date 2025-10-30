using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Domain.Entities;
using MomiaTrainSync.Infrastructure.Persistence;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MomiaTrainSync.Infrastructure.Repositories.Logging
{
    public class LogErrorRepository : ILogErrorRepository
    {
        private readonly MomiaTrainSyncDbContext _context;

        public LogErrorRepository(MomiaTrainSyncDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddLogAsync(string origen, Exception exception)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(origen))
                    origen = "Origen desconocido";

                if (exception == null)
                    throw new ArgumentNullException(nameof(exception));

                var logEntry = new LogErrorEnt
                {
                    Origen = origen,
                    Mensaje = exception.Message,
                    ExcepcionInterna = exception.InnerException?.Message ?? string.Empty,
                    TrazaError = exception.StackTrace ?? string.Empty,
                    FechaRegistro = DateTime.UtcNow
                };

                await _context.LogErrores.AddAsync(logEntry);
                await _context.SaveChangesAsync();
            }
            catch (Exception exGuardar)
            {
                try
                {
                    Directory.CreateDirectory("logs");
                    var logFilePath = Path.Combine("logs", $"errores-{DateTime.UtcNow:yyyy-MM-dd}.txt");

                    var logTexto = $@"
                        =========================================
                        🕒 Fecha: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}
                        📍 Origen: {origen}
                        💬 Mensaje: {exception.Message}
                        🔁 Excepción interna: {exception.InnerException?.Message}
                        📄 Traza: {exception.StackTrace}

                        [⚠️ Error al guardar en BD]: {exGuardar.Message}
                        =========================================
                        ";

                    await File.AppendAllTextAsync(logFilePath, logTexto);
                }
                catch (Exception exArchivo)
                {
                    // Último recurso: salida en consola
                    Console.WriteLine($"[FATAL] Error al registrar en archivo: {exArchivo.Message}");
                }
            }
        }
    }
}
