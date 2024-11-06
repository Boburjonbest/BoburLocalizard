using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Localizard.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMyEntity : Migration
    {
        public object CreatedAt { get; internal set; }

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MyEntities",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    DefaultLAnguage = table.Column<string>(type: "text", nullable: true),
                    AvailableLanguage = table.Column<Array>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime?>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime?>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyEntities", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MyEntities");
        }
    }
}
