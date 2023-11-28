using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERSZ.Infrastructure.Migrations
{
    public partial class RestructureJurorAddPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "content",
                table: "jurors",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "content_type",
                table: "jurors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "file_name",
                table: "jurors",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "content",
                table: "jurors");

            migrationBuilder.DropColumn(
                name: "content_type",
                table: "jurors");

            migrationBuilder.DropColumn(
                name: "file_name",
                table: "jurors");
        }
    }
}
