using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MomiaTrainSync.Migrations
{
    /// <inheritdoc />
    public partial class AddFechaRegistroToUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRegistro",
                table: "Usuarios",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaRegistro",
                value: new DateTime(2025, 9, 28, 16, 50, 29, 501, DateTimeKind.Local).AddTicks(6140));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaRegistro",
                value: new DateTime(2025, 9, 28, 16, 50, 29, 503, DateTimeKind.Local).AddTicks(9893));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaRegistro",
                value: new DateTime(2025, 9, 28, 16, 50, 29, 503, DateTimeKind.Local).AddTicks(9917));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaRegistro",
                value: new DateTime(2025, 9, 28, 16, 50, 29, 503, DateTimeKind.Local).AddTicks(9920));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaRegistro",
                value: new DateTime(2025, 9, 28, 16, 50, 29, 503, DateTimeKind.Local).AddTicks(9923));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "Usuarios");
        }
    }
}
