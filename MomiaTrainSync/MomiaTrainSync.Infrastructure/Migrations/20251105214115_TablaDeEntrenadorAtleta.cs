using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MomiaTrainSync.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TablaDeEntrenadorAtleta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntrenadorAtleta",
                columns: table => new
                {
                    IdEntrenador = table.Column<int>(type: "int", nullable: false),
                    IdAtleta = table.Column<int>(type: "int", nullable: false),
                    FechaAsignacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntrenadorAtleta", x => new { x.IdEntrenador, x.IdAtleta });
                    table.ForeignKey(
                        name: "FK_EntrenadorAtleta_Usuario_IdAtleta",
                        column: x => x.IdAtleta,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntrenadorAtleta_Usuario_IdEntrenador",
                        column: x => x.IdEntrenador,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntrenadorAtleta_IdAtleta",
                table: "EntrenadorAtleta",
                column: "IdAtleta");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntrenadorAtleta");
        }
    }
}
