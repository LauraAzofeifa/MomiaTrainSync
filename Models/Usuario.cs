using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MomiaTrainSync.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Correo { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Contrasena { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Rol { get; set; } = "Atleta";

        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } = "Activo";

        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [NotMapped]
        public Usuario? Entrenador { get; set; }

        [NotMapped]
        public string? ObjetivoTexto { get; set; }
    }
}
