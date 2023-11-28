﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERSZ.Infrastructure.Migrations
{
    public partial class RestructureJurorMandateAddDescriptionDateEnd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description_date_end",
                table: "juror_mandate",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description_date_end",
                table: "juror_mandate");
        }
    }
}
