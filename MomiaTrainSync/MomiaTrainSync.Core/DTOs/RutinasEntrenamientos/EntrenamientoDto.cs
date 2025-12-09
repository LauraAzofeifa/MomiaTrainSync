using MomiaTrainSync.Core.DTOs.RutinasEntrenamientos;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Domain.Entities.RutinasEntrenamientos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.DTOs.EntrenamientoZonas
{
    public class EntrenamientoDto
    {
        public int IdEntrenamiento { get; set; }
        public int IdRutina { get; set; } // FK
        public string Nombre { get; set; } = string.Empty;
        public int IdTipoSesion { get; set; } // Fk
        public string Objetivo { get; set; } = string.Empty;
        public int DuracionEstimada { get; set; }
        public byte NivelEsfuerzoEsperado { get; set; }
        public string? Descripcion { get; set; }
        public DateOnly FechaProgramada { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Estado { get; set; }

        public UsuarioDto? Entrenador { get; set; }
        public TipoSesionDto? TipoSesion { get; set; }
        public List<DetalleZonaPlanDto> ZonasPlan { get; set; } = new();
    }
}
