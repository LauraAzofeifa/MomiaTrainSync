using MomiaTrainSync.Core.DTOs.EntrenamientoZonas;
using System.ComponentModel.DataAnnotations;

namespace MomiaTrainSync.Web.ViewModels.RutinasEntrenamientos
{
    public class EntrenamientosViewModel
    {
        public int IdRutina { get; set; }

        // Listado
        public IEnumerable<EntrenamientoDto>? Entrenamientos { get; set; }

        // Formularios
        public EntrenamientoFormViewModel EntrenamientoFormCreate { get; set; } = new();
        public EntrenamientoFormViewModel EntrenamientoFormUpdate { get; set; } = new();
    }

    public class EntrenamientoFormViewModel
    {
        public int? IdEntrenamiento { get; set; }
        public int IdRutina { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de sesión es requerido")]
        public int IdTipoSesion { get; set; }

        [Required(ErrorMessage = "El objetivo es requerido")]
        public string Objetivo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La duración estimada es requerida")]
        public int DuracionEstimada { get; set; }

        [Required(ErrorMessage = "El nivel de esfuerzo esperado es requerido")]
        public byte NivelEsfuerzoEsperado { get; set; }

        [Required(ErrorMessage = "La fecha programada es requerida")]
        public DateOnly FechaProgramada { get; set; }

        public bool Estado { get; set; } = true;
    }

}
