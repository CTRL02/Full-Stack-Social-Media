using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace socialmedia.Migrations
{
    /// <inheritdoc />
    public partial class update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.name);
                });

            migrationBuilder.CreateTable(
                name: "Connections",
                columns: table => new
                {
                    connectionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Groupname = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.connectionId);
                    table.ForeignKey(
                        name: "FK_Connections_Groups_Groupname",
                        column: x => x.Groupname,
                        principalTable: "Groups",
                        principalColumn: "name");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Connections_Groupname",
                table: "Connections",
                column: "Groupname");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Connections");

            migrationBuilder.DropTable(
                name: "Groups");
        }
    }
}
