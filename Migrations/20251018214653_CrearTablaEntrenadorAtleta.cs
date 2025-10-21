using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MomiaTrainSync.Migrations
{
    /// <inheritdoc />
    public partial class CrearTablaEntrenadorAtleta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntrenadoresAtletas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntrenadorId = table.Column<int>(type: "int", nullable: false),
                    AtletaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntrenadoresAtletas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntrenadoresAtletas_Usuarios_AtletaId",
                        column: x => x.AtletaId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntrenadoresAtletas_Usuarios_EntrenadorId",
                        column: x => x.EntrenadorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntrenadoresAtletas_AtletaId",
                table: "EntrenadoresAtletas",
                column: "AtletaId");

            migrationBuilder.CreateIndex(
                name: "IX_EntrenadoresAtletas_EntrenadorId_AtletaId",
                table: "EntrenadoresAtletas",
                columns: new[] { "EntrenadorId", "AtletaId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntrenadoresAtletas");
        }
    }
}
