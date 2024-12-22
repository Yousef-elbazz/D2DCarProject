using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DALProject.Migrations
{
    /// <inheritdoc />
    public partial class Edit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_partServices_PartServiceId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_partServices_Cars_CarId",
                table: "partServices");

            migrationBuilder.DropIndex(
                name: "IX_partServices_CarId",
                table: "partServices");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_PartServiceId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "partServices");

            migrationBuilder.DropColumn(
                name: "PartServiceId",
                table: "Appointments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "partServices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PartServiceId",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_partServices_CarId",
                table: "partServices",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PartServiceId",
                table: "Appointments",
                column: "PartServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_partServices_PartServiceId",
                table: "Appointments",
                column: "PartServiceId",
                principalTable: "partServices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_partServices_Cars_CarId",
                table: "partServices",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
