using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class edit_role : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descriptions",
                table: "T_Users");

            migrationBuilder.DropColumn(
                name: "Descriptions",
                table: "T_Roles");

            migrationBuilder.RenameColumn(
                name: "Descriptions",
                table: "T_Permissions",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Descriptions",
                table: "T_Menus",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "T_Permissions",
                newName: "Descriptions");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "T_Menus",
                newName: "Descriptions");

            migrationBuilder.AddColumn<string>(
                name: "Descriptions",
                table: "T_Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descriptions",
                table: "T_Roles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
