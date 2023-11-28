using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERSZ.Infrastructure.Migrations
{
    public partial class AddFiledEducationId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_jurors_nom_education_rang_education_rang_id",
                table: "jurors");

            migrationBuilder.AddColumn<int>(
                name: "education_id",
                table: "nom_education_rang",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "education_rang_id",
                table: "jurors",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "ix_nom_education_rang_education_id",
                table: "nom_education_rang",
                column: "education_id");

            migrationBuilder.AddForeignKey(
                name: "fk_jurors_nom_education_rang_education_rang_id",
                table: "jurors",
                column: "education_rang_id",
                principalTable: "nom_education_rang",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_nom_education_rang_nom_education_education_id",
                table: "nom_education_rang",
                column: "education_id",
                principalTable: "nom_education",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_jurors_nom_education_rang_education_rang_id",
                table: "jurors");

            migrationBuilder.DropForeignKey(
                name: "fk_nom_education_rang_nom_education_education_id",
                table: "nom_education_rang");

            migrationBuilder.DropIndex(
                name: "ix_nom_education_rang_education_id",
                table: "nom_education_rang");

            migrationBuilder.DropColumn(
                name: "education_id",
                table: "nom_education_rang");

            migrationBuilder.AlterColumn<int>(
                name: "education_rang_id",
                table: "jurors",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_jurors_nom_education_rang_education_rang_id",
                table: "jurors",
                column: "education_rang_id",
                principalTable: "nom_education_rang",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
