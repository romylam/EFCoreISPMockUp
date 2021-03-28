using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsoleUI.Migrations
{
    public partial class SetUp1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Houses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    IsForSale = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsForRent = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Houses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Residents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    LastName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Residents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HouseResident",
                columns: table => new
                {
                    HousesId = table.Column<int>(type: "INTEGER", nullable: false),
                    ResidentsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseResident", x => new { x.HousesId, x.ResidentsId });
                    table.ForeignKey(
                        name: "FK_HouseResident_Houses_HousesId",
                        column: x => x.HousesId,
                        principalTable: "Houses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HouseResident_Residents_ResidentsId",
                        column: x => x.ResidentsId,
                        principalTable: "Residents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BuyOrRent = table.Column<string>(type: "TEXT", nullable: true),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    ResidentId = table.Column<int>(type: "INTEGER", nullable: false),
                    HouseId = table.Column<int>(type: "INTEGER", nullable: false),
                    ActualTransact = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transacts_Houses_HouseId",
                        column: x => x.HouseId,
                        principalTable: "Houses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transacts_Residents_ResidentId",
                        column: x => x.ResidentId,
                        principalTable: "Residents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HouseResident_ResidentsId",
                table: "HouseResident",
                column: "ResidentsId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacts_HouseId",
                table: "Transacts",
                column: "HouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacts_ResidentId",
                table: "Transacts",
                column: "ResidentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HouseResident");

            migrationBuilder.DropTable(
                name: "Transacts");

            migrationBuilder.DropTable(
                name: "Houses");

            migrationBuilder.DropTable(
                name: "Residents");
        }
    }
}
