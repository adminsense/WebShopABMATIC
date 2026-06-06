using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopABMATIC.Data.Persistence.DomainMigrations
{
    /// <inheritdoc />
    public partial class OrderAdvancePaymentMollieColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MollieCheckoutUrl",
                schema: "Projects",
                table: "OrderAdvancePayments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MolliePaidAt",
                schema: "Projects",
                table: "OrderAdvancePayments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MolliePaymentId",
                schema: "Projects",
                table: "OrderAdvancePayments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MolliePaymentStatus",
                schema: "Projects",
                table: "OrderAdvancePayments",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MollieCheckoutUrl",
                schema: "Projects",
                table: "OrderAdvancePayments");

            migrationBuilder.DropColumn(
                name: "MolliePaidAt",
                schema: "Projects",
                table: "OrderAdvancePayments");

            migrationBuilder.DropColumn(
                name: "MolliePaymentId",
                schema: "Projects",
                table: "OrderAdvancePayments");

            migrationBuilder.DropColumn(
                name: "MolliePaymentStatus",
                schema: "Projects",
                table: "OrderAdvancePayments");
        }
    }
}
