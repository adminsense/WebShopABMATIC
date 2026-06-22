using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopABMATIC.Infrastructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class StockLowAlerts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockLowAlerts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductStockLocationId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    StockLocationId = table.Column<int>(type: "int", nullable: false),
                    StockLocationName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    MinQuantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockLowAlerts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockLowAlerts_CreatedAt",
                table: "StockLowAlerts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_StockLowAlerts_IsRead_CreatedAt",
                table: "StockLowAlerts",
                columns: new[] { "IsRead", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_StockLowAlerts_ProductStockLocationId",
                table: "StockLowAlerts",
                column: "ProductStockLocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockLowAlerts");
        }
    }
}
