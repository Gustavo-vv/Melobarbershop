using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Melobarbershop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPercentualComissaoToUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PercentualComissao",
                table: "Usuarios",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PercentualComissao",
                table: "Usuarios");
        }
    }
}
