using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MomiaTrainSync.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeAgregaronCasiTodasLasEnt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntrenadorAtleta_Usuario_IdAtleta",
                table: "EntrenadorAtleta");

            migrationBuilder.DropForeignKey(
                name: "FK_EntrenadorAtleta_Usuario_IdEntrenador",
                table: "EntrenadorAtleta");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EntrenadorAtleta",
                table: "EntrenadorAtleta");

            migrationBuilder.AlterColumn<bool>(
                name: "Estado",
                table: "Permiso",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaAsignacion",
                table: "EntrenadorAtleta",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<int>(
                name: "IdRelacion",
                table: "EntrenadorAtleta",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "EntrenadorAtleta",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EntrenadorAtleta",
                table: "EntrenadorAtleta",
                column: "IdRelacion");

            migrationBuilder.CreateTable(
                name: "Rutina",
                columns: table => new
                {
                    IdRutina = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rutina", x => x.IdRutina);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FechaCumpleannos = table.Column<DateTime>(type: "datetime", nullable: true),
                    ContrasennaHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime", nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Rol_RolId",
                        column: x => x.RolId,
                        principalTable: "Rol",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ZonaEntrenamiento",
                columns: table => new
                {
                    IdZona = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Factor = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZonaEntrenamiento", x => x.IdZona);
                });

            migrationBuilder.CreateTable(
                name: "Entrenamiento",
                columns: table => new
                {
                    IdEntrenamiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEntrenador = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TipoSesion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Objetivo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DuracionEstimada = table.Column<int>(type: "int", nullable: false),
                    NivelEsfuerzoEsperado = table.Column<byte>(type: "tinyint", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioEntId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrenamiento", x => x.IdEntrenamiento);
                    table.ForeignKey(
                        name: "FK_Entrenamiento_Usuarios_IdEntrenador",
                        column: x => x.IdEntrenador,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entrenamiento_Usuarios_UsuarioEntId",
                        column: x => x.UsuarioEntId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AsignacionRutina",
                columns: table => new
                {
                    IdAsignacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdRutina = table.Column<int>(type: "int", nullable: false),
                    IdEntrenamiento = table.Column<int>(type: "int", nullable: false),
                    IdRelacion = table.Column<int>(type: "int", nullable: false),
                    FechaProgramada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NotaEntrenador = table.Column<string>(type: "text", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsignacionRutina", x => x.IdAsignacion);
                    table.ForeignKey(
                        name: "FK_AsignacionRutina_EntrenadorAtleta_IdRelacion",
                        column: x => x.IdRelacion,
                        principalTable: "EntrenadorAtleta",
                        principalColumn: "IdRelacion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AsignacionRutina_Entrenamiento_IdEntrenamiento",
                        column: x => x.IdEntrenamiento,
                        principalTable: "Entrenamiento",
                        principalColumn: "IdEntrenamiento",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AsignacionRutina_Rutina_IdRutina",
                        column: x => x.IdRutina,
                        principalTable: "Rutina",
                        principalColumn: "IdRutina",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetalleZonaPlan",
                columns: table => new
                {
                    IdDetalleZonaPlan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEntrenamiento = table.Column<int>(type: "int", nullable: false),
                    IdZona = table.Column<int>(type: "int", nullable: false),
                    MinutosPlanificados = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleZonaPlan", x => x.IdDetalleZonaPlan);
                    table.ForeignKey(
                        name: "FK_DetalleZonaPlan_Entrenamiento_IdEntrenamiento",
                        column: x => x.IdEntrenamiento,
                        principalTable: "Entrenamiento",
                        principalColumn: "IdEntrenamiento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleZonaPlan_ZonaEntrenamiento_IdZona",
                        column: x => x.IdZona,
                        principalTable: "ZonaEntrenamiento",
                        principalColumn: "IdZona",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SesionEntrenamiento",
                columns: table => new
                {
                    IdSesion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAsignacion = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DuracionReal = table.Column<int>(type: "int", nullable: false),
                    NivelEsfuerzoPercibido = table.Column<byte>(type: "tinyint", nullable: false),
                    Comentarios = table.Column<string>(type: "text", nullable: true),
                    CargaTotal = table.Column<decimal>(type: "decimal(6,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SesionEntrenamiento", x => x.IdSesion);
                    table.ForeignKey(
                        name: "FK_SesionEntrenamiento_AsignacionRutina_IdAsignacion",
                        column: x => x.IdAsignacion,
                        principalTable: "AsignacionRutina",
                        principalColumn: "IdAsignacion",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetalleZonaSesion",
                columns: table => new
                {
                    IdDetalleZonaSesion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSesion = table.Column<int>(type: "int", nullable: false),
                    IdZona = table.Column<int>(type: "int", nullable: false),
                    MinutosCompletados = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleZonaSesion", x => x.IdDetalleZonaSesion);
                    table.ForeignKey(
                        name: "FK_DetalleZonaSesion_SesionEntrenamiento_IdSesion",
                        column: x => x.IdSesion,
                        principalTable: "SesionEntrenamiento",
                        principalColumn: "IdSesion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleZonaSesion_ZonaEntrenamiento_IdZona",
                        column: x => x.IdZona,
                        principalTable: "ZonaEntrenamiento",
                        principalColumn: "IdZona",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "IdRol",
                keyValue: -3,
                column: "Descripcion",
                value: "Usuario que recibe rutinas asignadas");

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "IdRol",
                keyValue: -2,
                column: "Descripcion",
                value: "Gestiona rutinas, entrenamientos y atletas");

            migrationBuilder.CreateIndex(
                name: "IX_EntrenadorAtleta_IdEntrenador",
                table: "EntrenadorAtleta",
                column: "IdEntrenador");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionRutina_IdEntrenamiento",
                table: "AsignacionRutina",
                column: "IdEntrenamiento");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionRutina_IdRelacion",
                table: "AsignacionRutina",
                column: "IdRelacion");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionRutina_IdRutina",
                table: "AsignacionRutina",
                column: "IdRutina");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleZonaPlan_IdEntrenamiento",
                table: "DetalleZonaPlan",
                column: "IdEntrenamiento");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleZonaPlan_IdZona",
                table: "DetalleZonaPlan",
                column: "IdZona");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleZonaSesion_IdSesion",
                table: "DetalleZonaSesion",
                column: "IdSesion");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleZonaSesion_IdZona",
                table: "DetalleZonaSesion",
                column: "IdZona");

            migrationBuilder.CreateIndex(
                name: "IX_Entrenamiento_IdEntrenador",
                table: "Entrenamiento",
                column: "IdEntrenador");

            migrationBuilder.CreateIndex(
                name: "IX_Entrenamiento_UsuarioEntId",
                table: "Entrenamiento",
                column: "UsuarioEntId");

            migrationBuilder.CreateIndex(
                name: "IX_SesionEntrenamiento_IdAsignacion",
                table: "SesionEntrenamiento",
                column: "IdAsignacion");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Correo",
                table: "Usuarios",
                column: "Correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolId",
                table: "Usuarios",
                column: "RolId");

            migrationBuilder.AddForeignKey(
                name: "FK_EntrenadorAtleta_Atleta",
                table: "EntrenadorAtleta",
                column: "IdAtleta",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EntrenadorAtleta_Entrenador",
                table: "EntrenadorAtleta",
                column: "IdEntrenador",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntrenadorAtleta_Atleta",
                table: "EntrenadorAtleta");

            migrationBuilder.DropForeignKey(
                name: "FK_EntrenadorAtleta_Entrenador",
                table: "EntrenadorAtleta");

            migrationBuilder.DropTable(
                name: "DetalleZonaPlan");

            migrationBuilder.DropTable(
                name: "DetalleZonaSesion");

            migrationBuilder.DropTable(
                name: "SesionEntrenamiento");

            migrationBuilder.DropTable(
                name: "ZonaEntrenamiento");

            migrationBuilder.DropTable(
                name: "AsignacionRutina");

            migrationBuilder.DropTable(
                name: "Entrenamiento");

            migrationBuilder.DropTable(
                name: "Rutina");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EntrenadorAtleta",
                table: "EntrenadorAtleta");

            migrationBuilder.DropIndex(
                name: "IX_EntrenadorAtleta_IdEntrenador",
                table: "EntrenadorAtleta");

            migrationBuilder.DropColumn(
                name: "IdRelacion",
                table: "EntrenadorAtleta");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "EntrenadorAtleta");

            migrationBuilder.AlterColumn<bool>(
                name: "Estado",
                table: "Permiso",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaAsignacion",
                table: "EntrenadorAtleta",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EntrenadorAtleta",
                table: "EntrenadorAtleta",
                columns: new[] { "IdEntrenador", "IdAtleta" });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolId = table.Column<int>(type: "int", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContrasennaHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaCumpleannos = table.Column<DateTime>(type: "date", nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "datetime", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuario_Rol_RolId",
                        column: x => x.RolId,
                        principalTable: "Rol",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "IdRol",
                keyValue: -3,
                column: "Descripcion",
                value: "Acceso básico a funcionalidades del sistema");

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "IdRol",
                keyValue: -2,
                column: "Descripcion",
                value: "Gestión de entrenamientos y seguimiento de atletas");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_RolId",
                table: "Usuario",
                column: "RolId");

            migrationBuilder.AddForeignKey(
                name: "FK_EntrenadorAtleta_Usuario_IdAtleta",
                table: "EntrenadorAtleta",
                column: "IdAtleta",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntrenadorAtleta_Usuario_IdEntrenador",
                table: "EntrenadorAtleta",
                column: "IdEntrenador",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
