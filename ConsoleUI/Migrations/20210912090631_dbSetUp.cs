using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsoleUI.Migrations
{
    public partial class dbSetUp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Open = table.Column<decimal>(type: "TEXT", nullable: false),
                    Debit = table.Column<decimal>(type: "TEXT", nullable: false),
                    Credit = table.Column<decimal>(type: "TEXT", nullable: false),
                    Transfer = table.Column<decimal>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    Default = table.Column<bool>(type: "INTEGER", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    Limit = table.Column<decimal>(type: "TEXT", nullable: true),
                    Symbol = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<decimal>(type: "TEXT", nullable: true),
                    PriceDate = table.Column<DateOnly>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterKeys",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Prefix = table.Column<string>(type: "TEXT", nullable: true),
                    TwoFactor = table.Column<bool>(type: "INTEGER", nullable: false),
                    NextTerm = table.Column<int>(type: "INTEGER", nullable: false),
                    NextKey = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transacts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Payee = table.Column<string>(type: "TEXT", nullable: true),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    AccountId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transacts_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    TransactId = table.Column<string>(type: "TEXT", nullable: true),
                    Order = table.Column<string>(type: "TEXT", nullable: true),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    ForexCurrency = table.Column<string>(type: "TEXT", nullable: true),
                    ForexAmount = table.Column<decimal>(type: "TEXT", nullable: true),
                    Category = table.Column<string>(type: "TEXT", nullable: true),
                    Unit = table.Column<decimal>(type: "TEXT", nullable: true),
                    Price = table.Column<decimal>(type: "TEXT", nullable: true),
                    PriceDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    TradingId = table.Column<string>(type: "TEXT", nullable: true),
                    TradingAccountId = table.Column<string>(type: "TEXT", nullable: true),
                    TransferId = table.Column<string>(type: "TEXT", nullable: true),
                    LinkId = table.Column<string>(type: "TEXT", nullable: true),
                    LinkOrder = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactDetails_Accounts_TradingAccountId",
                        column: x => x.TradingAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactDetails_Accounts_TransferId",
                        column: x => x.TransferId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactDetails_Transacts_TransactId",
                        column: x => x.TransactId,
                        principalTable: "Transacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactDetails_TradingAccountId",
                table: "TransactDetails",
                column: "TradingAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactDetails_TransactId",
                table: "TransactDetails",
                column: "TransactId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactDetails_TransferId",
                table: "TransactDetails",
                column: "TransferId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacts_AccountId",
                table: "Transacts",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterKeys");

            migrationBuilder.DropTable(
                name: "TransactDetails");

            migrationBuilder.DropTable(
                name: "Transacts");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
