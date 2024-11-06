using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Localizard.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmpClasses",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
                    DefaultLanguage = table.Column<string>(type: "text", nullable: true),
                    AvailableLanguage = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type:"date", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "date", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpClasses", x => x.Name);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmpClasses");
        }
    }
}
