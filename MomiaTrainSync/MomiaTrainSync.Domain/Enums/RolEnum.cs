using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Domain.Enums
{
    public enum RolEnum
    {
        Administrador = -1,
        Entrenador = -2,
        Atleta = -3
    }

    public enum PermisoEnum
    {
        GestionarUsuarios = -1,
        GestionarEntrenamientos = -2,
        VerReportes = -3,
        EditarPerfil = -4,
        VerRutina = -5
    }

}
