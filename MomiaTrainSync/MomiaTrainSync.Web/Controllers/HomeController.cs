using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MomiaTrainSync.Core.UseCases.Home;
using MomiaTrainSync.Web.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace MomiaTrainSync.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly GetHomeUseCase _getHomeUseCase;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            GetHomeUseCase getHomeUseCase,
            ILogger<HomeController> logger)
        {
            _getHomeUseCase = getHomeUseCase;
            _logger = logger;
        }

        #region Private Methods
        private int? GetCurrentUserId()
        {
            var idValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(idValue, out var id) ? id : null;
        }
        #endregion

        [Authorize]
        public async Task<IActionResult> Index() {

            var userId = GetCurrentUserId();

            if (userId == null)
                return Unauthorized("No se pudo identificar al usuario.");

            var vm = await _getHomeUseCase.ExecuteAsync(userId.Value);

            return View(vm.Datos); 
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
