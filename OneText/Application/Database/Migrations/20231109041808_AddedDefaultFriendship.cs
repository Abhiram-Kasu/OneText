using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneText.Application.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedDefaultFriendship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "friendships",
                columns: new[] { "id", "blocked", "created_at", "updated_at", "user1id", "user2id", "user_id" },
                values: new object[] { 1, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 2, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "friendships",
                keyColumn: "id",
                keyValue: 1);
        }
    }
}
