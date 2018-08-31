using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ItLabs.MBox.Data.Migrations
{
    public partial class Version4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecordLabels_AspNetUsers_UserIdId",
                table: "RecordLabels");

            migrationBuilder.DropIndex(
                name: "IX_RecordLabels_UserIdId",
                table: "RecordLabels");

            migrationBuilder.DropColumn(
                name: "UserIdId",
                table: "RecordLabels");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "RecordLabels",
                type: "int4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecordLabels_UserId",
                table: "RecordLabels",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordLabels_AspNetUsers_UserId",
                table: "RecordLabels",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecordLabels_AspNetUsers_UserId",
                table: "RecordLabels");

            migrationBuilder.DropIndex(
                name: "IX_RecordLabels_UserId",
                table: "RecordLabels");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RecordLabels");

            migrationBuilder.AddColumn<int>(
                name: "UserIdId",
                table: "RecordLabels",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecordLabels_UserIdId",
                table: "RecordLabels",
                column: "UserIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordLabels_AspNetUsers_UserIdId",
                table: "RecordLabels",
                column: "UserIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
