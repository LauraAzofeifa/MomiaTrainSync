using Microsoft.AspNetCore.Mvc;

namespace MomiaTrainSync.Web.Controllers
{
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
