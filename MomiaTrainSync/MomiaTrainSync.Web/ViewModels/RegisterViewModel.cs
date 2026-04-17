using System.ComponentModel.DataAnnotations;

namespace MomiaTrainSync.Web.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [RegularExpression(@"^(?=.{2,50}$)(?!.*\s{2,})[A-Za-zÁÉÍÓÚáéíóúÑñ]+(?:\s[A-Za-zÁÉÍÓÚáéíóúÑñ]+)*$",
    ErrorMessage = "El nombre solo puede contener letras y espacios, sin espacios dobles y mínimo 2 caracteres.")]
        public string Nombre { get; set; } = string.Empty;


        [Required(ErrorMessage = "El apellido es requerido")]
        [RegularExpression(@"^(?=.{2,50}$)(?!.*\s{2,})[A-Za-zÁÉÍÓÚáéíóúÑñ]+(?:\s[A-Za-zÁÉÍÓÚáéíóúÑñ]+)*$",
            ErrorMessage = "El apellido solo puede contener letras y espacios, sin espacios dobles y mínimo 2 caracteres.")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es requerido")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            ErrorMessage = "Correo no válido. Debe tener el formato usuario@dominio.com")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=\S+$)(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]).{6,100}$",
            ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres, incluir mayúscula, minúscula, número, carácter especial y no contener espacios.")]
        public string Contrasenna { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        [Compare("Contrasenna", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarContrasenna { get; set; } = string.Empty;
    }
}
