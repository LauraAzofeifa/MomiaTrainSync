using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MomiaTrainSync.Models
{
    public class Objetivo
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }

        [Required]
        [MaxLength(500)]
        public string TextoObjetivo { get; set; } = string.Empty;

        public bool Estado { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaModificacion { get; set; }

        public Usuario? Usuario { get; set; }
    }
}
