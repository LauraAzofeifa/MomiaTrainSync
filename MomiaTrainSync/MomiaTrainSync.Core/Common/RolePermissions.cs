using MomiaTrainSync.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.Common
{
    public static class RolePermissions
    {
        public static readonly Dictionary<SeguridadEnums.Rol, SeguridadEnums.Permiso[]> RolAPermisos = new()
        {
            { SeguridadEnums.Rol.Administrador, new[]
                {
                    SeguridadEnums.Permiso.GestionarUsuarios,
                    SeguridadEnums.Permiso.GestionarEntrenamientos,
                    SeguridadEnums.Permiso.VerReportes,
                    SeguridadEnums.Permiso.EditarPerfil,
                    SeguridadEnums.Permiso.VerRutina
                }
            },
            { SeguridadEnums.Rol.Entrenador, new[]
                {
                    SeguridadEnums.Permiso.GestionarEntrenamientos,
                    SeguridadEnums.Permiso.VerReportes,
                    SeguridadEnums.Permiso.VerRutina
                }
            },
            { SeguridadEnums.Rol.Atleta, new[]
                {
                    SeguridadEnums.Permiso.VerRutina,
                    SeguridadEnums.Permiso.EditarPerfil
                }
            }
        };
    }
}
