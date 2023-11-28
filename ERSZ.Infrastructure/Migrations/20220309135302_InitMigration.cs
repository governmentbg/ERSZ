using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ERSZ.Infrastructure.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ek_country",
                columns: table => new
                {
                    country_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    eispp_code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ek_country", x => x.country_id);
                });

            migrationBuilder.CreateTable(
                name: "identity_roles",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identity_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "identity_users",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    must_change_password = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    full_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identity_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "nom_apeal_region",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    apeal_region_type = table.Column<int>(type: "integer", nullable: true),
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
                    table.PrimaryKey("pk_nom_apeal_region", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "nom_court_type",
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
                    table.PrimaryKey("pk_nom_court_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "nom_education",
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
                    table.PrimaryKey("pk_nom_education", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "nom_education_rang",
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
                    table.PrimaryKey("pk_nom_education_rang", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "nom_file_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    source_type = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("pk_nom_file_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "nom_mandate_type",
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
                    table.PrimaryKey("pk_nom_mandate_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "nom_speciality",
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
                    table.PrimaryKey("pk_nom_speciality", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ek_district",
                columns: table => new
                {
                    district_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    oblast = table.Column<string>(type: "text", nullable: true),
                    ekatte = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    region = table.Column<string>(type: "text", nullable: true),
                    document = table.Column<string>(type: "text", nullable: true),
                    abc = table.Column<string>(type: "text", nullable: true),
                    country_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ek_district", x => x.district_id);
                    table.ForeignKey(
                        name: "fk_ek_district_ek_country_country_id",
                        column: x => x.country_id,
                        principalTable: "ek_country",
                        principalColumn: "country_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_role_claims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<string>(type: "text", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identity_role_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_identity_role_claims_identity_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "identity_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_user_claims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identity_user_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_identity_user_claims_identity_users_user_id",
                        column: x => x.user_id,
                        principalTable: "identity_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_user_logins",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    provider_key = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    provider_display_name = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identity_user_logins", x => new { x.provider_key, x.login_provider });
                    table.ForeignKey(
                        name: "fk_identity_user_logins_identity_users_user_id",
                        column: x => x.user_id,
                        principalTable: "identity_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_user_roles",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    role_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identity_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_identity_user_roles_identity_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "identity_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_identity_user_roles_identity_users_user_id",
                        column: x => x.user_id,
                        principalTable: "identity_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_user_tokens",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    login_provider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identity_user_tokens", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "fk_identity_user_tokens_identity_users_user_id",
                        column: x => x.user_id,
                        principalTable: "identity_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "common_court",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    court_type_id = table.Column<int>(type: "integer", nullable: false),
                    longitude = table.Column<string>(type: "text", nullable: true),
                    latitude = table.Column<string>(type: "text", nullable: true),
                    city_code = table.Column<string>(type: "text", nullable: true),
                    apeal_region_id = table.Column<int>(type: "integer", nullable: true),
                    order_number = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<string>(type: "text", nullable: true),
                    label = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    date_start = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    date_end = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    object_id = table.Column<int>(type: "integer", nullable: false),
                    parent_id = table.Column<int>(type: "integer", nullable: true),
                    parent_object_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_common_court", x => x.id);
                    table.ForeignKey(
                        name: "fk_common_court_common_court_parent_id",
                        column: x => x.parent_id,
                        principalTable: "common_court",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_common_court_nom_apeal_region_apeal_region_id",
                        column: x => x.apeal_region_id,
                        principalTable: "nom_apeal_region",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_common_court_nom_court_type_court_type_id",
                        column: x => x.court_type_id,
                        principalTable: "nom_court_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mongo_file",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    file_id = table.Column<string>(type: "text", nullable: true),
                    source_type = table.Column<int>(type: "integer", nullable: false),
                    source_id = table.Column<string>(type: "text", nullable: true),
                    file_name = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    file_size = table.Column<int>(type: "integer", nullable: false),
                    file_type_id = table.Column<int>(type: "integer", nullable: true),
                    user_id = table.Column<string>(type: "text", nullable: true),
                    date_wrt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    date_expired = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mongo_file", x => x.id);
                    table.ForeignKey(
                        name: "fk_mongo_file_nom_file_type_file_type_id",
                        column: x => x.file_type_id,
                        principalTable: "nom_file_type",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ek_munincipality",
                columns: table => new
                {
                    municipality_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    municipality = table.Column<string>(type: "text", nullable: true),
                    ekatte = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    category = table.Column<string>(type: "text", nullable: true),
                    document = table.Column<string>(type: "text", nullable: true),
                    abc = table.Column<string>(type: "text", nullable: true),
                    district_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ek_munincipality", x => x.municipality_id);
                    table.ForeignKey(
                        name: "fk_ek_munincipality_ek_district_district_id",
                        column: x => x.district_id,
                        principalTable: "ek_district",
                        principalColumn: "district_id");
                });

            migrationBuilder.CreateTable(
                name: "ek_ekatte",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ekatte = table.Column<string>(type: "text", nullable: true),
                    tvm = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    oblast = table.Column<string>(type: "text", nullable: true),
                    obstina = table.Column<string>(type: "text", nullable: true),
                    kmetstvo = table.Column<string>(type: "text", nullable: true),
                    kind = table.Column<string>(type: "text", nullable: true),
                    category = table.Column<string>(type: "text", nullable: true),
                    altitude = table.Column<string>(type: "text", nullable: true),
                    document = table.Column<string>(type: "text", nullable: true),
                    tsb = table.Column<string>(type: "text", nullable: true),
                    abc = table.Column<string>(type: "text", nullable: true),
                    lat = table.Column<string>(type: "text", nullable: true),
                    lon = table.Column<string>(type: "text", nullable: true),
                    district_id = table.Column<int>(type: "integer", nullable: true),
                    country_id = table.Column<int>(type: "integer", nullable: false),
                    municipal_id = table.Column<int>(type: "integer", nullable: true),
                    eispp_code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ek_ekatte", x => x.id);
                    table.ForeignKey(
                        name: "fk_ek_ekatte_ek_country_country_id",
                        column: x => x.country_id,
                        principalTable: "ek_country",
                        principalColumn: "country_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_ek_ekatte_ek_district_district_id",
                        column: x => x.district_id,
                        principalTable: "ek_district",
                        principalColumn: "district_id");
                    table.ForeignKey(
                        name: "fk_ek_ekatte_ek_munincipality_municipal_id",
                        column: x => x.municipal_id,
                        principalTable: "ek_munincipality",
                        principalColumn: "municipality_id");
                });

            migrationBuilder.CreateTable(
                name: "common_court_ekatte",
                columns: table => new
                {
                    court_id = table.Column<int>(type: "integer", nullable: false),
                    ek_ekatte_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_common_court_ekatte", x => new { x.court_id, x.ek_ekatte_id });
                    table.ForeignKey(
                        name: "fk_common_court_ekatte_common_court_court_id",
                        column: x => x.court_id,
                        principalTable: "common_court",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_common_court_ekatte_ek_ekatte_ek_ekatte_id",
                        column: x => x.ek_ekatte_id,
                        principalTable: "ek_ekatte",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "jurors",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    uic = table.Column<string>(type: "text", nullable: true),
                    first_name = table.Column<string>(type: "text", nullable: true),
                    middle_name = table.Column<string>(type: "text", nullable: true),
                    family_name = table.Column<string>(type: "text", nullable: true),
                    family2name = table.Column<string>(type: "text", nullable: true),
                    full_name = table.Column<string>(type: "text", nullable: true),
                    address_city_id = table.Column<int>(type: "integer", nullable: false),
                    address_text = table.Column<string>(type: "text", nullable: true),
                    education_id = table.Column<int>(type: "integer", nullable: false),
                    education_rang_id = table.Column<int>(type: "integer", nullable: false),
                    date_start = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    date_end = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    user_id = table.Column<string>(type: "text", nullable: true),
                    date_wrt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    date_expired = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_jurors", x => x.id);
                    table.ForeignKey(
                        name: "fk_jurors_ek_ekatte_address_city_id",
                        column: x => x.address_city_id,
                        principalTable: "ek_ekatte",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_jurors_nom_education_education_id",
                        column: x => x.education_id,
                        principalTable: "nom_education",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_jurors_nom_education_rang_education_rang_id",
                        column: x => x.education_rang_id,
                        principalTable: "nom_education_rang",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "juror_mandate",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    juror_id = table.Column<string>(type: "text", nullable: true),
                    court_id = table.Column<int>(type: "integer", nullable: false),
                    register_court_id = table.Column<int>(type: "integer", nullable: true),
                    mandate_type_id = table.Column<int>(type: "integer", nullable: false),
                    date_start = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    date_end = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    user_id = table.Column<string>(type: "text", nullable: true),
                    date_wrt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    date_expired = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_juror_mandate", x => x.id);
                    table.ForeignKey(
                        name: "fk_juror_mandate_common_court_court_id",
                        column: x => x.court_id,
                        principalTable: "common_court",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_juror_mandate_common_court_register_court_id",
                        column: x => x.register_court_id,
                        principalTable: "common_court",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_juror_mandate_jurors_juror_id",
                        column: x => x.juror_id,
                        principalTable: "jurors",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_juror_mandate_nom_mandate_type_mandate_type_id",
                        column: x => x.mandate_type_id,
                        principalTable: "nom_mandate_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "juror_speciality",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    juror_id = table.Column<string>(type: "text", nullable: true),
                    speciality_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: true),
                    date_wrt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    date_expired = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_juror_speciality", x => x.id);
                    table.ForeignKey(
                        name: "fk_juror_speciality_jurors_juror_id",
                        column: x => x.juror_id,
                        principalTable: "jurors",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_juror_speciality_nom_speciality_speciality_id",
                        column: x => x.speciality_id,
                        principalTable: "nom_speciality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_common_court_apeal_region_id",
                table: "common_court",
                column: "apeal_region_id");

            migrationBuilder.CreateIndex(
                name: "ix_common_court_court_type_id",
                table: "common_court",
                column: "court_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_common_court_parent_id",
                table: "common_court",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_common_court_ekatte_ek_ekatte_id",
                table: "common_court_ekatte",
                column: "ek_ekatte_id");

            migrationBuilder.CreateIndex(
                name: "ix_ek_district_country_id",
                table: "ek_district",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "ix_ek_ekatte_country_id",
                table: "ek_ekatte",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "ix_ek_ekatte_district_id",
                table: "ek_ekatte",
                column: "district_id");

            migrationBuilder.CreateIndex(
                name: "ix_ek_ekatte_municipal_id",
                table: "ek_ekatte",
                column: "municipal_id");

            migrationBuilder.CreateIndex(
                name: "ix_ek_munincipality_district_id",
                table: "ek_munincipality",
                column: "district_id");

            migrationBuilder.CreateIndex(
                name: "ix_identity_role_claims_role_id",
                table: "identity_role_claims",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "role_name_index",
                table: "identity_roles",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_identity_user_claims_user_id",
                table: "identity_user_claims",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_identity_user_logins_user_id",
                table: "identity_user_logins",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_identity_user_roles_role_id",
                table: "identity_user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "email_index",
                table: "identity_users",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "user_name_index",
                table: "identity_users",
                column: "normalized_user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_juror_mandate_court_id",
                table: "juror_mandate",
                column: "court_id");

            migrationBuilder.CreateIndex(
                name: "ix_juror_mandate_juror_id",
                table: "juror_mandate",
                column: "juror_id");

            migrationBuilder.CreateIndex(
                name: "ix_juror_mandate_mandate_type_id",
                table: "juror_mandate",
                column: "mandate_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_juror_mandate_register_court_id",
                table: "juror_mandate",
                column: "register_court_id");

            migrationBuilder.CreateIndex(
                name: "ix_juror_speciality_juror_id",
                table: "juror_speciality",
                column: "juror_id");

            migrationBuilder.CreateIndex(
                name: "ix_juror_speciality_speciality_id",
                table: "juror_speciality",
                column: "speciality_id");

            migrationBuilder.CreateIndex(
                name: "ix_jurors_address_city_id",
                table: "jurors",
                column: "address_city_id");

            migrationBuilder.CreateIndex(
                name: "ix_jurors_education_id",
                table: "jurors",
                column: "education_id");

            migrationBuilder.CreateIndex(
                name: "ix_jurors_education_rang_id",
                table: "jurors",
                column: "education_rang_id");

            migrationBuilder.CreateIndex(
                name: "ix_mongo_file_file_type_id",
                table: "mongo_file",
                column: "file_type_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "common_court_ekatte");

            migrationBuilder.DropTable(
                name: "identity_role_claims");

            migrationBuilder.DropTable(
                name: "identity_user_claims");

            migrationBuilder.DropTable(
                name: "identity_user_logins");

            migrationBuilder.DropTable(
                name: "identity_user_roles");

            migrationBuilder.DropTable(
                name: "identity_user_tokens");

            migrationBuilder.DropTable(
                name: "juror_mandate");

            migrationBuilder.DropTable(
                name: "juror_speciality");

            migrationBuilder.DropTable(
                name: "mongo_file");

            migrationBuilder.DropTable(
                name: "identity_roles");

            migrationBuilder.DropTable(
                name: "identity_users");

            migrationBuilder.DropTable(
                name: "common_court");

            migrationBuilder.DropTable(
                name: "nom_mandate_type");

            migrationBuilder.DropTable(
                name: "jurors");

            migrationBuilder.DropTable(
                name: "nom_speciality");

            migrationBuilder.DropTable(
                name: "nom_file_type");

            migrationBuilder.DropTable(
                name: "nom_apeal_region");

            migrationBuilder.DropTable(
                name: "nom_court_type");

            migrationBuilder.DropTable(
                name: "ek_ekatte");

            migrationBuilder.DropTable(
                name: "nom_education");

            migrationBuilder.DropTable(
                name: "nom_education_rang");

            migrationBuilder.DropTable(
                name: "ek_munincipality");

            migrationBuilder.DropTable(
                name: "ek_district");

            migrationBuilder.DropTable(
                name: "ek_country");
        }
    }
}
