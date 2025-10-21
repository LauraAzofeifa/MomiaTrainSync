using System.ComponentModel.DataAnnotations;

namespace MomiaTrainSync.Models.ViewModels
{
    public class PerfilViewModel
    {
        public int UsuarioId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Correo { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Contrasena { get; set; } = string.Empty;

        public int? ObjetivoId { get; set; }

        [MaxLength(500)]
        [Display(Name = "Objetivo Personal")]
        public string? ObjetivoTexto { get; set; }
        public bool CambiarContrasena { get; set; }
    }
}
