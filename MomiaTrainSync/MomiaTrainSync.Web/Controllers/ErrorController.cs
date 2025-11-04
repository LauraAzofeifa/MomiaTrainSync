using Microsoft.AspNetCore.Mvc;

namespace MomiaTrainSync.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            return statusCode switch
            {
                403 => View("Error403"),
                404 => View("Error404"),
                _ => View("Error500"),
            };
        }

        [Route("Error")]
        public IActionResult Error()
        {
            return View("Error500");
        }
    }
}
