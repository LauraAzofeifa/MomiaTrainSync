using MomiaTrainSync.Core.DTOs.Calendario;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Core.Interfaces.Repositories.Calendario
{
    public interface ICalendarioRepository
    {
        Task<List<EntrenamientoCalendarioDto>> GetAllCalendar(
            int idUsuario,
            bool esEntrenador,
            int cantidad = 0,
            bool incluirInactivos = false
        );
    }
}
