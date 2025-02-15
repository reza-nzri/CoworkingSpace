using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoworkingSpaceAPI.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    address_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    street = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    house_number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    postal_code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    city = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    state = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Address__CAA247C8E6D97059", x => x.address_id);
                });

            migrationBuilder.CreateTable(
                name: "AddressType",
                columns: table => new
                {
                    address_type_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    address_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AddressT__5ABF11E50EED6709", x => x.address_type_id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Suffix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nickname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecoveryEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlternaiveEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecoveryPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Birthday = table.Column<DateOnly>(type: "date", nullable: true),
                    ProfilePicturePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppLanguage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Linkedin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Twitter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Github = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Youtube = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tiktok = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Snapchat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    company_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    industry = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    registration_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    tax_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    website = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    contact_email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    contact_phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    founded_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Company__3E267235DB003AF5", x => x.company_id);
                });

            migrationBuilder.CreateTable(
                name: "Label",
                columns: table => new
                {
                    label_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    label_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    color_code = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Label__E44FFA585B07B173", x => x.label_id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
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
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
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
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAddress",
                columns: table => new
                {
                    user_address_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    address_id = table.Column<int>(type: "int", nullable: true),
                    address_type_id = table.Column<int>(type: "int", nullable: true),
                    is_default = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserAddr__FEC0352ECEF2EE95", x => x.user_address_id);
                    table.ForeignKey(
                        name: "FK__UserAddre__addre__5165187F",
                        column: x => x.address_id,
                        principalTable: "Address",
                        principalColumn: "address_id");
                    table.ForeignKey(
                        name: "FK__UserAddre__addre__52593CB8",
                        column: x => x.address_type_id,
                        principalTable: "AddressType",
                        principalColumn: "address_type_id");
                    table.ForeignKey(
                        name: "FK__UserAddre__user___5070F446",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserSession",
                columns: table => new
                {
                    session_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    login_time = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    logout_time = table.Column<DateTime>(type: "datetime", nullable: true),
                    last_ip = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    device = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    browser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    operating_system = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    login_attempts = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    failed_login_attempts = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    session_duration = table.Column<int>(type: "int", nullable: true, computedColumnSql: "(datediff(minute,[login_time],[logout_time]))", stored: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserSess__69B13FDCAFEF904D", x => x.session_id);
                    table.ForeignKey(
                        name: "FK__UserSessi__user___59063A47",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CompanyAddress",
                columns: table => new
                {
                    company_address_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    company_id = table.Column<int>(type: "int", nullable: true),
                    address_id = table.Column<int>(type: "int", nullable: true),
                    address_type_id = table.Column<int>(type: "int", nullable: true),
                    is_default = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CompanyA__5650FC57FCFDFEB6", x => x.company_address_id);
                    table.ForeignKey(
                        name: "FK__CompanyAd__addre__693CA210",
                        column: x => x.address_id,
                        principalTable: "Address",
                        principalColumn: "address_id");
                    table.ForeignKey(
                        name: "FK__CompanyAd__addre__6A30C649",
                        column: x => x.address_type_id,
                        principalTable: "AddressType",
                        principalColumn: "address_type_id");
                    table.ForeignKey(
                        name: "FK__CompanyAd__compa__68487DD7",
                        column: x => x.company_id,
                        principalTable: "Company",
                        principalColumn: "company_id");
                });

            migrationBuilder.CreateTable(
                name: "CompanyCEO",
                columns: table => new
                {
                    company_id = table.Column<int>(type: "int", nullable: false),
                    ceo_user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CompanyC__D8C45EEBF1ACE5BA", x => new { x.company_id, x.ceo_user_id, x.start_date });
                    table.ForeignKey(
                        name: "FK__CompanyCE__ceo_u__6477ECF3",
                        column: x => x.ceo_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__CompanyCE__compa__6383C8BA",
                        column: x => x.company_id,
                        principalTable: "Company",
                        principalColumn: "company_id");
                });

            migrationBuilder.CreateTable(
                name: "CompanyEmployee",
                columns: table => new
                {
                    company_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CompanyE__E51B67242C37EA90", x => new { x.company_id, x.user_id, x.start_date });
                    table.ForeignKey(
                        name: "FK__CompanyEm__compa__5FB337D6",
                        column: x => x.company_id,
                        principalTable: "Company",
                        principalColumn: "company_id");
                    table.ForeignKey(
                        name: "FK__CompanyEm__user___60A75C0F",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LabelAssignment",
                columns: table => new
                {
                    label_assignment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    label_id = table.Column<int>(type: "int", nullable: false),
                    entity_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    entity_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LabelAss__F27DEC24A29D32BB", x => x.label_assignment_id);
                    table.ForeignKey(
                        name: "FK__LabelAssi__label__3F466844",
                        column: x => x.label_id,
                        principalTable: "Label",
                        principalColumn: "label_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    room_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    room_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    room_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: true, defaultValue: 0m),
                    currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true, defaultValue: "EUR"),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    company_address_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Room__19675A8AFBEE5EFB", x => x.room_id);
                    table.ForeignKey(
                        name: "FK__Room__company_ad__70DDC3D8",
                        column: x => x.company_address_id,
                        principalTable: "CompanyAddress",
                        principalColumn: "company_address_id");
                });

            migrationBuilder.CreateTable(
                name: "Desk",
                columns: table => new
                {
                    desk_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    room_id = table.Column<int>(type: "int", nullable: false),
                    desk_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: true, defaultValue: 0m),
                    currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true, defaultValue: "EUR"),
                    is_available = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Desk__8AF10D0469E707D1", x => x.desk_id);
                    table.ForeignKey(
                        name: "FK__Desk__room_id__73BA3083",
                        column: x => x.room_id,
                        principalTable: "Room",
                        principalColumn: "room_id");
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    booking_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    desk_id = table.Column<int>(type: "int", nullable: false),
                    start_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    end_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    total_cost = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    is_cancelled = table.Column<bool>(type: "bit", nullable: false),
                    cancellation_reason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    is_checked_in = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Booking__5DE3A5B11355DBAA", x => x.booking_id);
                    table.ForeignKey(
                        name: "FK__Booking__desk_id__7A672E12",
                        column: x => x.desk_id,
                        principalTable: "Desk",
                        principalColumn: "desk_id");
                    table.ForeignKey(
                        name: "FK__Booking__user_id__797309D9",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__AddressT__071A958776CC882F",
                table: "AddressType",
                column: "address_type",
                unique: true,
                filter: "[address_type] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_desk_id",
                table: "Booking",
                column: "desk_id");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_user_id",
                table: "Booking",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Company__125DB2A3918EFD1D",
                table: "Company",
                column: "registration_number",
                unique: true,
                filter: "[registration_number] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__Company__129B8671395BA472",
                table: "Company",
                column: "tax_id",
                unique: true,
                filter: "[tax_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAddress_address_id",
                table: "CompanyAddress",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAddress_address_type_id",
                table: "CompanyAddress",
                column: "address_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAddress_company_id",
                table: "CompanyAddress",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyCEO_ceo_user_id",
                table: "CompanyCEO",
                column: "ceo_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyEmployee_user_id",
                table: "CompanyEmployee",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Desk_room_id",
                table: "Desk",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Label__A74BEA65FF2F052D",
                table: "Label",
                column: "label_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LabelAssignment_label_id",
                table: "LabelAssignment",
                column: "label_id");

            migrationBuilder.CreateIndex(
                name: "IX_Room_company_address_id",
                table: "Room",
                column: "company_address_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddress_address_id",
                table: "UserAddress",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddress_address_type_id",
                table: "UserAddress",
                column: "address_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddress_user_id",
                table: "UserAddress",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserSession_user_id",
                table: "UserSession",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "CompanyCEO");

            migrationBuilder.DropTable(
                name: "CompanyEmployee");

            migrationBuilder.DropTable(
                name: "LabelAssignment");

            migrationBuilder.DropTable(
                name: "UserAddress");

            migrationBuilder.DropTable(
                name: "UserSession");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Desk");

            migrationBuilder.DropTable(
                name: "Label");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "CompanyAddress");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "AddressType");

            migrationBuilder.DropTable(
                name: "Company");
        }
    }
}
