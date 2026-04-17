using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using System.Security.Claims;

namespace MomiaTrainSync.Web.Security
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class PermisoAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userIdStr = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdStr))
            {
                context.Result = new ForbidResult();
                return;
            }

            var userId = int.Parse(userIdStr);
            var permisoRepo = context.HttpContext.RequestServices.GetRequiredService<IPermisoRepository>();

            var rutaActual = context.HttpContext.Request.Path.Value ?? "";

            // 1. Verificar si la ruta está registrada como permiso
            var permiso = await permisoRepo.GetByRutaAsync(rutaActual);

            // 2. Si la ruta NO está en la BD → permitir acceso
            if (permiso == null)
            {
                await next();
                return;
            }

            // 3. Si la ruta SÍ existe → verificar si el usuario tiene permiso
            var tieneAcceso = await permisoRepo.HasPermissionAsync(userId, rutaActual);

            if (!tieneAcceso)
            {
                context.Result = new ForbidResult();
                return;
            }

            await next();
        }
    }
}
