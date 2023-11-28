using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERSZ.Infrastructure.Migrations
{
    public partial class RestructureJurorMandateAddDateTermination : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "description_date_end",
                table: "juror_mandate",
                newName: "date_termination_description");

            migrationBuilder.AddColumn<DateTime>(
                name: "date_termination",
                table: "juror_mandate",
                type: "timestamp without time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date_termination",
                table: "juror_mandate");

            migrationBuilder.RenameColumn(
                name: "date_termination_description",
                table: "juror_mandate",
                newName: "description_date_end");
        }
    }
}
