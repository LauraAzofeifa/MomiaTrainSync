using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MomiaTrainSync.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EntidadUsuarioModificada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FechaCumpleannos",
                table: "Usuarios",
                newName: "FechaUltimoLogin");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaNacimiento",
                table: "Usuarios",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaUltimoCambioContrasenna",
                table: "Usuarios",
                type: "datetime",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaNacimiento",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "FechaUltimoCambioContrasenna",
                table: "Usuarios");

            migrationBuilder.RenameColumn(
                name: "FechaUltimoLogin",
                table: "Usuarios",
                newName: "FechaCumpleannos");
        }
    }
}
