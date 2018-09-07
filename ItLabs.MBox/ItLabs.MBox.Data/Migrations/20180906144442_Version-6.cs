using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ItLabs.MBox.Data.Migrations
{
    public partial class Version6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecordLabelArtists_Artists_ArtistId",
                table: "RecordLabelArtists");

            migrationBuilder.DropForeignKey(
                name: "FK_RecordLabelArtists_RecordLabels_RecordLabelId",
                table: "RecordLabelArtists");

            migrationBuilder.AlterColumn<int>(
                name: "RecordLabelId",
                table: "RecordLabelArtists",
                type: "int4",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ArtistId",
                table: "RecordLabelArtists",
                type: "int4",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RecordLabelArtists_Artists_ArtistId",
                table: "RecordLabelArtists",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecordLabelArtists_RecordLabels_RecordLabelId",
                table: "RecordLabelArtists",
                column: "RecordLabelId",
                principalTable: "RecordLabels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecordLabelArtists_Artists_ArtistId",
                table: "RecordLabelArtists");

            migrationBuilder.DropForeignKey(
                name: "FK_RecordLabelArtists_RecordLabels_RecordLabelId",
                table: "RecordLabelArtists");

            migrationBuilder.AlterColumn<int>(
                name: "RecordLabelId",
                table: "RecordLabelArtists",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int4");

            migrationBuilder.AlterColumn<int>(
                name: "ArtistId",
                table: "RecordLabelArtists",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int4");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordLabelArtists_Artists_ArtistId",
                table: "RecordLabelArtists",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecordLabelArtists_RecordLabels_RecordLabelId",
                table: "RecordLabelArtists",
                column: "RecordLabelId",
                principalTable: "RecordLabels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
