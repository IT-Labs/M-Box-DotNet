using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ItLabs.MBox.Data.Migrations
{
    public partial class Version4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Songs_Picture",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Picture",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
