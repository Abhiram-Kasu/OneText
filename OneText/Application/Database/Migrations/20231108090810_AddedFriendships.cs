using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneText.Application.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedFriendships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "friendships",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user1id = table.Column<int>(type: "INTEGER", nullable: false),
                    user2id = table.Column<int>(type: "INTEGER", nullable: false),
                    blocked = table.Column<bool>(type: "INTEGER", nullable: false),
                    user_id = table.Column<int>(type: "INTEGER", nullable: true),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_friendships", x => x.id);
                    table.ForeignKey(
                        name: "fk_friendships_users_user1id",
                        column: x => x.user1id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_friendships_users_user2id",
                        column: x => x.user2id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_friendships_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_friendships_user_id",
                table: "friendships",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_friendships_user1id",
                table: "friendships",
                column: "user1id");

            migrationBuilder.CreateIndex(
                name: "ix_friendships_user2id",
                table: "friendships",
                column: "user2id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "friendships");
        }
    }
}
