using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Localizard.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.CreateTable(
                name: "Perevods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Russian = table.Column<string>(type: "text", nullable: false),
                    English = table.Column<string>(type: "text", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perevods", x => new { x.Id, x.Name, x.Russian, x.English, x.ParentId });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateTable(
                name: "Perevos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    English = table.Column<string>(type: "text", nullable: false),
                    Namekeys = table.Column<string>(type: "text", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    Russian = table.Column<string>(type: "text", nullable: false),
                    SomeValue = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perevods", x => x.Id);
                });
        }
    }
}
