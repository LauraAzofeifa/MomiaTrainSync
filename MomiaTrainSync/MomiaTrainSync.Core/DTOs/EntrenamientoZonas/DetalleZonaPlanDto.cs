using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.DTOs.EntrenamientoZonas
{
    public class DetalleZonaPlanDto
    {
        public int IdDetalleZonaPlan { get; set; }
        public int IdEntrenamiento { get; set; }
        public int IdZona { get; set; }
        public int MinutosPlanificados { get; set; }

        public ZonaEntrenamientoDto? Zona { get; set; }
    }
}
