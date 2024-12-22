using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DALProject.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderقق : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "Customers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AppUserId1",
                table: "Customers",
                column: "AppUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_AspNetUsers_AppUserId1",
                table: "Customers",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_AspNetUsers_AppUserId1",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_AppUserId1",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "Customers");
        }
    }
}
