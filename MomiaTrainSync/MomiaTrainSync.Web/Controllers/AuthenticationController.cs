using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MomiaTrainSync.Core.DTOs;
using MomiaTrainSync.Core.UseCases;
using MomiaTrainSync.Web.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MomiaTrainSync.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly LoginUseCase _loginUseCase;
        private readonly RegisterUseCase _registerUseCase;
        
        public AuthenticationController(LoginUseCase loginUseCase, RegisterUseCase registerUseCase)
        {
            _loginUseCase = loginUseCase;
            _registerUseCase = registerUseCase;
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

            var usuario = await _loginUseCase.ExecuteAsync(vm.Correo, vm.Contrasenna);

            if (usuario == null)
            {
                ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                return View();
            }

            var claims = GetClaims(usuario);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Mantener la sesión iniciada
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(1) // Expira en x
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
                new Claim(ClaimTypes.Role, dto.Rol!),
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

            var usuario = await _registerUseCase.ExecuteAsync(vm.Nombre, vm.Correo, vm.Contrasenna);

            if (usuario == null)
            {
                ModelState.AddModelError("", "Error al crear el usuario. Intente nuevamente.");
                return View(vm);
            }

            // Guardar un mensaje temporal que se muestra en el login
            TempData["SuccessMessage"] = "Usuario registrado con éxito. Ahora puede iniciar sesión.";

            // Redirigir al Login enviando solo el correo
            TempData["PreFillCorreo"] = vm.Correo;
            return RedirectToAction("Login");
        }

        #endregion
    }
}
