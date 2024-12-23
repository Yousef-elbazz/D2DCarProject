using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DALProject.Migrations
{
    /// <inheritdoc />
    public partial class RelationBetweenCarAndOrderHeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Car",
                table: "OrdeHeaders");

            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "OrdeHeaders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrdeHeaders_CarId",
                table: "OrdeHeaders",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdeHeaders_Cars_CarId",
                table: "OrdeHeaders",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdeHeaders_Cars_CarId",
                table: "OrdeHeaders");

            migrationBuilder.DropIndex(
                name: "IX_OrdeHeaders_CarId",
                table: "OrdeHeaders");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "OrdeHeaders");

            migrationBuilder.AddColumn<string>(
                name: "Car",
                table: "OrdeHeaders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
