using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tool_Menagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kategoria",
                columns: table => new
                {
                    Id_Kategorii = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Opis = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Przeznaczenie = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Material_wykonania = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Kategori__E2A56928B2A8F3CE", x => x.Id_Kategorii);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Technologia",
                columns: table => new
                {
                    Id_technologi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Opis = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Data_utworzenia = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Technolo__ECE48A4CFDB3801A", x => x.Id_technologi);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
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
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
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
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "Narzedzie",
                columns: table => new
                {
                    Id_narzedzia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Kategorii = table.Column<int>(type: "int", nullable: false),
                    Nazwa = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Srednica = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Narzedzi__B019BBBC628096CC", x => x.Id_narzedzia);
                    table.ForeignKey(
                        name: "rST_NarzedzieKategoria",
                        column: x => x.Id_Kategorii,
                        principalTable: "Kategoria",
                        principalColumn: "Id_Kategorii");
                });

            migrationBuilder.CreateTable(
                name: "Zlecenie",
                columns: table => new
                {
                    Id_zlecenia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_technologi = table.Column<int>(type: "int", nullable: false),
                    Aktywne = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Zlecenie__4FF1DFCEB8CCD08B", x => x.Id_zlecenia);
                    table.ForeignKey(
                        name: "rST_ZlecenieTechnologia",
                        column: x => x.Id_technologi,
                        principalTable: "Technologia",
                        principalColumn: "Id_technologi");
                });

            migrationBuilder.CreateTable(
                name: "Magazyn",
                columns: table => new
                {
                    Pozycja_magazynowa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_narzedzia = table.Column<int>(type: "int", nullable: false),
                    Trwalosc = table.Column<int>(type: "int", nullable: false),
                    Uzycie = table.Column<int>(type: "int", nullable: false),
                    Cykl_regeneracji = table.Column<int>(type: "int", nullable: false),
                    Wycofany = table.Column<bool>(type: "bit", nullable: false),
                    Regeneracja = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Magazyn__78B0461F242BAAD7", x => x.Pozycja_magazynowa);
                    table.ForeignKey(
                        name: "rST_MagazynNarzedzie",
                        column: x => x.Id_narzedzia,
                        principalTable: "Narzedzie",
                        principalColumn: "Id_narzedzia");
                });

            migrationBuilder.CreateTable(
                name: "Narzedzia_Technologia",
                columns: table => new
                {
                    Id_narzedzia = table.Column<int>(type: "int", nullable: false),
                    Id_technologi = table.Column<int>(type: "int", nullable: false),
                    Czas_pracy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Narzedzi__6ED7F3187D2CDCAB", x => new { x.Id_narzedzia, x.Id_technologi });
                    table.ForeignKey(
                        name: "rST_Narzedzia_TechnologiaNarzedzie",
                        column: x => x.Id_narzedzia,
                        principalTable: "Narzedzie",
                        principalColumn: "Id_narzedzia");
                    table.ForeignKey(
                        name: "rST_Narzedzia_TechnologiaTechnologia",
                        column: x => x.Id_technologi,
                        principalTable: "Technologia",
                        principalColumn: "Id_technologi");
                });

            migrationBuilder.CreateTable(
                name: "OrderTT",
                columns: table => new
                {
                    Position_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order_Id = table.Column<int>(type: "int", nullable: false),
                    Tool_Id = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTT", x => x.Position_Id);
                    table.ForeignKey(
                        name: "rST_OrderTT_Zlecenie",
                        column: x => x.Order_Id,
                        principalTable: "Zlecenie",
                        principalColumn: "Id_zlecenia");
                });

            migrationBuilder.CreateTable(
                name: "Rejestracja",
                columns: table => new
                {
                    Id_pozycji = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_zlecenia = table.Column<int>(type: "int", nullable: false),
                    Sztuk = table.Column<int>(type: "int", nullable: true),
                    Data_wykonania = table.Column<DateOnly>(type: "date", nullable: true),
                    Wykonal = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Rejestra__04BB3C818C437FEF", x => x.Id_pozycji);
                    table.ForeignKey(
                        name: "rST_ZlecenieRejestracja",
                        column: x => x.Id_zlecenia,
                        principalTable: "Zlecenie",
                        principalColumn: "Id_zlecenia");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Magazyn_Id_narzedzia",
                table: "Magazyn",
                column: "Id_narzedzia");

            migrationBuilder.CreateIndex(
                name: "IX_Narzedzia_Technologia_Id_technologi",
                table: "Narzedzia_Technologia",
                column: "Id_technologi");

            migrationBuilder.CreateIndex(
                name: "IX_Narzedzie_Id_Kategorii",
                table: "Narzedzie",
                column: "Id_Kategorii");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTT_Order_Id",
                table: "OrderTT",
                column: "Order_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Rejestracja_Id_zlecenia",
                table: "Rejestracja",
                column: "Id_zlecenia");

            migrationBuilder.CreateIndex(
                name: "IX_Zlecenie_Id_technologi",
                table: "Zlecenie",
                column: "Id_technologi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Magazyn");

            migrationBuilder.DropTable(
                name: "Narzedzia_Technologia");

            migrationBuilder.DropTable(
                name: "OrderTT");

            migrationBuilder.DropTable(
                name: "Rejestracja");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Narzedzie");

            migrationBuilder.DropTable(
                name: "Zlecenie");

            migrationBuilder.DropTable(
                name: "Kategoria");

            migrationBuilder.DropTable(
                name: "Technologia");
        }
    }
}
