using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MothraForum.Migrations
{
    /// <inheritdoc />
    public partial class StupidFuckingDatabaseRecreateIHateThis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Votes_DiscussionId",
                table: "Votes");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Votes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Discussions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Comments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Votes_ApplicationUserId",
                table: "Votes",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_DiscussionId_ApplicationUserId",
                table: "Votes",
                columns: new[] { "DiscussionId", "ApplicationUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discussions_ApplicationUserId",
                table: "Discussions",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ApplicationUserId",
                table: "Comments",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_ApplicationUserId",
                table: "Comments",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Discussions_AspNetUsers_ApplicationUserId",
                table: "Discussions",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_AspNetUsers_ApplicationUserId",
                table: "Votes",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_ApplicationUserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Discussions_AspNetUsers_ApplicationUserId",
                table: "Discussions");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_AspNetUsers_ApplicationUserId",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Votes_ApplicationUserId",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Votes_DiscussionId_ApplicationUserId",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Discussions_ApplicationUserId",
                table: "Discussions");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ApplicationUserId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Discussions");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Comments");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_DiscussionId",
                table: "Votes",
                column: "DiscussionId");
        }
    }
}
