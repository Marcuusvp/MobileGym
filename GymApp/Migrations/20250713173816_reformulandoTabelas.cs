using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymApp.Migrations
{
    /// <inheritdoc />
    public partial class reformulandoTabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExercicioTreino");

            migrationBuilder.DropColumn(
                name: "Carga",
                table: "EXERCICIOS");

            migrationBuilder.DropColumn(
                name: "Repeticoes",
                table: "EXERCICIOS");

            migrationBuilder.DropColumn(
                name: "Series",
                table: "EXERCICIOS");

            migrationBuilder.CreateTable(
                name: "EXERCICIO_TREINO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TreinoId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExercicioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Series = table.Column<int>(type: "integer", nullable: false),
                    Repeticoes = table.Column<int>(type: "integer", nullable: false),
                    Carga = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXERCICIO_TREINO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EXERCICIO_TREINO_EXERCICIOS_ExercicioId",
                        column: x => x.ExercicioId,
                        principalTable: "EXERCICIOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EXERCICIO_TREINO_TREINOS_TreinoId",
                        column: x => x.TreinoId,
                        principalTable: "TREINOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EXERCICIO_TREINO_ExercicioId",
                table: "EXERCICIO_TREINO",
                column: "ExercicioId");

            migrationBuilder.CreateIndex(
                name: "IX_EXERCICIO_TREINO_TreinoId",
                table: "EXERCICIO_TREINO",
                column: "TreinoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EXERCICIO_TREINO");

            migrationBuilder.AddColumn<decimal>(
                name: "Carga",
                table: "EXERCICIOS",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Repeticoes",
                table: "EXERCICIOS",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Series",
                table: "EXERCICIOS",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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
    }
}
