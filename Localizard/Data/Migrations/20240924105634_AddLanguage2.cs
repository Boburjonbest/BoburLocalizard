using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Localizard.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLanguage2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Translations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SomeValue",
                table: "Translations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "SomeValue",
                table: "Translations");
        }
    }
}
