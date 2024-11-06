using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;



namespace Localizard.Data.Migrations
{
    public partial class AddUpdateUserRoleRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
            migrationBuilder.DropIndex(
                name: "IX_Roles_UserId",
                table: "Roles");

            
            migrationBuilder.DropColumn(
                name: "Roles",
                table: "RegisterModels");

            
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "RegisterModels",
                type: "text",
                nullable: false,
                defaultValue: "");

            
            migrationBuilder.CreateIndex(
                name: "IX_Roles_UserId",
                table: "Roles",
                column: "UserId");  
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropIndex(
                name: "IX_Roles_UserId",
                table: "Roles");

           
            migrationBuilder.DropColumn(
                name: "Role",
                table: "RegisterModels");

            
            migrationBuilder.AddColumn<List<string>>(
                name: "Roles",
                table: "RegisterModels",
                type: "text[]",
                nullable: false);

          
            migrationBuilder.CreateIndex(
                name: "IX_Roles_UserId",
                table: "Roles",
                column: "UserId");
        }
    }
}
