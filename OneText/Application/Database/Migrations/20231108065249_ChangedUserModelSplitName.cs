using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneText.Application.Database.Migrations
{
    /// <inheritdoc />
    public partial class ChangedUserModelSplitName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "users",
                newName: "last_name");

            migrationBuilder.AddColumn<string>(
                name: "first_name",
                table: "users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "first_name",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "users",
                newName: "name");
        }
    }
}
