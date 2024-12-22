using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DALProject.Migrations
{
    /// <inheritdoc />
    public partial class Eeedit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_shoppingCarts_Customers_CustomerId1",
                table: "shoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_shoppingCarts_CustomerId1",
                table: "shoppingCarts");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "shoppingCarts");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "shoppingCarts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "shoppingCarts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId1",
                table: "shoppingCarts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_shoppingCarts_CustomerId1",
                table: "shoppingCarts",
                column: "CustomerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_shoppingCarts_Customers_CustomerId1",
                table: "shoppingCarts",
                column: "CustomerId1",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
