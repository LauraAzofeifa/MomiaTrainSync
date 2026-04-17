using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Domain.Entities.EntrenamientosZonas
{
    public class DetalleZonaPlanEnt
    {
        public int IdDetalleZonaPlan { get; set; }
        public int IdEntrenamiento { get; set; }
        public int IdZona { get; set; }
        public int MinutosPlanificados { get; set; }

        // Relaciones
        public EntrenamientoEnt? Entrenamiento { get; set; }
        public ZonaEntrenamientoEnt? Zona { get; set; }
    }
}
