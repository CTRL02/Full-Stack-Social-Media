using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace socialmedia.Migrations
{
    /// <inheritdoc />
    public partial class editedConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Impressions_AppUserId",
                table: "Impressions");

            migrationBuilder.CreateIndex(
                name: "IX_Impressions_AppUserId_CommentId",
                table: "Impressions",
                columns: new[] { "AppUserId", "CommentId" },
                unique: true,
                filter: "[CommentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Impressions_AppUserId_PostId",
                table: "Impressions",
                columns: new[] { "AppUserId", "PostId" },
                unique: true,
                filter: "[PostId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Impressions_AppUserId_CommentId",
                table: "Impressions");

            migrationBuilder.DropIndex(
                name: "IX_Impressions_AppUserId_PostId",
                table: "Impressions");

            migrationBuilder.CreateIndex(
                name: "IX_Impressions_AppUserId",
                table: "Impressions",
                column: "AppUserId");
        }
    }
}
