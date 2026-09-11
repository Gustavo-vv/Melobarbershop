using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Melobarbershop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTelefoneWhatsappApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TelefoneWhatsApp",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TelefoneWhatsApp",
                table: "Usuarios");
        }
    }
}
