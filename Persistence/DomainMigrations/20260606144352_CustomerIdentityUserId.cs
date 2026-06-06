using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopABMATIC.Data.Persistence.DomainMigrations
{
    /// <inheritdoc />
    public partial class CustomerIdentityUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                schema: "Customers",
                table: "Customers",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_IdentityUserId",
                schema: "Customers",
                table: "Customers",
                column: "IdentityUserId",
                unique: true,
                filter: "[IdentityUserId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customers_IdentityUserId",
                schema: "Customers",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                schema: "Customers",
                table: "Customers");
        }
    }
}
