using MomiaTrainSync.Core.DTOs.EntrenamientoZonas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.DTOs.SesionesEntrenamientos
{
    public class DetalleZonaSesionDto
    {
        public int IdDetalleZonaSesion { get; set; }
        public int IdSesion { get; set; }
        public int IdZona { get; set; }
        public int MinutosCompletados { get; set; }

        public ZonaEntrenamientoDto? Zona { get; set; }
    }
}
