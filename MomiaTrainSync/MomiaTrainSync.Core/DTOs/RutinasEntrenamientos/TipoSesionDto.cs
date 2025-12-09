using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Core.DTOs.RutinasEntrenamientos
{
    public class TipoSesionDto
    {
        public int IdTipoSesion { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }
}
