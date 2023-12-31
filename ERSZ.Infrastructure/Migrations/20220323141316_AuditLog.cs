﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ERSZ.Infrastructure.Migrations
{
    public partial class AuditLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "audit_log",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: true),
                    date_wrt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    operation = table.Column<string>(type: "text", nullable: true),
                    object_info = table.Column<string>(type: "text", nullable: true),
                    action_info = table.Column<string>(type: "text", nullable: true),
                    request_url = table.Column<string>(type: "text", nullable: true),
                    client_ip = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audit_log", x => x.id);
                    table.ForeignKey(
                        name: "fk_audit_log_application_user_user_id",
                        column: x => x.user_id,
                        principalTable: "identity_users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "log_operation",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    operation = table.Column<string>(type: "text", nullable: true),
                    controller = table.Column<string>(type: "text", nullable: true),
                    action = table.Column<string>(type: "text", nullable: true),
                    object_id = table.Column<string>(type: "text", nullable: true),
                    user_wrt = table.Column<string>(type: "text", nullable: true),
                    date_wrt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    user_data = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_log_operation", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_audit_log_user_id",
                table: "audit_log",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audit_log");

            migrationBuilder.DropTable(
                name: "log_operation");
        }
    }
}
