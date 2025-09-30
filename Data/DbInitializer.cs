using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Data;
using MomiaTrainSync.Models;

public static class DbInitializer
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>().HasData(
            new Usuario
            {
                Id = 1,
                Nombre = "Laura Azofeifa",
                Correo = "laura@ejemplo.com",
                Contrasena = "123456",
                Rol = "Administrador",
                Estado = "Activo",
                FechaRegistro = new DateTime(2025, 09, 28)
            },
            new Usuario
            {
                Id = 2,
                Nombre = "Carlos Ramírez",
                Correo = "carlos@ejemplo.com",
                Contrasena = "123456",
                Rol = "Entrenador",
                Estado = "Activo",
                FechaRegistro = new DateTime(2025, 09, 28)
            },
            new Usuario
            {
                Id = 3,
                Nombre = "Ana Rojas",
                Correo = "ana@ejemplo.com",
                Contrasena = "123456",
                Rol = "Entrenador",
                Estado = "Activo",
                FechaRegistro = new DateTime(2025, 09, 28)
            },
            new Usuario
            {
                Id = 4,
                Nombre = "Luis Jiménez",
                Correo = "luis@ejemplo.com",
                Contrasena = "123456",
                Rol = "Atleta",
                Estado = "Activo",
                FechaRegistro = new DateTime(2025, 09, 28)
            },
            new Usuario
            {
                Id = 5,
                Nombre = "María Pérez",
                Correo = "maria@ejemplo.com",
                Contrasena = "123456",
                Rol = "Atleta",
                Estado = "Activo",
                FechaRegistro = new DateTime(2025, 09, 28)
            }
        );
    }
}