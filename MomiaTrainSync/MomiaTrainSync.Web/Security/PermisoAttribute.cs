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
