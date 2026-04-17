using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Domain.Entities
{
    public class LogErrorEnt
    {
        public int Id { get; set; }
        public string Origen { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public string ExcepcionInterna { get; set; } = string.Empty;
        public string? TrazaError { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
