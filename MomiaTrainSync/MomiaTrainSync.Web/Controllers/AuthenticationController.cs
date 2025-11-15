using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.UseCases.AuthenticationUseCase;
using MomiaTrainSync.Web.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MomiaTrainSync.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly LoginUseCase _loginUseCase;
        private readonly RegisterUseCase _registerUseCase;
        private readonly RecoverPasswordUseCase _recoverPasswordUseCase;

        public AuthenticationController(
            LoginUseCase loginUseCase, 
            RegisterUseCase registerUseCase,
            RecoverPasswordUseCase recoverPasswordUseCase)
        {
            _loginUseCase = loginUseCase;
            _registerUseCase = registerUseCase;
            _recoverPasswordUseCase = recoverPasswordUseCase;
        }

        #region Metodo Login

        [HttpGet]
        public IActionResult Login()
        {
            var vm = new LoginViewModel
            {
                Correo = TempData["PreFillCorreo"] as string ?? string.Empty
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var response = await _loginUseCase.ExecuteAsync(vm.Correo, vm.Contrasenna);

            if (!response.Exito)
            {
                ModelState.AddModelError("", response.Mensaje);
                return View();
            }

            var usuario = response.Datos!;
            var claims = GetClaims(usuario);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = vm.Recordarme,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) // Expira en x
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );

            return RedirectToAction("Index", "Home");
        }

        // Lista de claims
        public static List<Claim> GetClaims(UsuarioDto dto)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.Name, dto.Nombre!),
                new Claim(ClaimTypes.Email, dto.Correo),
                new Claim(ClaimTypes.Role, dto.Rol!.Nombre),
                new Claim(ClaimTypes.NameIdentifier, dto.Id.ToString()),
            };
        }
        #endregion

        #region Metodo Registro

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var response = await _registerUseCase.ExecuteAsync(vm.Nombre, vm.Apellido, vm.Correo, vm.Contrasenna);

            if (!response.Exito)
            {
                TempData["ErrorMessage"] = response.Mensaje;
                return View(vm);
            }

            // Guardar un mensaje temporal que se muestra en el login
            TempData["SuccessMessage"] = response.Mensaje;

            // Redirigir al Login enviando solo el correo
            TempData["PreFillCorreo"] = vm.Correo;

            return RedirectToAction("Login");
        }

        #endregion

        #region Metodo RecuperarContrasenna
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(RecoverPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var response = await _recoverPasswordUseCase.ExecuteAsync(vm.Correo);

            if (!response.Exito)
            {
                ModelState.AddModelError("", response.Mensaje);
                return View(vm);
            }

            TempData["SuccessMessage"] = response.Mensaje;

            return RedirectToAction("Login");
        }
        #endregion

        #region Metodo CerrarSesion
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        #endregion
    }
}
