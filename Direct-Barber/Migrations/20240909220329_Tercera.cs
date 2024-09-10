using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Direct_Barber.Migrations
{
    /// <inheritdoc />
    public partial class Tercera : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    apellido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    correo = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    contrasena = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    direccion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fec_registro = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    fec_nacimiento = table.Column<DateOnly>(type: "date", nullable: true),
                    calificacion = table.Column<decimal>(type: "decimal(3,2)", nullable: true),
                    foto = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    documento = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Id_Rol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuario_Rol",
                        column: x => x.Id_Rol,
                        principalTable: "Rol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Id_Rol",
                table: "Usuario",
                column: "Id_Rol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Rol");
        }
    }
}
