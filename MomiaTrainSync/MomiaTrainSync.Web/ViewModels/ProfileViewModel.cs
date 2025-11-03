using MomiaTrainSync.Core.DTOs;
using System.ComponentModel.DataAnnotations;

namespace MomiaTrainSync.Web.ViewModels
{
    public class ProfileViewModel
    {
        public UsuarioDto Details { get; set; } = new();
        public UpdateProfileViewModel Update { get; set; } = new();
        public ChangePasswordViewModel ChangePassword { get; set; } = new();
    }

    public class UpdateProfileViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        [StringLength(50)]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es requerido")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo válido")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El telefono es requerido")]
        [Phone(ErrorMessage = "Debe ingresar un número de teléfono válido")]
        public string Telefono { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime FechaCumpleannos { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "La contraseña es requerida")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]).+$",
            ErrorMessage = "La contraseña debe contener al menos una letra mayúscula, una letra minúscula, un número y un carácter especial.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
