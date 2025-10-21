using System.Collections.Generic;
namespace MomiaTrainSync.Models
{
    public class Entrenador
    {
        public int EntrenadorId { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public ICollection<Atleta>? Atletas { get; set; }
    }
}
