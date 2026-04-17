using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MomiaTrainSync.Domain.Entities.EntrenamientosZonas;
using MomiaTrainSync.Domain.Entities.RutinasEntrenamientos;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MomiaTrainSync.Infrastructure.Persistence
{
    public static class SeedData
    {
        private static readonly List<RolEnt> DefaultRoles = new()
        {
            new RolEnt
            {
                Nombre = "Administrador",
                Descripcion = "Acceso completo al sistema",
                Estado = true
            },
            new RolEnt
            {
                Nombre = "Entrenador",
                Descripcion = "Gestiona rutinas, entrenamientos y atletas",
                Estado = true
            },
            new RolEnt
            {
                Nombre = "Atleta",
                Descripcion = "Usuario que recibe rutinas asignadas",
                Estado = true
            }
        };

        private static readonly List<PermisoEnt> DefaultPermissions = new()
        {
            new PermisoEnt
            {
                Codigo = "GESTIONAR_USUARIOS",
                Descripcion = "Permite administrar usuarios del sistema",
                Categoria = "Users",
                Ruta = "/Users/ManageUsers",
                Estado = true
            },
            new PermisoEnt
            {
                Codigo = "GESTIONAR_ATLETAS",
                Descripcion = "Permite gestionar atletas",
                Categoria = "Athletes",
                Ruta = "/Trainer/ManageAthletes",
                Estado = true
            },
            new PermisoEnt
            {
                Codigo = "VER_PERFIL",
                Descripcion = "Permite ver el perfil del usuario",
                Categoria = "Profile",
                Ruta = "/Profile/MyProfile",
                Estado = true
            },
            new PermisoEnt
            {
                Codigo = "EDITAR_PERFIL",
                Descripcion = "Permite editar el perfil del usuario",
                Categoria = "Profile",
                Ruta = "/Profile/EditProfile",
                Estado = true
            },
            new PermisoEnt
            {
                Codigo = "CAMBIAR_CONTRASENNA_PERFIL",
                Descripcion = "Permite cambiar la contraseña del usuario",
                Categoria = "Profile",
                Ruta = "/Profile/ChangePassword",
                Estado = true
            },
            new PermisoEnt
            {
                Codigo = "AGREGAR_ATLETA_ENTRENADOR",
                Descripcion = "Permite al entrenador agregar atletas a su lista asignada",
                Categoria = "Usuarios",
                Ruta = "/Trainer/AddAthlete",
                Estado = true
            },
            new PermisoEnt
            {
                Codigo = "ELIMINAR_ATLETA_ENTRENADOR",
                Descripcion = "Permite al entrenador eliminar atletas de su lista",
                Categoria = "Usuarios",
                Ruta = "/Trainer/DeleteAthlete",
                Estado = true
            },
            new PermisoEnt
            {
                Codigo = "VER_PERMISOS",
                Descripcion = "Permite ver la lista de permisos",
                Categoria = "Security",
                Ruta = "/Permissions/Index",
                Estado = true
            }
        };

        private static readonly List<ZonaEntrenamientoEnt> DefaultZonas = new()
        {
            new ZonaEntrenamientoEnt
            {
                Nombre = "Z1 - Recuperación",
                Descripcion = "Muy suave, ideal para regeneración",
                Factor = 0.60m
            },
            new ZonaEntrenamientoEnt
            {
                Nombre = "Z2 - Aeróbica",
                Descripcion = "Ritmo suave controlado, base aeróbica",
                Factor = 0.70m
            },
            new ZonaEntrenamientoEnt
            {
                Nombre = "Z3 - Tempo",
                Descripcion = "Ritmo moderado sostenido",
                Factor = 0.80m
            },
            new ZonaEntrenamientoEnt
            {
                Nombre = "Z4 - Umbral",
                Descripcion = "Ritmo fuerte, cercano al umbral",
                Factor = 0.90m
            },
            new ZonaEntrenamientoEnt
            {
                Nombre = "Z5 - VO2Max",
                Descripcion = "Intervalos duros de alta intensidad",
                Factor = 1.00m
            },
            new ZonaEntrenamientoEnt
            {
                Nombre = "Z6 - Anaeróbico",
                Descripcion = "Esfuerzos máximos muy cortos",
                Factor = 1.10m
            }
        };

        private static readonly List<TipoSesionEnt> DefaultTipoSesiones = new()
        {
            new TipoSesionEnt
            {
                Nombre = "Swim",
                Estado = true
            },
            new TipoSesionEnt
            {
                Nombre = "Run",
                Estado = true
            },
            new TipoSesionEnt
            {
                Nombre = "Bike",
                Estado = true
            },
            new TipoSesionEnt
            {
                Nombre = "Walk",
                Estado = true
            },
            new TipoSesionEnt
            {
                Nombre = "Strength",
                Estado = true
            }
        };

        // ===============================
        // 🔹 SEED ROLES
        // ===============================

        public static async Task EnsureRolesAsync(IServiceProvider service)
        {
            using var scope = service.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<MomiaTrainSyncDbContext>();

            if (!await db.Roles.AnyAsync())
            {
                await db.Roles.AddRangeAsync(DefaultRoles);

                await db.SaveChangesAsync();

                Console.WriteLine("Roles iniciales insertados");
            }
        }

        // ===============================
        // 🔹 SEED PERMISOS
        // ===============================

        public static async Task EnsurePermissionsAsync(IServiceProvider service)
        {
            using var scope = service.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<MomiaTrainSyncDbContext>();

            foreach (var permiso in DefaultPermissions)
            {
                var exists = await db.Permisos
                    .AnyAsync(p => p.Codigo == permiso.Codigo);

                if (!exists)
                {
                    db.Permisos.Add(permiso);
                }
            }

            await db.SaveChangesAsync();

            Console.WriteLine("Permisos verificados");
        }

        // ===============================
        // 🔹 SEED ROLPERMISOS
        // ===============================

        public static async Task EnsureRolePermissionsAsync(IServiceProvider service)
        {
            using var scope = service.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<MomiaTrainSyncDbContext>();

            // Obtener roles
            var adminRole = await db.Roles
                .FirstOrDefaultAsync(r => r.Nombre == "Administrador");

            var trainerRole = await db.Roles
                .FirstOrDefaultAsync(r => r.Nombre == "Entrenador");

            var athleteRole = await db.Roles
                .FirstOrDefaultAsync(r => r.Nombre == "Atleta");

            if (adminRole == null ||
                trainerRole == null ||
                athleteRole == null)
            {
                Console.WriteLine("Roles no encontrados");
                return;
            }

            // Obtener permisos
            var allPermissions = await db.Permisos.ToListAsync();

            var trainerPermissionCodes = new[]
            {
                "GESTIONAR_ATLETAS",
                "VER_PERFIL",
                "EDITAR_PERFIL",
                "CAMBIAR_CONTRASENNA_PERFIL",
                "AGREGAR_ATLETA_ENTRENADOR",
                "ELIMINAR_ATLETA_ENTRENADOR"
            };

            var athletePermissionCodes = new[]
            {
                "VER_PERFIL",
                "EDITAR_PERFIL",
                "CAMBIAR_CONTRASENNA_PERFIL"
            };

            var trainerPermissions = allPermissions
                .Where(p => trainerPermissionCodes.Contains(p.Codigo))
                .ToList();

            var athletePermissions = allPermissions
                .Where(p => athletePermissionCodes.Contains(p.Codigo))
                .ToList();

            // =========================================
            // 🔹 ADMIN → TODOS LOS PERMISOS
            // =========================================

            foreach (var permiso in allPermissions)
            {
                var exists = await db.RolesPermisos.AnyAsync(rp =>
                    rp.IdRol == adminRole.IdRol &&
                    rp.IdPermiso == permiso.IdPermiso);

                if (!exists)
                {
                    db.RolesPermisos.Add(new RolPermisoEnt
                    {
                        IdRol = adminRole.IdRol,
                        IdPermiso = permiso.IdPermiso
                    });
                }
            }

            // =========================================
            // 🔹 ENTRENADOR → PERMISOS ESPECÍFICOS
            // =========================================

            foreach (var permiso in trainerPermissions)
            {
                var exists = await db.RolesPermisos.AnyAsync(rp =>
                    rp.IdRol == trainerRole.IdRol &&
                    rp.IdPermiso == permiso.IdPermiso);

                if (!exists)
                {
                    db.RolesPermisos.Add(new RolPermisoEnt
                    {
                        IdRol = trainerRole.IdRol,
                        IdPermiso = permiso.IdPermiso
                    });
                }
            }

            // =========================================
            // 🔹 ATLETA → PERFIL
            // =========================================

            foreach (var permiso in athletePermissions)
            {
                var exists = await db.RolesPermisos.AnyAsync(rp =>
                    rp.IdRol == athleteRole.IdRol &&
                    rp.IdPermiso == permiso.IdPermiso);

                if (!exists)
                {
                    db.RolesPermisos.Add(new RolPermisoEnt
                    {
                        IdRol = athleteRole.IdRol,
                        IdPermiso = permiso.IdPermiso
                    });
                }
            }

            await db.SaveChangesAsync();

            Console.WriteLine("RolPermiso relaciones verificadas");
        }

        // ===============================
        // 🔹 SEED USUARIOS
        // ===============================

        public static async Task EnsureUsuariosAsync(IServiceProvider service)
        {
            using var scope = service.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<MomiaTrainSyncDbContext>();

            // Buscar rol Administrador
            var adminRole = await db.Roles
                .FirstOrDefaultAsync(r => r.Nombre == "Administrador");

            if (adminRole == null)
            {
                Console.WriteLine("Rol Administrador no encontrado");
                return;
            }

            // Verificar si David ya existe
            if (!await db.Usuarios
                .AnyAsync(u => u.Correo == "d1234avid@gmail.com"))
            {
                var davidAdmin = new UsuarioEnt
                {
                    Nombre = "David",
                    Apellido = "Rojas",
                    Correo = "d1234avid@gmail.com",
                    Telefono = "123456789",
                    FechaNacimiento = new DateTime(1994, 7, 19),
                    Biografia = "Administrador del sistema",

                    ContrasennaHash =
                        "UDd9Jxr59YTGLmp8Dofxlw==.Bf/QH105NwCI9Dt8C+fkRpjRXwOlPSEOjVZKMgqK0pI=",

                    Estado = true,
                    FechaCreacion = DateTime.UtcNow,

                    RolId = adminRole.IdRol
                };

                db.Usuarios.Add(davidAdmin);

                await db.SaveChangesAsync();

                Console.WriteLine("Usuario David (Admin) creado");
            }
        }

        // ===============================
        // 🔹 SEED ZONAS ENTRENAMIENTO
        // ===============================

        public static async Task EnsureZonasAsync(IServiceProvider service)
        {
            using var scope = service.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<MomiaTrainSyncDbContext>();

            foreach (var zona in DefaultZonas)
            {
                var exists = await db.ZonasEntrenamiento
                    .AnyAsync(z => z.Nombre == zona.Nombre);

                if (!exists)
                {
                    db.ZonasEntrenamiento.Add(zona);
                }
            }

            await db.SaveChangesAsync();

            Console.WriteLine("Zonas de entrenamiento verificadas");
        }

        // ===============================
        // 🔹 SEED TIPO SESION
        // ===============================

        public static async Task EnsureTipoSesionAsync(IServiceProvider service)
        {
            using var scope = service.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<MomiaTrainSyncDbContext>();

            foreach (var tipo in DefaultTipoSesiones)
            {
                var exists = await db.TiposSesion
                    .AnyAsync(t => t.Nombre == tipo.Nombre);

                if (!exists)
                {
                    db.TiposSesion.Add(tipo);
                }
            }

            await db.SaveChangesAsync();

            Console.WriteLine("Tipos de sesión verificados");
        }
    }
}