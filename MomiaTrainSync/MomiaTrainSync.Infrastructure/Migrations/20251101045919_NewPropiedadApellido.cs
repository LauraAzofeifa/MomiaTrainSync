using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MomiaTrainSync.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewPropiedadApellido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Apellido",
                table: "Usuario",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apellido",
                table: "Usuario");
        }
    }
}
