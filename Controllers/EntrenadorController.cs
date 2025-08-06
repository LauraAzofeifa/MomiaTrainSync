using Microsoft.AspNetCore.Mvc;
using MomiaTrainSync.Models; 
using System.Collections.Generic;
using System.Linq;

namespace MomiaTrainSync.Controllers
{
    public class EntrenadorController : Controller
    {
        public IActionResult Dashboard() => View();

        public IActionResult Biblioteca() => View();

        public IActionResult Atleta() => View();

        public IActionResult Calendario() => View();

        // Acción para ver detalle del atleta
        public IActionResult Detalle(int id)
        {
            // Simulación de datos de atletas
            var atletas = new List<AtletaViewModel>
            {
                new AtletaViewModel { Id = 1, Nombre = "Andrea Vargas", Objetivo = "Maratón 42K", Estado = "Activo" },
                new AtletaViewModel { Id = 2, Nombre = "Marco Jiménez", Objetivo = "Recuperación post-lesión", Estado = "Inactivo" }
            };

            var atleta = atletas.FirstOrDefault(a => a.Id == id);
            if (atleta == null)
            {
                return NotFound();
            }

            return View(atleta); // Vista: Views/Entrenador/Detalle.cshtml
        }
    }
}
