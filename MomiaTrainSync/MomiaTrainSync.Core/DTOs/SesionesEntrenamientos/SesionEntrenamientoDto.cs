using MomiaTrainSync.Core.DTOs.RutinasAsignaciones;
using MomiaTrainSync.Domain.Entities.EntrenamientosZonas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.DTOs.SesionesEntrenamientos
{
    public class SesionEntrenamientoDto
    {
        public int IdSesion { get; set; }
        public int IdEntrenamiento { get; set; }
        public DateTime FechaEjecucion { get; set; }
        public int DuracionReal { get; set; }
        public byte NivelEsfuerzoPercibido { get; set; }
        public string? Comentarios { get; set; }
        public decimal CargaTotal { get; set; }

        // Relaciones
        public EntrenamientoEnt? Entrenamiento { get; set; }
        public List<DetalleZonaSesionDto> DetallesZona { get; set; } = new();
    }
}
