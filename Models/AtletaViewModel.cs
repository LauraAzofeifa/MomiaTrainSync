using System;
using System.Collections.Generic;

namespace MomiaTrainSync.Models
{
    public class AtletaViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Objetivo { get; set; }
        public string Estado { get; set; }

        // Información general
        public int Edad { get; set; }
        public string Genero { get; set; }
        public string HistorialMedico { get; set; }

        // Historial de rutinas
        public List<RutinaResumen> HistorialRutinas { get; set; } = new List<RutinaResumen>();

        // Rutina actual
        public List<RutinaItem> Rutina { get; set; } = new List<RutinaItem>();

        // Comentarios del atleta
        public List<ComentarioItem> Comentarios { get; set; } = new List<ComentarioItem>();

        // Rutina destacada de hoy (usada en el Dashboard)
        public string RutinaDeHoy { get; set; }

        // Último comentario reciente o planificador del coach
        public string NotaDelCoach { get; set; }
    }

    public class RutinaItem
    {
        public string Dia { get; set; }
        public string Tipo { get; set; }
        public string Duracion { get; set; }
        public string Intensidad { get; set; }
    }

    public class ComentarioItem
    {
        public string Fecha { get; set; }
        public string Texto { get; set; }
    }

    public class RutinaResumen
    {
        public string Fecha { get; set; }
        public string Descripcion { get; set; }
    }
}
