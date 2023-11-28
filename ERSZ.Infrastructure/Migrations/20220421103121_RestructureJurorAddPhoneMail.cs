using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERSZ.Infrastructure.Migrations
{
    public partial class RestructureJurorAddPhoneMail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "e_mail",
                table: "jurors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "phone",
                table: "jurors",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "e_mail",
                table: "jurors");

            migrationBuilder.DropColumn(
                name: "phone",
                table: "jurors");
        }
    }
}
