using MomiaTrainSync.Domain.Entities.EntrenamientosZonas;
using MomiaTrainSync.Domain.Entities.RutinasAsignaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Domain.Entities.SesionesEntrenamiento
{
    public class SesionEntrenamientoEnt
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
        public ICollection<DetalleZonaSesionEnt> DetallesZona { get; set; } = new List<DetalleZonaSesionEnt>();
    }
}
