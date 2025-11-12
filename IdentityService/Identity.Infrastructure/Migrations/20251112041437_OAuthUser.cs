using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OAuthUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "T_OAuthAccounts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);
            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "T_Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                       name: "Description",
                       table: "T_OAuthAccounts",
                       type: "nvarchar(500)",
                       maxLength: 500,
                       nullable: false,
                       defaultValue: "",
                       oldClrType: typeof(string),
                       oldType: "nvarchar(500)",
                       oldMaxLength: 500,
                       oldNullable: true);
            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "T_Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
