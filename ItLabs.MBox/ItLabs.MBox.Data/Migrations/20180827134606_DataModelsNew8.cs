using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ItLabs.MBox.Data.Migrations
{
    public partial class DataModelsNew8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_Picture",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Songs");

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Songs",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Picture",
                table: "AspNetUsers",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_Songs_Picture",
                table: "Songs",
                column: "Picture",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Picture",
                table: "AspNetUsers",
                column: "Picture",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Songs_Picture",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Picture",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Songs");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Songs",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Picture",
                table: "AspNetUsers",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_Picture",
                table: "AspNetUsers",
                column: "Picture");
        }
    }
}
