using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class renametable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleUser_T_Roles_RolesId",
                table: "RoleUser");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleUser_T_Users_UsersId",
                table: "RoleUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleUser",
                table: "RoleUser");

            migrationBuilder.RenameTable(
                name: "RoleUser",
                newName: "T_UserRole");

            migrationBuilder.RenameIndex(
                name: "IX_RoleUser_UsersId",
                table: "T_UserRole",
                newName: "IX_T_UserRole_UsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_T_UserRole",
                table: "T_UserRole",
                columns: new[] { "RolesId", "UsersId" });

            migrationBuilder.CreateTable(
                name: "T_RolePermissions",
                columns: table => new
                {
                    PermissionsId = table.Column<long>(type: "bigint", nullable: false),
                    RolesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_RolePermissions", x => new { x.PermissionsId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_T_RolePermissions_T_Permissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "T_Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_RolePermissions_T_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "T_Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_RolePermissions_RolesId",
                table: "T_RolePermissions",
                column: "RolesId");

            migrationBuilder.AddForeignKey(
                name: "FK_T_UserRole_T_Roles_RolesId",
                table: "T_UserRole",
                column: "RolesId",
                principalTable: "T_Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_UserRole_T_Users_UsersId",
                table: "T_UserRole",
                column: "UsersId",
                principalTable: "T_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_UserRole_T_Roles_RolesId",
                table: "T_UserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_T_UserRole_T_Users_UsersId",
                table: "T_UserRole");

            migrationBuilder.DropTable(
                name: "T_RolePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_T_UserRole",
                table: "T_UserRole");

            migrationBuilder.RenameTable(
                name: "T_UserRole",
                newName: "RoleUser");

            migrationBuilder.RenameIndex(
                name: "IX_T_UserRole_UsersId",
                table: "RoleUser",
                newName: "IX_RoleUser_UsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleUser",
                table: "RoleUser",
                columns: new[] { "RolesId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUser_T_Roles_RolesId",
                table: "RoleUser",
                column: "RolesId",
                principalTable: "T_Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUser_T_Users_UsersId",
                table: "RoleUser",
                column: "UsersId",
                principalTable: "T_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
