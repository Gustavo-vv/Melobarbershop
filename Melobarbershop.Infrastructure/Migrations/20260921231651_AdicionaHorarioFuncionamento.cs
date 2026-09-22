using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Melobarbershop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaHorarioFuncionamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HorariosEspeciais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<DateTime>(type: "date", nullable: false),
                    Aberto = table.Column<bool>(type: "bit", nullable: false),
                    HoraAbertura = table.Column<TimeSpan>(type: "time", nullable: true),
                    HoraFechamento = table.Column<TimeSpan>(type: "time", nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorariosEspeciais", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HorariosFuncionamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiaSemana = table.Column<int>(type: "int", nullable: false),
                    Aberto = table.Column<bool>(type: "bit", nullable: false),
                    HoraAbertura = table.Column<TimeSpan>(type: "time", nullable: false),
                    HoraFechamento = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorariosFuncionamento", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HorariosEspeciais_Data",
                table: "HorariosEspeciais",
                column: "Data",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HorariosFuncionamento_DiaSemana",
                table: "HorariosFuncionamento",
                column: "DiaSemana",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HorariosEspeciais");

            migrationBuilder.DropTable(
                name: "HorariosFuncionamento");
        }
    }
}
