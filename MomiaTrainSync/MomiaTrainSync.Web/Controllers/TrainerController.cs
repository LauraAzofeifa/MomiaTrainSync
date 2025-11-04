using Microsoft.AspNetCore.Mvc;

namespace MomiaTrainSync.Web.Controllers
{
    public class TrainerController : Controller
    {
        [HttpGet]
        public IActionResult ManageAthletes()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Trainings()
        {
            return View();
        }
    }
}
