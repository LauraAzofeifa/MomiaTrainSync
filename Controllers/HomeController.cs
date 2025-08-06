using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MomiaTrainSync.Models;

namespace MomiaTrainSync.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult About() => View();
        public IActionResult Contact() => View();
    }
}
