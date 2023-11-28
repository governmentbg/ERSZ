using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ERSZ.Infrastructure.Migrations
{
    public partial class CaseData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "nom_case_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_number = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<string>(type: "text", nullable: true),
                    label = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    date_start = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    date_end = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_nom_case_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "case_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    case_type_id = table.Column<int>(type: "integer", nullable: false),
                    reg_number = table.Column<string>(type: "text", nullable: true),
                    short_number = table.Column<string>(type: "text", nullable: true),
                    reg_year = table.Column<int>(type: "integer", nullable: false),
                    court_id = table.Column<int>(type: "integer", nullable: false),
                    gid = table.Column<string>(type: "text", nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    date_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_case_data", x => x.id);
                    table.ForeignKey(
                        name: "fk_case_data_common_court_court_id",
                        column: x => x.court_id,
                        principalTable: "common_court",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_case_data_nom_case_type_case_type_id",
                        column: x => x.case_type_id,
                        principalTable: "nom_case_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "case_dismissal",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    case_id = table.Column<int>(type: "integer", nullable: false),
                    juror_id = table.Column<string>(type: "text", nullable: true),
                    dismissal_kind = table.Column<string>(type: "text", nullable: true),
                    reason = table.Column<string>(type: "text", nullable: true),
                    dismissal_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    court_id = table.Column<int>(type: "integer", nullable: false),
                    gid = table.Column<string>(type: "text", nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    date_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_case_dismissal", x => x.id);
                    table.ForeignKey(
                        name: "fk_case_dismissal_case_data_case_id",
                        column: x => x.case_id,
                        principalTable: "case_data",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_case_dismissal_common_court_court_id",
                        column: x => x.court_id,
                        principalTable: "common_court",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_case_dismissal_jurors_juror_id",
                        column: x => x.juror_id,
                        principalTable: "jurors",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "case_selection_protokol",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    case_id = table.Column<int>(type: "integer", nullable: false),
                    juror_id = table.Column<string>(type: "text", nullable: true),
                    selection_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    court_id = table.Column<int>(type: "integer", nullable: false),
                    gid = table.Column<string>(type: "text", nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    date_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_case_selection_protokol", x => x.id);
                    table.ForeignKey(
                        name: "fk_case_selection_protokol_case_data_case_id",
                        column: x => x.case_id,
                        principalTable: "case_data",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_case_selection_protokol_common_court_court_id",
                        column: x => x.court_id,
                        principalTable: "common_court",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_case_selection_protokol_jurors_juror_id",
                        column: x => x.juror_id,
                        principalTable: "jurors",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "case_session",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    case_id = table.Column<int>(type: "integer", nullable: false),
                    session_kind = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    result = table.Column<string>(type: "text", nullable: true),
                    result_base = table.Column<string>(type: "text", nullable: true),
                    date_start = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    date_end = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    court_id = table.Column<int>(type: "integer", nullable: false),
                    gid = table.Column<string>(type: "text", nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    date_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_case_session", x => x.id);
                    table.ForeignKey(
                        name: "fk_case_session_case_data_case_id",
                        column: x => x.case_id,
                        principalTable: "case_data",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_case_session_common_court_court_id",
                        column: x => x.court_id,
                        principalTable: "common_court",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "case_session_act",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    case_sesion_id = table.Column<int>(type: "integer", nullable: false),
                    act_kind = table.Column<string>(type: "text", nullable: true),
                    reg_number = table.Column<string>(type: "text", nullable: true),
                    reg_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    court_id = table.Column<int>(type: "integer", nullable: false),
                    gid = table.Column<string>(type: "text", nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    date_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_case_session_act", x => x.id);
                    table.ForeignKey(
                        name: "fk_case_session_act_case_session_case_sesion_id",
                        column: x => x.case_sesion_id,
                        principalTable: "case_session",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_case_session_act_common_court_court_id",
                        column: x => x.court_id,
                        principalTable: "common_court",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "case_session_amount",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    juror_id = table.Column<string>(type: "text", nullable: true),
                    case_id = table.Column<int>(type: "integer", nullable: false),
                    case_sesion_id = table.Column<int>(type: "integer", nullable: false),
                    fee = table.Column<decimal>(type: "numeric", nullable: false),
                    expences = table.Column<decimal>(type: "numeric", nullable: false),
                    fine = table.Column<decimal>(type: "numeric", nullable: false),
                    fine_is_paid = table.Column<bool>(type: "boolean", nullable: false),
                    court_id = table.Column<int>(type: "integer", nullable: false),
                    gid = table.Column<string>(type: "text", nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    date_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_case_session_amount", x => x.id);
                    table.ForeignKey(
                        name: "fk_case_session_amount_case_data_case_id",
                        column: x => x.case_id,
                        principalTable: "case_data",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_case_session_amount_case_session_case_sesion_id",
                        column: x => x.case_sesion_id,
                        principalTable: "case_session",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_case_session_amount_common_court_court_id",
                        column: x => x.court_id,
                        principalTable: "common_court",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_case_session_amount_jurors_juror_id",
                        column: x => x.juror_id,
                        principalTable: "jurors",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_case_data_case_type_id",
                table: "case_data",
                column: "case_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_case_data_court_id",
                table: "case_data",
                column: "court_id");

            migrationBuilder.CreateIndex(
                name: "ix_case_dismissal_case_id",
                table: "case_dismissal",
                column: "case_id");

            migrationBuilder.CreateIndex(
                name: "ix_case_dismissal_court_id",
                table: "case_dismissal",
                column: "court_id");

            migrationBuilder.CreateIndex(
                name: "ix_case_dismissal_juror_id",
                table: "case_dismissal",
                column: "juror_id");

            migrationBuilder.CreateIndex(
                name: "ix_case_selection_protokol_case_id",
                table: "case_selection_protokol",
                column: "case_id");

            migrationBuilder.CreateIndex(
                name: "ix_case_selection_protokol_court_id",
                table: "case_selection_protokol",
                column: "court_id");

            migrationBuilder.CreateIndex(
                name: "ix_case_selection_protokol_juror_id",
                table: "case_selection_protokol",
                column: "juror_id");

            migrationBuilder.CreateIndex(
                name: "ix_case_session_case_id",
                table: "case_session",
                column: "case_id");

            migrationBuilder.CreateIndex(
                name: "ix_case_session_court_id",
                table: "case_session",
                column: "court_id");

            migrationBuilder.CreateIndex(
                name: "ix_case_session_act_case_sesion_id",
                table: "case_session_act",
                column: "case_sesion_id");

            migrationBuilder.CreateIndex(
                name: "ix_case_session_act_court_id",
                table: "case_session_act",
                column: "court_id");

            migrationBuilder.CreateIndex(
                name: "ix_case_session_amount_case_id",
                table: "case_session_amount",
                column: "case_id");

            migrationBuilder.CreateIndex(
                name: "ix_case_session_amount_case_sesion_id",
                table: "case_session_amount",
                column: "case_sesion_id");

            migrationBuilder.CreateIndex(
                name: "ix_case_session_amount_court_id",
                table: "case_session_amount",
                column: "court_id");

            migrationBuilder.CreateIndex(
                name: "ix_case_session_amount_juror_id",
                table: "case_session_amount",
                column: "juror_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "case_dismissal");

            migrationBuilder.DropTable(
                name: "case_selection_protokol");

            migrationBuilder.DropTable(
                name: "case_session_act");

            migrationBuilder.DropTable(
                name: "case_session_amount");

            migrationBuilder.DropTable(
                name: "case_session");

            migrationBuilder.DropTable(
                name: "case_data");

            migrationBuilder.DropTable(
                name: "nom_case_type");
        }
    }
}
