using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DALProject.Migrations
{
    /// <inheritdoc />
    public partial class Car2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Car",
                table: "OrdeHeaders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Car",
                table: "OrdeHeaders");
        }
    }
}
