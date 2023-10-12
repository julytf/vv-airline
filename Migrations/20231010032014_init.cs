using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vv_airline.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configs",
                columns: table => new
                {
                    key = table.Column<string>(type: "char(20)", unicode: false, fixedLength: true, maxLength: 20, nullable: false),
                    value = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__configs__DFD83CAE40CE0AED", x => x.key);
                });

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__model__3213E83F64857539", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    name_en = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    full_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    full_name_en = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    code_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("provinces_pkey", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeatClasses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__seat_cla__3213E83FD5D1D927", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SeatTypes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    surcharge = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__seat_typ__3213E83F4BEA9166", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    name = table.Column<string>(type: "char(50)", unicode: false, fixedLength: true, maxLength: 50, nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__service__72E12F1A07F61963", x => x.name);
                });

            migrationBuilder.CreateTable(
                name: "Aircrafts",
                columns: table => new
                {
                    registration_number = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    model_id = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__aircraft__125DB2A2E604A408", x => x.registration_number);
                    table.ForeignKey(
                        name: "FK__aircraft__model___68487DD7",
                        column: x => x.model_id,
                        principalTable: "Models",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "SeatMaps",
                columns: table => new
                {
                    model_id = table.Column<long>(type: "bigint", nullable: false),
                    no_row = table.Column<byte>(type: "tinyint", nullable: false),
                    no_col = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__seat_map__DC39CAF467886AEF", x => x.model_id);
                    table.ForeignKey(
                        name: "FK__seat_map__model___693CA210",
                        column: x => x.model_id,
                        principalTable: "Models",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    name_en = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    full_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    full_name_en = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    code_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    province_code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("districts_pkey", x => x.code);
                    table.ForeignKey(
                        name: "districts_province_code_fkey",
                        column: x => x.province_code,
                        principalTable: "Provinces",
                        principalColumn: "code");
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AisleCols",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    model_id = table.Column<long>(type: "bigint", nullable: false),
                    value = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__aisle_co__3213E83F3B06808E", x => x.id);
                    table.ForeignKey(
                        name: "FK__aisle_col__model__6B24EA82",
                        column: x => x.model_id,
                        principalTable: "SeatMaps",
                        principalColumn: "model_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExitRows",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    model_id = table.Column<long>(type: "bigint", nullable: false),
                    value = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__exit_row__3213E83F21867537", x => x.id);
                    table.ForeignKey(
                        name: "FK__exit_row__model___6A30C649",
                        column: x => x.model_id,
                        principalTable: "SeatMaps",
                        principalColumn: "model_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    model_id = table.Column<long>(type: "bigint", nullable: false),
                    row = table.Column<byte>(type: "tinyint", nullable: false),
                    col = table.Column<byte>(type: "tinyint", nullable: false),
                    seat_class_id = table.Column<long>(type: "bigint", nullable: false),
                    seat_type_id = table.Column<long>(type: "bigint", nullable: true),
                    status = table.Column<string>(type: "char(20)", unicode: false, fixedLength: true, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__seat__3213E83F7530C246", x => x.id);
                    table.ForeignKey(
                        name: "FK__seat__model_id__6C190EBB",
                        column: x => x.model_id,
                        principalTable: "SeatMaps",
                        principalColumn: "model_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__seat__seat_class__6D0D32F4",
                        column: x => x.seat_class_id,
                        principalTable: "SeatClasses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__seat__seat_type___6E01572D",
                        column: x => x.seat_type_id,
                        principalTable: "SeatTypes",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Wards",
                columns: table => new
                {
                    code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    name_en = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    full_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    full_name_en = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    code_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    district_code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("wards_pkey", x => x.code);
                    table.ForeignKey(
                        name: "wards_district_code_fkey",
                        column: x => x.district_code,
                        principalTable: "Districts",
                        principalColumn: "code");
                });

            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    longtitude = table.Column<double>(type: "float", nullable: true),
                    latitude = table.Column<double>(type: "float", nullable: true),
                    province_code = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    district_code = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    ward_code = table.Column<string>(type: "nvarchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__airport__3213E83FBD0BFB0E", x => x.id);
                    table.ForeignKey(
                        name: "FK__airport__distric__787EE5A0",
                        column: x => x.district_code,
                        principalTable: "Districts",
                        principalColumn: "code");
                    table.ForeignKey(
                        name: "FK__airport__province_id__778AC167",
                        column: x => x.province_code,
                        principalTable: "Provinces",
                        principalColumn: "code");
                    table.ForeignKey(
                        name: "FK__airport__ward_id__797309D9",
                        column: x => x.ward_code,
                        principalTable: "Wards",
                        principalColumn: "code");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    first_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "date", nullable: true),
                    gender = table.Column<bool>(type: "bit", nullable: true),
                    id_number = table.Column<string>(type: "char(12)", unicode: false, fixedLength: true, maxLength: 12, nullable: true),
                    province_id = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    district_id = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    ward_id = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    address = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    address2 = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__user__3213E83F778C8393", x => x.Id);
                    table.ForeignKey(
                        name: "FK__user__district_i__66603565",
                        column: x => x.district_id,
                        principalTable: "Districts",
                        principalColumn: "code");
                    table.ForeignKey(
                        name: "FK__user__province_id__656C112C",
                        column: x => x.province_id,
                        principalTable: "Provinces",
                        principalColumn: "code");
                    table.ForeignKey(
                        name: "FK__user__ward_id__6754599E",
                        column: x => x.ward_id,
                        principalTable: "Wards",
                        principalColumn: "code");
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    departure_airport = table.Column<long>(type: "bigint", nullable: false),
                    destination_airport = table.Column<long>(type: "bigint", nullable: false),
                    distance = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__route__3213E83F3EC1E94B", x => x.id);
                    table.ForeignKey(
                        name: "FK__route__departure__6EF57B66",
                        column: x => x.departure_airport,
                        principalTable: "Airports",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__route__destinati__6FE99F9F",
                        column: x => x.destination_airport,
                        principalTable: "Airports",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    adults = table.Column<byte>(type: "tinyint", nullable: false),
                    children = table.Column<byte>(type: "tinyint", nullable: true),
                    is_roundtrip = table.Column<bool>(type: "bit", nullable: true),
                    total_price = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__booking__3213E83F278586E2", x => x.id);
                    table.ForeignKey(
                        name: "FK__booking__user_id__7D439ABD",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    aircraft_registration_number = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    departure_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    arrival_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    route_id = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__flight__3213E83F67464E02", x => x.id);
                    table.ForeignKey(
                        name: "FK__flight__aircraft__72C60C4A",
                        column: x => x.aircraft_registration_number,
                        principalTable: "Aircrafts",
                        principalColumn: "registration_number",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__flight__route_id__73BA3083",
                        column: x => x.route_id,
                        principalTable: "Routes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    route_id = table.Column<long>(type: "bigint", nullable: false),
                    seat_class_id = table.Column<long>(type: "bigint", nullable: false),
                    value = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__price__3213E83FA4A40A38", x => x.id);
                    table.ForeignKey(
                        name: "FK__price__route_id__71D1E811",
                        column: x => x.route_id,
                        principalTable: "Routes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__price__seat_clas__70DDC3D8",
                        column: x => x.seat_class_id,
                        principalTable: "SeatClasses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    has_transit = table.Column<bool>(type: "bit", nullable: true),
                    route_id = table.Column<long>(type: "bigint", nullable: false),
                    departure_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    arrival_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    distance = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__schedule__3213E83F003A11A8", x => x.id);
                    table.ForeignKey(
                        name: "FK__schedule__route___74AE54BC",
                        column: x => x.route_id,
                        principalTable: "Routes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "booking_service",
                columns: table => new
                {
                    booking_id = table.Column<long>(type: "bigint", nullable: false),
                    service_name = table.Column<string>(type: "char(50)", unicode: false, fixedLength: true, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__booking___C94B4842E286A9C8", x => new { x.booking_id, x.service_name });
                    table.ForeignKey(
                        name: "FK__booking_s__booki__01142BA1",
                        column: x => x.booking_id,
                        principalTable: "Bookings",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__booking_s__servi__02084FDA",
                        column: x => x.service_name,
                        principalTable: "Services",
                        principalColumn: "name");
                });

            migrationBuilder.CreateTable(
                name: "Passengers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    booking_id = table.Column<long>(type: "bigint", nullable: false),
                    index = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    phone_number = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    first_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "date", nullable: true),
                    gender = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__passenge__3213E83FC1DFDF90", x => x.id);
                    table.ForeignKey(
                        name: "FK__passenger__booki__00200768",
                        column: x => x.booking_id,
                        principalTable: "Bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlightSchedules",
                columns: table => new
                {
                    flight_id = table.Column<long>(type: "bigint", nullable: false),
                    schedule_id = table.Column<long>(type: "bigint", nullable: false),
                    index = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__flight_s__0F36FFC35870B195", x => new { x.flight_id, x.schedule_id });
                    table.ForeignKey(
                        name: "FK__flight_sc__fligh__75A278F5",
                        column: x => x.flight_id,
                        principalTable: "Flights",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__flight_sc__sched__76969D2E",
                        column: x => x.schedule_id,
                        principalTable: "Schedules",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ScheduleBookings",
                columns: table => new
                {
                    schedule_id = table.Column<long>(type: "bigint", nullable: false),
                    booking_id = table.Column<long>(type: "bigint", nullable: false),
                    index = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__schedule__C1B4B034C4B7CC2E", x => new { x.schedule_id, x.booking_id });
                    table.ForeignKey(
                        name: "FK__schedule___booki__7F2BE32F",
                        column: x => x.booking_id,
                        principalTable: "Bookings",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__schedule___sched__7E37BEF6",
                        column: x => x.schedule_id,
                        principalTable: "Schedules",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    flight_id = table.Column<long>(type: "bigint", nullable: false),
                    seat_id = table.Column<long>(type: "bigint", nullable: false),
                    passenger_id = table.Column<long>(type: "bigint", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ticket__3213E83FB53CA3F1", x => x.id);
                    table.ForeignKey(
                        name: "FK__ticket__flight_i__7A672E12",
                        column: x => x.flight_id,
                        principalTable: "Flights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__ticket__passenge__7C4F7684",
                        column: x => x.passenger_id,
                        principalTable: "Passengers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__ticket__seat_id__7B5B524B",
                        column: x => x.seat_id,
                        principalTable: "Seats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aircrafts_model_id",
                table: "Aircrafts",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "IX_Airports_district_code",
                table: "Airports",
                column: "district_code");

            migrationBuilder.CreateIndex(
                name: "IX_Airports_province_code",
                table: "Airports",
                column: "province_code");

            migrationBuilder.CreateIndex(
                name: "IX_Airports_ward_code",
                table: "Airports",
                column: "ward_code");

            migrationBuilder.CreateIndex(
                name: "aisle_col_index_1",
                table: "AisleCols",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "IX_booking_service_service_name",
                table: "booking_service",
                column: "service_name");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_user_id",
                table: "Bookings",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_districts_province",
                table: "Districts",
                column: "province_code");

            migrationBuilder.CreateIndex(
                name: "exit_row_index_0",
                table: "ExitRows",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_aircraft_registration_number",
                table: "Flights",
                column: "aircraft_registration_number");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_route_id",
                table: "Flights",
                column: "route_id");

            migrationBuilder.CreateIndex(
                name: "IX_Flight_Schedule_schedule_id",
                table: "FlightSchedules",
                column: "schedule_id");

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_booking_id",
                table: "Passengers",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_route_id",
                table: "Prices",
                column: "route_id");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_seat_class_id",
                table: "Prices",
                column: "seat_class_id");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_departure_airport",
                table: "Routes",
                column: "departure_airport");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_destination_airport",
                table: "Routes",
                column: "destination_airport");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_Booking_booking_id",
                table: "ScheduleBookings",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_route_id",
                table: "Schedules",
                column: "route_id");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_model_id",
                table: "Seats",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_seat_class_id",
                table: "Seats",
                column: "seat_class_id");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_seat_type_id",
                table: "Seats",
                column: "seat_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_flight_id",
                table: "Tickets",
                column: "flight_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_passenger_id",
                table: "Tickets",
                column: "passenger_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_seat_id",
                table: "Tickets",
                column: "seat_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_district_code",
                table: "Users",
                column: "district_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_province_code",
                table: "Users",
                column: "province_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ward_code",
                table: "Users",
                column: "ward_id");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex1",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "([NormalizedUserName] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "idx_wards_district",
                table: "Wards",
                column: "district_code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AisleCols");

            migrationBuilder.DropTable(
                name: "booking_service");

            migrationBuilder.DropTable(
                name: "Configs");

            migrationBuilder.DropTable(
                name: "ExitRows");

            migrationBuilder.DropTable(
                name: "FlightSchedules");

            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "ScheduleBookings");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Passengers");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Aircrafts");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "SeatMaps");

            migrationBuilder.DropTable(
                name: "SeatClasses");

            migrationBuilder.DropTable(
                name: "SeatTypes");

            migrationBuilder.DropTable(
                name: "Airports");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropTable(
                name: "Wards");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "Provinces");
        }
    }
}
