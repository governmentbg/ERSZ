using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERSZ.Infrastructure.Migrations
{
    public partial class AddExpire : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date_expired",
                table: "jurors");

            migrationBuilder.DropColumn(
                name: "expired_description",
                table: "jurors");

            migrationBuilder.DropColumn(
                name: "expired_user_id",
                table: "jurors");

            migrationBuilder.DropColumn(
                name: "date_expired",
                table: "juror_speciality");

            migrationBuilder.DropColumn(
                name: "expired_description",
                table: "juror_speciality");

            migrationBuilder.DropColumn(
                name: "expired_user_id",
                table: "juror_speciality");

            migrationBuilder.DropColumn(
                name: "date_expired",
                table: "juror_mandate");

            migrationBuilder.DropColumn(
                name: "expired_description",
                table: "juror_mandate");

            migrationBuilder.DropColumn(
                name: "expired_user_id",
                table: "juror_mandate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "date_expired",
                table: "jurors",
                type: "timestamp without time zone",
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

            migrationBuilder.AddColumn<DateTime>(
                name: "date_expired",
                table: "juror_speciality",
                type: "timestamp without time zone",
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

            migrationBuilder.AddColumn<DateTime>(
                name: "date_expired",
                table: "juror_mandate",
                type: "timestamp without time zone",
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
        }
    }
}
