using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MomiaTrainSync.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewItemEnPermisos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Categoria",
                table: "Permiso",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -5,
                column: "Categoria",
                value: "Profile");

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -4,
                column: "Categoria",
                value: "Profile");

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "IdPermiso",
                keyValue: -3,
                column: "Categoria",
                value: "Profile");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "Permiso");
        }
    }
}
