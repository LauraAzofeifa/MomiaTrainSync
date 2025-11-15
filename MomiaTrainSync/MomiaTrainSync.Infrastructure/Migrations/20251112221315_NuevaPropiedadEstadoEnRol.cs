using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MomiaTrainSync.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NuevaPropiedadEstadoEnRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "Rol",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "IdRol",
                keyValue: -3,
                column: "Estado",
                value: true);

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "IdRol",
                keyValue: -2,
                column: "Estado",
                value: true);

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "IdRol",
                keyValue: -1,
                column: "Estado",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Rol");
        }
    }
}
