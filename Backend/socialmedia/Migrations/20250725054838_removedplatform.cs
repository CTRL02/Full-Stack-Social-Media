using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace socialmedia.Migrations
{
    /// <inheritdoc />
    public partial class removedplatform : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Platform",
                table: "SocialLinks");

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<string>(
                name: "Platform",
                table: "SocialLinks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
