using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Localizard.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectToUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "MyEntities",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MyEntities");
        }
    }
}
