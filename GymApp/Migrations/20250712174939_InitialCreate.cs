using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EXERCICIOS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "varchar(100)", nullable: false),
                    Series = table.Column<int>(type: "integer", nullable: false),
                    Repeticoes = table.Column<int>(type: "integer", nullable: false),
                    Carga = table.Column<decimal>(type: "numeric", nullable: false),
                    Foto = table.Column<string>(type: "varchar(255)", nullable: false),
                    Video = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXERCICIOS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TREINOS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "varchar(100)", nullable: false),
                    Usuario = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TREINOS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExercicioTreino",
                columns: table => new
                {
                    ExerciciosId = table.Column<Guid>(type: "uuid", nullable: false),
                    TreinoId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExercicioTreino", x => new { x.ExerciciosId, x.TreinoId });
                    table.ForeignKey(
                        name: "FK_ExercicioTreino_EXERCICIOS_ExerciciosId",
                        column: x => x.ExerciciosId,
                        principalTable: "EXERCICIOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExercicioTreino_TREINOS_TreinoId",
                        column: x => x.TreinoId,
                        principalTable: "TREINOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExercicioTreino_TreinoId",
                table: "ExercicioTreino",
                column: "TreinoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExercicioTreino");

            migrationBuilder.DropTable(
                name: "EXERCICIOS");

            migrationBuilder.DropTable(
                name: "TREINOS");
        }
    }
}
