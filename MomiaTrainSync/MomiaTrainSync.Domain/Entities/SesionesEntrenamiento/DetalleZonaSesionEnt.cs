using MomiaTrainSync.Domain.Entities.EntrenamientosZonas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Domain.Entities.SesionesEntrenamiento
{
    public class DetalleZonaSesionEnt
    {
        public int IdDetalleZonaSesion { get; set; }
        public int IdSesion { get; set; }
        public int IdZona { get; set; }
        public int MinutosCompletados { get; set; }

        // Relaciones
        public SesionEntrenamientoEnt? Sesion { get; set; }
        public ZonaEntrenamientoEnt? Zona { get; set; }
    }
}
