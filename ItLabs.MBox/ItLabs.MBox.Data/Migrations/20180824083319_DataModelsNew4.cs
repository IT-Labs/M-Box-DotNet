﻿using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ItLabs.MBox.Data.Migrations
{
    public partial class DataModelsNew4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    Role = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Configurations",
                columns: table => new
                {
                    ConfigurationId = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<int>(type: "int4", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "Date", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp", nullable: false),
                    Key = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<int>(type: "int4", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configurations", x => x.ConfigurationId);
                    table.UniqueConstraint("AK_Configurations_Key", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "EmailTemplates",
                columns: table => new
                {
                    EmailTemplateId = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Body = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<int>(type: "int4", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "Date", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int4", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Subject = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplates", x => x.EmailTemplateId);
                    table.UniqueConstraint("AK_EmailTemplates_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true),
                    RoleId = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AccessFailedCount = table.Column<int>(type: "int4", nullable: false),
                    ApplicationUserRoleId = table.Column<int>(type: "int4", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<int>(type: "int4", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "Date", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp", nullable: false),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bool", nullable: false),
                    IsActivated = table.Column<bool>(type: "bool", nullable: false, defaultValue: false),
                    LockoutEnabled = table.Column<bool>(type: "bool", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int4", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bool", nullable: false),
                    Picture = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bool", nullable: false),
                    UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.UniqueConstraint("AK_AspNetUsers_Picture", x => x.Picture);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_AspNetRoles_ApplicationUserRoleId",
                        column: x => x.ApplicationUserRoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    ArtistId = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ApplicationUserId = table.Column<int>(type: "int4", nullable: true),
                    Bio = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<int>(type: "int4", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "Date", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bool", nullable: false, defaultValue: false),
                    ModifiedBy = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.ArtistId);
                    table.ForeignKey(
                        name: "FK_Artists_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int4", nullable: false),
                    RoleId = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int4", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecordLabels",
                columns: table => new
                {
                    RecordLabelId = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AboutInfo = table.Column<string>(type: "text", nullable: true),
                    ApplicationUserId = table.Column<int>(type: "int4", nullable: true),
                    CreatedBy = table.Column<int>(type: "int4", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "Date", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordLabels", x => x.RecordLabelId);
                    table.ForeignKey(
                        name: "FK_RecordLabels_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Follows",
                columns: table => new
                {
                    FollowId = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ArtistId = table.Column<int>(type: "int4", nullable: true),
                    CreatedBy = table.Column<int>(type: "int4", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "Date", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp", nullable: false),
                    FollowerId = table.Column<int>(type: "int4", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follows", x => x.FollowId);
                    table.ForeignKey(
                        name: "FK_Follows_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "ArtistId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Follows_AspNetUsers_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                columns: table => new
                {
                    SongId = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AlbumName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    ArtistId = table.Column<int>(type: "int4", nullable: true),
                    CreatedBy = table.Column<int>(type: "int4", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "Date", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp", nullable: false),
                    DateOfRelease = table.Column<DateTime>(type: "Date", nullable: false),
                    Genre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Image = table.Column<string>(type: "text", nullable: true),
                    Lyrics = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int4", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    VimeoLink = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    YoutubeLink = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Songs", x => x.SongId);
                    table.ForeignKey(
                        name: "FK_Songs_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "ArtistId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecordLabelArtists",
                columns: table => new
                {
                    RecordLabelArtistsId = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ArtistId = table.Column<int>(type: "int4", nullable: true),
                    CreatedBy = table.Column<int>(type: "int4", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "Date", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int4", nullable: false),
                    RecordLabelId = table.Column<int>(type: "int4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordLabelArtists", x => x.RecordLabelArtistsId);
                    table.ForeignKey(
                        name: "FK_RecordLabelArtists_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "ArtistId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecordLabelArtists_RecordLabels_RecordLabelId",
                        column: x => x.RecordLabelId,
                        principalTable: "RecordLabels",
                        principalColumn: "RecordLabelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Artists_ApplicationUserId",
                table: "Artists",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ApplicationUserRoleId",
                table: "AspNetUsers",
                column: "ApplicationUserRoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Follows_ArtistId",
                table: "Follows",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_FollowerId",
                table: "Follows",
                column: "FollowerId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordLabelArtists_ArtistId",
                table: "RecordLabelArtists",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordLabelArtists_RecordLabelId",
                table: "RecordLabelArtists",
                column: "RecordLabelId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordLabels_ApplicationUserId",
                table: "RecordLabels",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_ArtistId",
                table: "Songs",
                column: "ArtistId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Configurations");

            migrationBuilder.DropTable(
                name: "EmailTemplates");

            migrationBuilder.DropTable(
                name: "Follows");

            migrationBuilder.DropTable(
                name: "RecordLabelArtists");

            migrationBuilder.DropTable(
                name: "Songs");

            migrationBuilder.DropTable(
                name: "RecordLabels");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");
        }
    }
}
