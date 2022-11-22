using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaseProject.Persistence.Migrations
{
    public partial class Update07 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "SecretKey",
                table: "OtpAuthenticators",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateIndex(
                name: "IX_OtpAuthenticators_UserId",
                table: "OtpAuthenticators",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OtpAuthenticators_Users_UserId",
                table: "OtpAuthenticators",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OtpAuthenticators_Users_UserId",
                table: "OtpAuthenticators");

            migrationBuilder.DropIndex(
                name: "IX_OtpAuthenticators_UserId",
                table: "OtpAuthenticators");

            migrationBuilder.DropColumn(
                name: "SecretKey",
                table: "OtpAuthenticators");
        }
    }
}
