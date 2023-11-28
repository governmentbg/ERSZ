using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERSZ.Infrastructure.Migrations
{
    public partial class RestructureJurorMandateSetNullFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_juror_mandate_common_court_court_id",
                table: "juror_mandate");

            migrationBuilder.DropForeignKey(
                name: "fk_juror_mandate_ek_munincipality_municipality_id",
                table: "juror_mandate");

            migrationBuilder.AlterColumn<int>(
                name: "municipality_id",
                table: "juror_mandate",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "court_id",
                table: "juror_mandate",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "fk_juror_mandate_common_court_court_id",
                table: "juror_mandate",
                column: "court_id",
                principalTable: "common_court",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_juror_mandate_ek_munincipality_municipality_id",
                table: "juror_mandate",
                column: "municipality_id",
                principalTable: "ek_munincipality",
                principalColumn: "municipality_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_juror_mandate_common_court_court_id",
                table: "juror_mandate");

            migrationBuilder.DropForeignKey(
                name: "fk_juror_mandate_ek_munincipality_municipality_id",
                table: "juror_mandate");

            migrationBuilder.AlterColumn<int>(
                name: "municipality_id",
                table: "juror_mandate",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "court_id",
                table: "juror_mandate",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_juror_mandate_common_court_court_id",
                table: "juror_mandate",
                column: "court_id",
                principalTable: "common_court",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_juror_mandate_ek_munincipality_municipality_id",
                table: "juror_mandate",
                column: "municipality_id",
                principalTable: "ek_munincipality",
                principalColumn: "municipality_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
