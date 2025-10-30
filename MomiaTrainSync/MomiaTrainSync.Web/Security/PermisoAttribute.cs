using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MomiaTrainSync.Core.Interfaces.Repositories;
using System.Security.Claims;

namespace MomiaTrainSync.Web.Security
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class PermisoAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string _codigoPermiso;

        public PermisoAttribute(string codigoPermiso)
        {
            _codigoPermiso = codigoPermiso;
        }

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

            var tienePermiso = await permisoRepo.HasPermissionAsync(userId, _codigoPermiso);

            if (!tienePermiso)
            {
                context.Result = new ForbidResult();
                return;
            }

            await next();
        }
    }
}
