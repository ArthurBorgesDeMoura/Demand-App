using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IDemandApp.Migrations
{
    /// <inheritdoc />
    public partial class RefactorCategoryProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Request",
                table: "Categories",
                newName: "Active");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Active",
                table: "Categories",
                newName: "Request");
        }
    }
}
