using Microsoft.AspNetCore.Mvc;
using MomiaTrainSync.Models;
using System;
using System.Collections.Generic;

namespace MomiaTrainSync.Controllers
{
    public class AtletaController : Controller
    {
        public IActionResult Dashboard()
        {
            var modelo = new AtletaViewModel
            {
                Id = 1,
                Nombre = "Andrea Vargas",
                Objetivo = "Maratón 42K",
                Estado = "Activo",
                Edad = 30,
                Genero = "Femenino",
                HistorialMedico = "Ninguna condición médica reportada.",
                RutinaDeHoy = "Intervalos VO2Max - 40 min",
                NotaDelCoach = "Recuerda enfocarte en la técnica de respiración y mantener el ritmo en la última serie.",

                Rutina = new List<RutinaItem>
                {
                    new RutinaItem { Dia = "Lunes", Tipo = "Fondo", Duracion = "60 min", Intensidad = "Moderada" },
                    new RutinaItem { Dia = "Martes", Tipo = "Intervalos", Duracion = "40 min", Intensidad = "Alta" },
                    new RutinaItem { Dia = "Miércoles", Tipo = "Recuperación", Duracion = "30 min", Intensidad = "Baja" }
                },

                Comentarios = new List<ComentarioItem>
                {
                    new ComentarioItem { Fecha = "2024-11-03", Texto = "Me costó mantener el ritmo en la última serie." },
                    new ComentarioItem { Fecha = "2024-11-02", Texto = "Entrenamiento completado sin molestias." }
                },

                HistorialRutinas = new List<RutinaResumen>
                {
                    new RutinaResumen { Fecha = "2024-10-29", Descripcion = "Entrenamiento de técnica con técnica de pisada." },
                    new RutinaResumen { Fecha = "2024-10-31", Descripcion = "Rodaje de recuperación." }
                }
            };

            return View(modelo);
        }

        public IActionResult DetalleRutina()
        {
            var modelo = new AtletaViewModel
            {
                Nombre = "Andrea Vargas",
                RutinaDeHoy = "Intervalos VO2Max - 40 min",
                NotaDelCoach = "Recuerda enfocarte en la técnica de respiración y mantener el ritmo en la última serie."
            };

            return View(modelo);
        }
    }
}
