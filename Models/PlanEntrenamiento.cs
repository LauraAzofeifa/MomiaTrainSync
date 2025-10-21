using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MomiaTrainSync.Models
{
    public class PlanEntrenamiento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdAtleta { get; set; }

        [ForeignKey(nameof(IdAtleta))]
        public Usuario? Atleta { get; set; }

        [Required]
        public int IdCreador { get; set; }

        [ForeignKey(nameof(IdCreador))]
        public Usuario? Creador { get; set; }

        [Required]
        [MaxLength(200)]
        public string Objetivo { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.MultilineText)]
        public string Detalle { get; set; } = string.Empty;

        [Required]
        public bool Estado { get; set; } = true;

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaModificacion { get; set; }
    }

}
