using MomiaTrainSync.Domain.Entities.SesionesEntrenamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Domain.Entities.EntrenamientosZonas
{
    public class ZonaEntrenamientoEnt
    {
        public int IdZona { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Factor { get; set; }

        public ICollection<DetalleZonaPlanEnt> DetalleZonaPlanes { get; set; } = new List<DetalleZonaPlanEnt>();
        public ICollection<DetalleZonaSesionEnt> DetalleZonaSesiones { get; set; } = new List<DetalleZonaSesionEnt>();
    }
}
