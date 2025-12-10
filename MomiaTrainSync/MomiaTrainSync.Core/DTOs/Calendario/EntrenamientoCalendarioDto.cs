using MomiaTrainSync.Core.DTOs.EntrenamientoZonas;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Core.DTOs.Calendario
{
    public class EntrenamientoCalendarioDto
    {
        public int IdEntrenamiento { get; set; }
        public string NombreEntrenamiento { get; set; } = string.Empty;
        public string ObjetivoEntrenamiento { get; set; } = string.Empty;
        public int DuracionEstimada { get; set; }
        public string TipoSesionNombre { get; set; } = string.Empty;
        public DateOnly FechaProgramada { get; set; }

        // Rutina
        public int IdRutina { get; set; }
        public string NombreRutina { get; set; } = string.Empty;
        public string DescripcionRutina { get; set; } = string.Empty;

        public int IdEntrenador { get; set; }
        public string NombreEntrenador { get; set; } = string.Empty;

        public int IdAtleta { get; set; }
        public string NombreAtleta { get; set; } = string.Empty;
    }
}
