using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MomiaTrainSync.Migrations
{
    /// <inheritdoc />
    public partial class CrearPlanEntrenamiento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlanesEntrenamiento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAtleta = table.Column<int>(type: "int", nullable: false),
                    IdCreador = table.Column<int>(type: "int", nullable: false),
                    Detalle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanesEntrenamiento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanesEntrenamiento_Usuarios_IdAtleta",
                        column: x => x.IdAtleta,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanesEntrenamiento_Usuarios_IdCreador",
                        column: x => x.IdCreador,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanesEntrenamiento_IdAtleta",
                table: "PlanesEntrenamiento",
                column: "IdAtleta");

            migrationBuilder.CreateIndex(
                name: "IX_PlanesEntrenamiento_IdCreador",
                table: "PlanesEntrenamiento",
                column: "IdCreador");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanesEntrenamiento");
        }
    }
}
