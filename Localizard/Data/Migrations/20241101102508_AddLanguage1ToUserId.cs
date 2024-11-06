using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Localizard.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLanguage1ToUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "languages",
                type: "text", 
                nullable: false, 
                defaultValue: ""); 
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "languages");
        }
    }
}
