using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebsiteWebApI.Data.Migrations
{
    public partial class OneOrZero : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Websites_WebsiteId",
                table: "Files");

            migrationBuilder.AlterColumn<Guid>(
                name: "WebsiteId",
                table: "Files",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Websites_WebsiteId",
                table: "Files",
                column: "WebsiteId",
                principalTable: "Websites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Websites_WebsiteId",
                table: "Files");

            migrationBuilder.AlterColumn<Guid>(
                name: "WebsiteId",
                table: "Files",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Websites_WebsiteId",
                table: "Files",
                column: "WebsiteId",
                principalTable: "Websites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
