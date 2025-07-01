using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class menu_permission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_MenuPermissions",
                columns: table => new
                {
                    MenusId = table.Column<long>(type: "bigint", nullable: false),
                    PermissionsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MenuPermissions", x => new { x.MenusId, x.PermissionsId });
                    table.ForeignKey(
                        name: "FK_T_MenuPermissions_T_Menus_MenusId",
                        column: x => x.MenusId,
                        principalTable: "T_Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_MenuPermissions_T_Permissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "T_Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_MenuPermissions_PermissionsId",
                table: "T_MenuPermissions",
                column: "PermissionsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_MenuPermissions");
        }
    }
}
