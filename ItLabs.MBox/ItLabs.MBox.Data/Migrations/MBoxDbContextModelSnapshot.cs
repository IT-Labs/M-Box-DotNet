﻿// <auto-generated />
using ItLabs.MBox.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;

namespace ItLabs.MBox.Data.Migrations
{
    [DbContext(typeof(MBoxDbContext))]
    partial class MBoxDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("ItLabs.MBox.Contracts.Entities.ApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("Date");

                    b.Property<DateTime>("DateModified");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("IsActivated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<int?>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("Picture")
                        .HasMaxLength(50);

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("ItLabs.MBox.Contracts.Entities.Artist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bio")
                        .HasMaxLength(500);

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("Date");

                    b.Property<DateTime>("DateModified");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<int?>("ModifiedBy");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Artists");
                });

            modelBuilder.Entity("ItLabs.MBox.Contracts.Entities.Configuration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("Date");

                    b.Property<DateTime>("DateModified");

                    b.Property<int>("Key");

                    b.Property<int?>("ModifiedBy");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasAlternateKey("Key");

                    b.ToTable("Configurations");
                });

            modelBuilder.Entity("ItLabs.MBox.Contracts.Entities.EmailTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body");

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("Date");

                    b.Property<DateTime>("DateModified");

                    b.Property<string>("LinkText");

                    b.Property<int?>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Subject")
                        .HasMaxLength(50);

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("EmailTemplates");
                });

            modelBuilder.Entity("ItLabs.MBox.Contracts.Entities.Follow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ArtistId");

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("Date");

                    b.Property<DateTime>("DateModified");

                    b.Property<int>("FollowerId");

                    b.Property<int?>("ModifiedBy");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.HasIndex("FollowerId");

                    b.ToTable("Follows");
                });

            modelBuilder.Entity("ItLabs.MBox.Contracts.Entities.RecordLabel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AboutInfo");

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("Date");

                    b.Property<DateTime>("DateModified");

                    b.Property<int?>("ModifiedBy");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RecordLabels");
                });

            modelBuilder.Entity("ItLabs.MBox.Contracts.Entities.RecordLabelArtist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ArtistId");

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("Date");

                    b.Property<DateTime>("DateModified");

                    b.Property<int?>("ModifiedBy");

                    b.Property<int>("RecordLabelId");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.HasIndex("RecordLabelId");

                    b.ToTable("RecordLabelArtists");
                });

            modelBuilder.Entity("ItLabs.MBox.Contracts.Entities.Song", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AlbumName")
                        .HasMaxLength(100);

                    b.Property<int>("ArtistId");

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("Date");

                    b.Property<DateTime>("DateModified");

                    b.Property<string>("Genre")
                        .HasMaxLength(50);

                    b.Property<string>("Lyrics");

                    b.Property<int?>("ModifiedBy");

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.Property<string>("Picture")
                        .HasMaxLength(50);

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("Date");

                    b.Property<string>("VimeoLink")
                        .HasMaxLength(100);

                    b.Property<string>("YouTubeLink")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.ToTable("Songs");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityRole<int>");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ItLabs.MBox.Contracts.Entities.ApplicationRole", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityRole<int>");

                    b.Property<int>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<int?>("ModifiedBy");

                    b.Property<int>("Type");

                    b.ToTable("Roles");

                    b.HasDiscriminator().HasValue("ApplicationRole");
                });

            modelBuilder.Entity("ItLabs.MBox.Contracts.Entities.Artist", b =>
                {
                    b.HasOne("ItLabs.MBox.Contracts.Entities.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("ItLabs.MBox.Contracts.Entities.Follow", b =>
                {
                    b.HasOne("ItLabs.MBox.Contracts.Entities.Artist", "Artist")
                        .WithMany("Follows")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ItLabs.MBox.Contracts.Entities.ApplicationUser", "Follower")
                        .WithMany("Follows")
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ItLabs.MBox.Contracts.Entities.RecordLabel", b =>
                {
                    b.HasOne("ItLabs.MBox.Contracts.Entities.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("ItLabs.MBox.Contracts.Entities.RecordLabelArtist", b =>
                {
                    b.HasOne("ItLabs.MBox.Contracts.Entities.Artist", "Artist")
                        .WithMany("RecordLabelArtists")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ItLabs.MBox.Contracts.Entities.RecordLabel", "RecordLabel")
                        .WithMany("RecordLabelArtists")
                        .HasForeignKey("RecordLabelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ItLabs.MBox.Contracts.Entities.Song", b =>
                {
                    b.HasOne("ItLabs.MBox.Contracts.Entities.Artist", "Artist")
                        .WithMany("Songs")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("ItLabs.MBox.Contracts.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("ItLabs.MBox.Contracts.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ItLabs.MBox.Contracts.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("ItLabs.MBox.Contracts.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
