using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MomiaTrainSync.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CambioEnCategoriaPermisos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -2,
                column: "Categoria",
                value: "Athletes");

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -1,
                column: "Categoria",
                value: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -2,
                column: "Categoria",
                value: "Trainer");

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -1,
                column: "Categoria",
                value: "Admin");
        }
    }
}
