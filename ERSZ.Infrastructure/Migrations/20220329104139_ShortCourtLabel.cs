using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERSZ.Infrastructure.Migrations
{
    public partial class ShortCourtLabel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "short_label",
                table: "common_court",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "short_label",
                table: "common_court");
        }
    }
}
