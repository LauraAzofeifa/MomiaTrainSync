using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MomiaTrainSync.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Contrasena", "Correo", "Nombre", "Rol" },
                values: new object[,]
                {
                    { 1, "123456", "laura@ejemplo.com", "Laura Azofeifa", "Administrador" },
                    { 2, "123456", "carlos@ejemplo.com", "Carlos Ramírez", "Entrenador" },
                    { 3, "123456", "ana@ejemplo.com", "Ana Rojas", "Entrenador" },
                    { 4, "123456", "luis@ejemplo.com", "Luis Jiménez", "Atleta" },
                    { 5, "123456", "maria@ejemplo.com", "María Pérez", "Atleta" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
