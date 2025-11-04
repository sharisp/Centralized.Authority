using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class systemconfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_UserAccessFails_T_Users_UserId",
                table: "T_UserAccessFails");

            migrationBuilder.CreateTable(
                name: "T_Systems_Config",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    SystemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ConfigKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ConfigValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValueType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeleteDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDel = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdaterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Systems_Config", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_T_UserAccessFails_T_Users_UserId",
                table: "T_UserAccessFails",
                column: "UserId",
                principalTable: "T_Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_UserAccessFails_T_Users_UserId",
                table: "T_UserAccessFails");

            migrationBuilder.DropTable(
                name: "T_Systems_Config");

            migrationBuilder.AddForeignKey(
                name: "FK_T_UserAccessFails_T_Users_UserId",
                table: "T_UserAccessFails",
                column: "UserId",
                principalTable: "T_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
