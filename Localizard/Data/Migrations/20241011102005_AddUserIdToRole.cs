using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Localizard.Data.Migrations
{
    public partial class AddUserIdToRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Roles",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Roles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<List<string>>(
                name: "Roles",
                table: "RegisterModels",
                type: "text[]",
                nullable: false);

           
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "UserId",
                value:7 );

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "UserId",
                value:7 );

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UserId",
                table: "Roles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_UserId",
                table: "Roles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_UserId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_UserId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Roles",
                table: "RegisterModels");

            migrationBuilder.AddColumn<List<string>>(
                name: "Roles",
                table: "Users",
                type: "text[]",
                nullable: false);
        }
    }
}
