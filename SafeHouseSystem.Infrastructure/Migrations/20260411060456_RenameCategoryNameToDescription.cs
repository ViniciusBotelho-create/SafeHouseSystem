using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafeHouseSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameCategoryNameToDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Categories",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Categories",
                newName: "Name");
        }
    }
}
