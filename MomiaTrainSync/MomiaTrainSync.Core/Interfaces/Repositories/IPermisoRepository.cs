using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.Interfaces.Repositories
{
    public interface IPermisoRepository
    {
        Task<bool> HasPermissionAsync(int userId, string permissionName);
    }
}
