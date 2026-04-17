using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MomiaTrainSync.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogErrores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Origen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExcepcionInterna = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrazaError = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogErrores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permiso",
                columns: table => new
                {
                    IdPermiso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ruta = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permiso", x => x.IdPermiso);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "TipoSesion",
                columns: table => new
                {
                    IdTipoSesion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoSesion", x => x.IdTipoSesion);
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
                name: "RolPermiso",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false),
                    IdPermiso = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolPermiso", x => new { x.IdRol, x.IdPermiso });
                    table.ForeignKey(
                        name: "FK_RolPermiso_Permiso_IdPermiso",
                        column: x => x.IdPermiso,
                        principalTable: "Permiso",
                        principalColumn: "IdPermiso",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolPermiso_Rol_IdRol",
                        column: x => x.IdRol,
                        principalTable: "Rol",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
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
                    Biografia = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
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
                name: "EntrenadorAtleta",
                columns: table => new
                {
                    IdRelacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEntrenador = table.Column<int>(type: "int", nullable: false),
                    IdAtleta = table.Column<int>(type: "int", nullable: false),
                    FechaAsignacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntrenadorAtleta", x => x.IdRelacion);
                    table.ForeignKey(
                        name: "FK_EntrenadorAtleta_Atleta",
                        column: x => x.IdAtleta,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EntrenadorAtleta_Entrenador",
                        column: x => x.IdEntrenador,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rutina",
                columns: table => new
                {
                    IdRutina = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdRelacion = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rutina", x => x.IdRutina);
                    table.ForeignKey(
                        name: "FK_Rutina_EntrenadorAtleta_IdRelacion",
                        column: x => x.IdRelacion,
                        principalTable: "EntrenadorAtleta",
                        principalColumn: "IdRelacion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Entrenamiento",
                columns: table => new
                {
                    IdEntrenamiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdRutina = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IdTipoSesion = table.Column<int>(type: "int", nullable: false),
                    Objetivo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DuracionEstimada = table.Column<int>(type: "int", nullable: false),
                    NivelEsfuerzoEsperado = table.Column<byte>(type: "tinyint", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    FechaProgramada = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrenamiento", x => x.IdEntrenamiento);
                    table.ForeignKey(
                        name: "FK_Entrenamiento_Rutina_IdRutina",
                        column: x => x.IdRutina,
                        principalTable: "Rutina",
                        principalColumn: "IdRutina",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entrenamiento_TipoSesion_IdTipoSesion",
                        column: x => x.IdTipoSesion,
                        principalTable: "TipoSesion",
                        principalColumn: "IdTipoSesion",
                        onDelete: ReferentialAction.Restrict);
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
                    IdEntrenamiento = table.Column<int>(type: "int", nullable: false),
                    FechaEjecucion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DuracionReal = table.Column<int>(type: "int", nullable: false),
                    NivelEsfuerzoPercibido = table.Column<byte>(type: "tinyint", nullable: false),
                    Comentarios = table.Column<string>(type: "text", nullable: true),
                    CargaTotal = table.Column<decimal>(type: "decimal(6,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SesionEntrenamiento", x => x.IdSesion);
                    table.ForeignKey(
                        name: "FK_SesionEntrenamiento_Entrenamiento_IdEntrenamiento",
                        column: x => x.IdEntrenamiento,
                        principalTable: "Entrenamiento",
                        principalColumn: "IdEntrenamiento",
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
                name: "IX_EntrenadorAtleta_IdAtleta",
                table: "EntrenadorAtleta",
                column: "IdAtleta");

            migrationBuilder.CreateIndex(
                name: "IX_EntrenadorAtleta_IdEntrenador",
                table: "EntrenadorAtleta",
                column: "IdEntrenador");

            migrationBuilder.CreateIndex(
                name: "IX_Entrenamiento_IdRutina",
                table: "Entrenamiento",
                column: "IdRutina");

            migrationBuilder.CreateIndex(
                name: "IX_Entrenamiento_IdTipoSesion",
                table: "Entrenamiento",
                column: "IdTipoSesion");

            migrationBuilder.CreateIndex(
                name: "IX_Rol_Nombre",
                table: "Rol",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolPermiso_IdPermiso",
                table: "RolPermiso",
                column: "IdPermiso");

            migrationBuilder.CreateIndex(
                name: "IX_Rutina_IdRelacion",
                table: "Rutina",
                column: "IdRelacion");

            migrationBuilder.CreateIndex(
                name: "IX_SesionEntrenamiento_IdEntrenamiento",
                table: "SesionEntrenamiento",
                column: "IdEntrenamiento");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Correo",
                table: "Usuarios",
                column: "Correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolId",
                table: "Usuarios",
                column: "RolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetalleZonaPlan");

            migrationBuilder.DropTable(
                name: "DetalleZonaSesion");

            migrationBuilder.DropTable(
                name: "LogErrores");

            migrationBuilder.DropTable(
                name: "RolPermiso");

            migrationBuilder.DropTable(
                name: "SesionEntrenamiento");

            migrationBuilder.DropTable(
                name: "ZonaEntrenamiento");

            migrationBuilder.DropTable(
                name: "Permiso");

            migrationBuilder.DropTable(
                name: "Entrenamiento");

            migrationBuilder.DropTable(
                name: "Rutina");

            migrationBuilder.DropTable(
                name: "TipoSesion");

            migrationBuilder.DropTable(
                name: "EntrenadorAtleta");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Rol");
        }
    }
}
