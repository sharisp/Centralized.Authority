using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class menu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_Menus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentID = table.Column<long>(type: "bigint", nullable: false),
                    Component = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsShow = table.Column<bool>(type: "bit", nullable: false),
                    ExternalLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SystemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeleteDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDel = table.Column<bool>(type: "bit", nullable: false),
                    Descriptions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdaterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Menus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_RoleMenus",
                columns: table => new
                {
                    MenusId = table.Column<long>(type: "bigint", nullable: false),
                    RolesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_RoleMenus", x => new { x.MenusId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_T_RoleMenus_T_Menus_MenusId",
                        column: x => x.MenusId,
                        principalTable: "T_Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_RoleMenus_T_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "T_Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_RoleMenus_RolesId",
                table: "T_RoleMenus",
                column: "RolesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_RoleMenus");

            migrationBuilder.DropTable(
                name: "T_Menus");
        }
    }
}
