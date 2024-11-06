using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Localizard.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddObyektTranslation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

           
            migrationBuilder.CreateTable(
                name: "ObyektPerevods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Namekeys = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Tags = table.Column<int[]>(type: "integer[]", nullable: false),
                    FileNameIOS = table.Column<string>(type: "text", nullable: false),
                    FileNameAndroid = table.Column<string>(type: "text", nullable: false),
                    FileNameWeb = table.Column<string>(type: "text", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObyektPerevods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObyektTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Key = table.Column<string>(type: "text", nullable: false),
                    Language = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    ObyektPerevodId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObyektTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObyektTranslations_ObyektPerevods_ObyektPerevodId",
                        column: x => x.ObyektPerevodId,
                        principalTable: "ObyektPerevods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ObyektTranslations_ObyektPerevodId",
                table: "ObyektTranslations",
                column: "ObyektPerevodId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ObyektTranslations");

            migrationBuilder.DropTable(
                name: "ObyektPerevods");

            migrationBuilder.CreateTable(
                name: "Translationperevods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false),
                    FileNameAndroid = table.Column<string>(type: "text", nullable: false),
                    FileNameIOS = table.Column<string>(type: "text", nullable: false),
                    FileNameWeb = table.Column<string>(type: "text", nullable: false),
                    Namekeys = table.Column<string>(type: "text", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    Tags = table.Column<List<int>>(type: "integer[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translationperevods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Translations",
                columns: table => new
                {
                    Key = table.Column<string>(type: "text", nullable: false),
                    Language = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    TranslationperevodId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translations", x => new { x.Key, x.Language });
                    table.ForeignKey(
                        name: "FK_Translations_Translationperevods_TranslationperevodId",
                        column: x => x.TranslationperevodId,
                        principalTable: "Translationperevods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Translations_TranslationperevodId",
                table: "Translations",
                column: "TranslationperevodId");
        }
    }
}
