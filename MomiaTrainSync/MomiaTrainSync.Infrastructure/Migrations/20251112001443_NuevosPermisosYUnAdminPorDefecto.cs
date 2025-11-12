using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MomiaTrainSync.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NuevosPermisosYUnAdminPorDefecto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -1,
                column: "Ruta",
                value: "/Users/ManageUsers");

            migrationBuilder.InsertData(
                table: "Permiso",
                columns: new[] { "IdPermiso", "Categoria", "Codigo", "Descripcion", "Estado", "Ruta" },
                values: new object[,]
                {
                    { -8, "Security", "VER_PERMISOS", "Permite ver la lista de permisos", true, "/Permissions/Index" },
                    { -7, "Usuarios", "ELIMINAR_ATLETA_ENTRENADOR", "Permite al entrenador eliminar atletas de su lista", true, "/Trainer/DeleteAthlete" },
                    { -6, "Usuarios", "AGREGAR_ATLETA_ENTRENADOR", "Permite al entrenador agregar atletas a su lista asignada", true, "/Trainer/AddAthlete" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Apellido", "ContrasennaHash", "Correo", "Estado", "FechaCreacion", "FechaCumpleannos", "Nombre", "RolId", "Telefono" },
                values: new object[] { -1, "Sistema", "UDd9Jxr59YTGLmp8Dofxlw==.Bf/QH105NwCI9Dt8C+fkRpjRXwOlPSEOjVZKMgqK0pI=", "admin@dominio.com", true, new DateTime(2025, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", -1, "60000000" });

            migrationBuilder.InsertData(
                table: "RolPermiso",
                columns: new[] { "IdPermiso", "IdRol" },
                values: new object[,]
                {
                    { -7, -2 },
                    { -6, -2 },
                    { -8, -1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumns: new[] { "IdPermiso", "IdRol" },
                keyValues: new object[] { -7, -2 });

            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumns: new[] { "IdPermiso", "IdRol" },
                keyValues: new object[] { -6, -2 });

            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumns: new[] { "IdPermiso", "IdRol" },
                keyValues: new object[] { -8, -1 });

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -8);

            migrationBuilder.DeleteData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -7);

            migrationBuilder.DeleteData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -6);

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -1,
                column: "Ruta",
                value: "/Admin/ManageUsers");
        }
    }
}
