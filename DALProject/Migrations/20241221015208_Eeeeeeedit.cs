using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DALProject.Migrations
{
    /// <inheritdoc />
    public partial class Eeeeeeedit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_shoppingCarts_AspNetUsers_AppUserId",
                table: "shoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_shoppingCarts_AppUserId",
                table: "shoppingCarts");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "shoppingCarts");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "shoppingCarts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_shoppingCarts_CustomerId",
                table: "shoppingCarts",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_shoppingCarts_Customers_CustomerId",
                table: "shoppingCarts",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_shoppingCarts_Customers_CustomerId",
                table: "shoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_shoppingCarts_CustomerId",
                table: "shoppingCarts");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "shoppingCarts");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "shoppingCarts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_shoppingCarts_AppUserId",
                table: "shoppingCarts",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_shoppingCarts_AspNetUsers_AppUserId",
                table: "shoppingCarts",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
