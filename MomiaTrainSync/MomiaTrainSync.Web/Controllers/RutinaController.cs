using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MomiaTrainSync.Core.DTOs.RutinasEntrenamientos;
using System.Collections.ObjectModel;

namespace MomiaTrainSync.Web.Controllers
{
    [Authorize]
    public class RutinaController : Controller
    {
        public IActionResult Index()
        {
            var dto = new Collection<RutinaDto>();
            return View(dto);
        }
    }
}
