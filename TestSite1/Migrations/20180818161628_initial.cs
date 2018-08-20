using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TestSite1.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaseCurrency",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    baseData = table.Column<string>(nullable: true),
                    disclaimer = table.Column<string>(nullable: true),
                    license = table.Column<string>(nullable: true),
                    timestamp = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseCurrency", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Rates",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AED = table.Column<double>(nullable: false),
                    AFN = table.Column<double>(nullable: false),
                    ALL = table.Column<int>(nullable: false),
                    AMD = table.Column<double>(nullable: false),
                    ANG = table.Column<double>(nullable: false),
                    BaseCurrencyid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rates", x => x.id);
                    table.ForeignKey(
                        name: "FK_Rates_BaseCurrency_BaseCurrencyid",
                        column: x => x.BaseCurrencyid,
                        principalTable: "BaseCurrency",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rates_BaseCurrencyid",
                table: "Rates",
                column: "BaseCurrencyid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rates");

            migrationBuilder.DropTable(
                name: "BaseCurrency");
        }
    }
}
