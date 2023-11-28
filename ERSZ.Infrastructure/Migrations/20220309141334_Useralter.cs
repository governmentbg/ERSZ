using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERSZ.Infrastructure.Migrations
{
    public partial class Useralter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "court_id",
                table: "identity_users",
                type: "integer",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "uic",
                table: "identity_users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_identity_users_court_id",
                table: "identity_users",
                column: "court_id");

            migrationBuilder.AddForeignKey(
                name: "fk_identity_users_common_court_court_id",
                table: "identity_users",
                column: "court_id",
                principalTable: "common_court",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_identity_users_common_court_court_id",
                table: "identity_users");

            migrationBuilder.DropIndex(
                name: "ix_identity_users_court_id",
                table: "identity_users");

            migrationBuilder.DropColumn(
                name: "court_id",
                table: "identity_users");

            migrationBuilder.DropColumn(
                name: "uic",
                table: "identity_users");
        }
    }
}
