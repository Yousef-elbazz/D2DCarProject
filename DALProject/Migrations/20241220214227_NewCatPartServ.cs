using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DALProject.Migrations
{
    /// <inheritdoc />
    public partial class NewCatPartServ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Tickets_TicketId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_TicketId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "CartItems");

            migrationBuilder.AddColumn<int>(
                name: "PartServiceId",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "prodServCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prodServCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "partServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Img = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProdServCategoryId = table.Column<int>(type: "int", nullable: false),
                    CarId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_partServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_partServices_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_partServices_prodServCategories_ProdServCategoryId",
                        column: x => x.ProdServCategoryId,
                        principalTable: "prodServCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PartServiceId",
                table: "Appointments",
                column: "PartServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_partServices_CarId",
                table: "partServices",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_partServices_ProdServCategoryId",
                table: "partServices",
                column: "ProdServCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_partServices_PartServiceId",
                table: "Appointments",
                column: "PartServiceId",
                principalTable: "partServices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_partServices_PartServiceId",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "partServices");

            migrationBuilder.DropTable(
                name: "prodServCategories");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_PartServiceId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "PartServiceId",
                table: "Appointments");

            migrationBuilder.AddColumn<int>(
                name: "TicketId",
                table: "CartItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_TicketId",
                table: "CartItems",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Tickets_TicketId",
                table: "CartItems",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id");
        }
    }
}
