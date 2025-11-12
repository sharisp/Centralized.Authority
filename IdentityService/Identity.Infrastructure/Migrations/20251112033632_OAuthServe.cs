using IdGen;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OAuthServe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {// 1. 删除旧主键（在原表名上）
            migrationBuilder.DropPrimaryKey(
                name: "PK_OAuthAccounts",
                table: "OAuthAccounts");

            // 2. 重命名表为目标名称
            migrationBuilder.RenameTable(
                name: "OAuthAccounts",
                newName: "T_OAuthAccounts");

            // 3. 修改 Description 列定义
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "T_OAuthAccounts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            // 4. 删除旧的 Id 列（原先可能带有 IDENTITY 注释）
            migrationBuilder.DropColumn(
                name: "Id",
                table: "T_OAuthAccounts");

            // 5. 添加新的 Id 列（非 IDENTITY）
            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "T_OAuthAccounts",
                type: "bigint",
                nullable: false);

            // 6. 重新创建主键
            migrationBuilder.AddPrimaryKey(
                name: "PK_T_OAuthAccounts",
                table: "T_OAuthAccounts",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 逆操作：删除当前主键（在新表名上）
            migrationBuilder.DropPrimaryKey(
                name: "PK_T_OAuthAccounts",
                table: "T_OAuthAccounts");

            // 删除当前 Id 列（非 identity）
            migrationBuilder.DropColumn(
                name: "Id",
                table: "T_OAuthAccounts");

            // 把 Description 列还原为原始定义（nvarchar(max) 可空）
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "T_OAuthAccounts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldDefaultValue: "");

            // 将表名改回原名
            migrationBuilder.RenameTable(
                name: "T_OAuthAccounts",
                newName: "OAuthAccounts");

            // 重新创建 Id 列为带 IDENTITY 的版本（原始结构）
            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "OAuthAccounts",
                type: "bigint",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            // 恢复原主键
            migrationBuilder.AddPrimaryKey(
                name: "PK_OAuthAccounts",
                table: "OAuthAccounts",
                column: "Id");
        }
    }
}
