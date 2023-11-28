using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERSZ.Infrastructure.Migrations
{
    public partial class AddFieldDescriptionUserExpired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "expired_description",
                table: "mongo_file",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "expired_user_id",
                table: "mongo_file",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "expired_description",
                table: "jurors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "expired_user_id",
                table: "jurors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "expired_description",
                table: "juror_speciality",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "expired_user_id",
                table: "juror_speciality",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "expired_description",
                table: "juror_mandate",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "expired_user_id",
                table: "juror_mandate",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "counter_value_v_ms",
                columns: table => new
                {
                    value = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "counter_value_v_ms");

            migrationBuilder.DropColumn(
                name: "expired_description",
                table: "mongo_file");

            migrationBuilder.DropColumn(
                name: "expired_user_id",
                table: "mongo_file");

            migrationBuilder.DropColumn(
                name: "expired_description",
                table: "jurors");

            migrationBuilder.DropColumn(
                name: "expired_user_id",
                table: "jurors");

            migrationBuilder.DropColumn(
                name: "expired_description",
                table: "juror_speciality");

            migrationBuilder.DropColumn(
                name: "expired_user_id",
                table: "juror_speciality");

            migrationBuilder.DropColumn(
                name: "expired_description",
                table: "juror_mandate");

            migrationBuilder.DropColumn(
                name: "expired_user_id",
                table: "juror_mandate");
        }
    }
}
