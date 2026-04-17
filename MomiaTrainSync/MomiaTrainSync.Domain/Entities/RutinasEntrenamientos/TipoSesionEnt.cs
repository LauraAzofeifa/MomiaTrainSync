using MomiaTrainSync.Domain.Common;
using MomiaTrainSync.Domain.Entities.EntrenamientosZonas;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Domain.Entities.RutinasEntrenamientos
{
    public class TipoSesionEnt : ISoftDelete
    {
        public int IdTipoSesion { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Estado { get; set; }

        // Relaciones
        public ICollection<EntrenamientoEnt> Entrenamientos { get; set; } = new List<EntrenamientoEnt>();
    }
}
