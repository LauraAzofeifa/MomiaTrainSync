using Microsoft.AspNetCore.Mvc;

namespace MomiaTrainSync.Web.Controllers.Admin
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
