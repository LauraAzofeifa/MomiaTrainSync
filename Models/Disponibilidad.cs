namespace MomiaTrainSync.Models
{
    public class Disponibilidad
    {
        public int Id { get; set; } // Id único
        public int AtletaId { get; set; } // ID del atleta relacionado

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public string Tipo { get; set; } // "Limitada" o "No disponible"
        public List<string> DeportesDisponibles { get; set; } = new();
        public string Descripcion { get; set; } // Ej: "Lesión en rodilla"

        // Para facilitar la visualización en calendario
        public string Color =>
            Tipo == "No disponible" ? "#d3d3d3" : "#ffe08a"; // gris o amarillo
    }
}
