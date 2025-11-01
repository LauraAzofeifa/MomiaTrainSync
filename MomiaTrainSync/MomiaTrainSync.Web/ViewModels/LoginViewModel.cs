using System.ComponentModel.DataAnnotations;

namespace MomiaTrainSync.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El correo es requerido")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            ErrorMessage = "Correo no válido. Debe tener el formato usuario@dominio.com")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Contrasenna { get; set; } = string.Empty;

        public bool Recordarme { get; set; } = false;
    }
}
