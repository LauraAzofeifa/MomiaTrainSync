using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MomiaTrainSync.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeQuitoElHashDataEnPermisosYRolesYUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumns: new[] { "IdPermiso", "IdRol" },
                keyValues: new object[] { -5, -3 });

            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumns: new[] { "IdPermiso", "IdRol" },
                keyValues: new object[] { -4, -3 });

            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumns: new[] { "IdPermiso", "IdRol" },
                keyValues: new object[] { -3, -3 });

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
                keyValues: new object[] { -5, -2 });

            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumns: new[] { "IdPermiso", "IdRol" },
                keyValues: new object[] { -4, -2 });

            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumns: new[] { "IdPermiso", "IdRol" },
                keyValues: new object[] { -3, -2 });

            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumns: new[] { "IdPermiso", "IdRol" },
                keyValues: new object[] { -2, -2 });

            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumns: new[] { "IdPermiso", "IdRol" },
                keyValues: new object[] { -8, -1 });

            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumns: new[] { "IdPermiso", "IdRol" },
                keyValues: new object[] { -5, -1 });

            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumns: new[] { "IdPermiso", "IdRol" },
                keyValues: new object[] { -4, -1 });

            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumns: new[] { "IdPermiso", "IdRol" },
                keyValues: new object[] { -3, -1 });

            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumns: new[] { "IdPermiso", "IdRol" },
                keyValues: new object[] { -2, -1 });

            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumns: new[] { "IdPermiso", "IdRol" },
                keyValues: new object[] { -1, -1 });

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

            migrationBuilder.DeleteData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -5);

            migrationBuilder.DeleteData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "Rol",
                keyColumn: "IdRol",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "Rol",
                keyColumn: "IdRol",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "Rol",
                keyColumn: "IdRol",
                keyValue: -1);

            migrationBuilder.CreateIndex(
                name: "IX_Rol_Nombre",
                table: "Rol",
                column: "Nombre",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rol_Nombre",
                table: "Rol");

            migrationBuilder.InsertData(
                table: "Permiso",
                columns: new[] { "IdPermiso", "Categoria", "Codigo", "Descripcion", "Estado", "Ruta" },
                values: new object[,]
                {
                    { -8, "Security", "VER_PERMISOS", "Permite ver la lista de permisos", true, "/Permissions/Index" },
                    { -7, "Usuarios", "ELIMINAR_ATLETA_ENTRENADOR", "Permite al entrenador eliminar atletas de su lista", true, "/Trainer/DeleteAthlete" },
                    { -6, "Usuarios", "AGREGAR_ATLETA_ENTRENADOR", "Permite al entrenador agregar atletas a su lista asignada", true, "/Trainer/AddAthlete" },
                    { -5, "Profile", "CAMBIAR_CONTRASENNA_PERFIL", "Permite cambiar la contraseña del usuario", true, "/Profile/ChangePassword" },
                    { -4, "Profile", "EDITAR_PERFIL", "Permite editar el perfil del usuario", true, "/Profile/EditProfile" },
                    { -3, "Profile", "VER_PERFIL", "Permite ver el perfil del usuario", true, "/Profile/MyProfile" },
                    { -2, "Athletes", "GESTIONAR_ATLETAS", "Permite gestionar atletas", true, "/Trainer/ManageAthletes" },
                    { -1, "Users", "GESTIONAR_USUARIOS", "Permite administrar usuarios del sistema", true, "/Users/ManageUsers" }
                });

            migrationBuilder.InsertData(
                table: "Rol",
                columns: new[] { "IdRol", "Descripcion", "Estado", "Nombre" },
                values: new object[,]
                {
                    { -3, "Usuario que recibe rutinas asignadas", true, "Atleta" },
                    { -2, "Gestiona rutinas, entrenamientos y atletas", true, "Entrenador" },
                    { -1, "Acceso completo al sistema", true, "Administrador" }
                });

            migrationBuilder.InsertData(
                table: "RolPermiso",
                columns: new[] { "IdPermiso", "IdRol" },
                values: new object[,]
                {
                    { -5, -3 },
                    { -4, -3 },
                    { -3, -3 },
                    { -7, -2 },
                    { -6, -2 },
                    { -5, -2 },
                    { -4, -2 },
                    { -3, -2 },
                    { -2, -2 },
                    { -8, -1 },
                    { -5, -1 },
                    { -4, -1 },
                    { -3, -1 },
                    { -2, -1 },
                    { -1, -1 }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Apellido", "ContrasennaHash", "Correo", "Estado", "FechaCreacion", "FechaCumpleannos", "Nombre", "RolId", "Telefono" },
                values: new object[] { -1, "Sistema", "UDd9Jxr59YTGLmp8Dofxlw==.Bf/QH105NwCI9Dt8C+fkRpjRXwOlPSEOjVZKMgqK0pI=", "admin@dominio.com", true, new DateTime(2025, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", -1, "60000000" });
        }
    }
}
