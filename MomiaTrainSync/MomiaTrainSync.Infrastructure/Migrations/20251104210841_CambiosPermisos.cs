using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MomiaTrainSync.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CambiosPermisos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Permiso",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ruta",
                table: "Permiso",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -5,
                columns: new[] { "Codigo", "Descripcion", "Ruta" },
                values: new object[] { "CAMBIAR_CONTRASENNA_PERFIL", "Permite cambiar la contraseña del usuario", "/Profile/ChangePassword" });

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -4,
                columns: new[] { "Codigo", "Descripcion", "Ruta" },
                values: new object[] { "EDITAR_PERFIL", "Permite editar el perfil del usuario", "/Profile/EditProfile" });

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -3,
                columns: new[] { "Codigo", "Descripcion", "Ruta" },
                values: new object[] { "VER_PERFIL", "Permite ver el perfil del usuario", "/Profile/MyProfile" });

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -2,
                columns: new[] { "Codigo", "Descripcion", "Ruta" },
                values: new object[] { "GESTIONAR_ATLETAS", "Permite gestionar atletas", "/Trainer/ManageAthletes" });

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -1,
                columns: new[] { "Descripcion", "Ruta" },
                values: new object[] { "Permite administrar usuarios del sistema", "/Admin/ManageUsers" });

            migrationBuilder.InsertData(
                table: "RolPermiso",
                columns: new[] { "IdPermiso", "IdRol" },
                values: new object[] { -3, -3 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumns: new[] { "IdPermiso", "IdRol" },
                keyValues: new object[] { -3, -3 });

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Permiso");

            migrationBuilder.DropColumn(
                name: "Ruta",
                table: "Permiso");

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -5,
                column: "Codigo",
                value: "GESTIONAR_RUTINAS");

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -4,
                column: "Codigo",
                value: "GESTIONAR_PERFILES");

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -3,
                column: "Codigo",
                value: "GESTIONAR_REPORTES");

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -2,
                column: "Codigo",
                value: "GESTIONAR_ENTRENAMIENTOS");
        }
    }
}
