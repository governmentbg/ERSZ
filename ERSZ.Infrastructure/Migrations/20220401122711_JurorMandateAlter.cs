using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERSZ.Infrastructure.Migrations
{
    public partial class JurorMandateAlter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_case_selection_protokol_jurors_juror_id",
                table: "case_selection_protokol");

            migrationBuilder.DropForeignKey(
                name: "fk_case_session_amount_jurors_juror_id",
                table: "case_session_amount");

            migrationBuilder.DropIndex(
                name: "ix_case_session_amount_juror_id",
                table: "case_session_amount");

            migrationBuilder.DropIndex(
                name: "ix_case_selection_protokol_juror_id",
                table: "case_selection_protokol");

            migrationBuilder.DropColumn(
                name: "juror_id",
                table: "case_session_amount");

            migrationBuilder.DropColumn(
                name: "juror_id",
                table: "case_selection_protokol");

            migrationBuilder.AddColumn<string>(
                name: "mandate_no",
                table: "juror_mandate",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "municipality_id",
                table: "juror_mandate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "parent_id",
                table: "juror_mandate",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "juror_mandate_id",
                table: "case_session_amount",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "juror_mandate_id",
                table: "case_selection_protokol",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_juror_mandate_municipality_id",
                table: "juror_mandate",
                column: "municipality_id");

            migrationBuilder.CreateIndex(
                name: "ix_juror_mandate_parent_id",
                table: "juror_mandate",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_case_session_amount_juror_mandate_id",
                table: "case_session_amount",
                column: "juror_mandate_id");

            migrationBuilder.CreateIndex(
                name: "ix_case_selection_protokol_juror_mandate_id",
                table: "case_selection_protokol",
                column: "juror_mandate_id");

            migrationBuilder.AddForeignKey(
                name: "fk_case_selection_protokol_juror_mandate_juror_mandate_id",
                table: "case_selection_protokol",
                column: "juror_mandate_id",
                principalTable: "juror_mandate",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_case_session_amount_juror_mandate_juror_mandate_id",
                table: "case_session_amount",
                column: "juror_mandate_id",
                principalTable: "juror_mandate",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_juror_mandate_ek_munincipality_municipality_id",
                table: "juror_mandate",
                column: "municipality_id",
                principalTable: "ek_munincipality",
                principalColumn: "municipality_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_juror_mandate_juror_mandate_parent_id",
                table: "juror_mandate",
                column: "parent_id",
                principalTable: "juror_mandate",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_case_selection_protokol_juror_mandate_juror_mandate_id",
                table: "case_selection_protokol");

            migrationBuilder.DropForeignKey(
                name: "fk_case_session_amount_juror_mandate_juror_mandate_id",
                table: "case_session_amount");

            migrationBuilder.DropForeignKey(
                name: "fk_juror_mandate_ek_munincipality_municipality_id",
                table: "juror_mandate");

            migrationBuilder.DropForeignKey(
                name: "fk_juror_mandate_juror_mandate_parent_id",
                table: "juror_mandate");

            migrationBuilder.DropIndex(
                name: "ix_juror_mandate_municipality_id",
                table: "juror_mandate");

            migrationBuilder.DropIndex(
                name: "ix_juror_mandate_parent_id",
                table: "juror_mandate");

            migrationBuilder.DropIndex(
                name: "ix_case_session_amount_juror_mandate_id",
                table: "case_session_amount");

            migrationBuilder.DropIndex(
                name: "ix_case_selection_protokol_juror_mandate_id",
                table: "case_selection_protokol");

            migrationBuilder.DropColumn(
                name: "mandate_no",
                table: "juror_mandate");

            migrationBuilder.DropColumn(
                name: "municipality_id",
                table: "juror_mandate");

            migrationBuilder.DropColumn(
                name: "parent_id",
                table: "juror_mandate");

            migrationBuilder.DropColumn(
                name: "juror_mandate_id",
                table: "case_session_amount");

            migrationBuilder.DropColumn(
                name: "juror_mandate_id",
                table: "case_selection_protokol");

            migrationBuilder.AddColumn<string>(
                name: "juror_id",
                table: "case_session_amount",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "juror_id",
                table: "case_selection_protokol",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_case_session_amount_juror_id",
                table: "case_session_amount",
                column: "juror_id");

            migrationBuilder.CreateIndex(
                name: "ix_case_selection_protokol_juror_id",
                table: "case_selection_protokol",
                column: "juror_id");

            migrationBuilder.AddForeignKey(
                name: "fk_case_selection_protokol_jurors_juror_id",
                table: "case_selection_protokol",
                column: "juror_id",
                principalTable: "jurors",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_case_session_amount_jurors_juror_id",
                table: "case_session_amount",
                column: "juror_id",
                principalTable: "jurors",
                principalColumn: "id");
        }
    }
}
