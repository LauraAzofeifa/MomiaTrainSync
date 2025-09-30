using System.ComponentModel.DataAnnotations;

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
    }
}
