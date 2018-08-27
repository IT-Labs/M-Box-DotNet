using ItLabs.MBox.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Data.DbMapping
{
    public class SongMapping : IEntityTypeConfiguration<Song>
    {
        public void Configure(EntityTypeBuilder<Song> builder)
        {
            builder.ToTable("Songs");
            builder.HasKey(c => c.SongId);
            builder.Property(c => c.Name).HasMaxLength(50);
            builder.Property(c => c.AlbumName).HasMaxLength(50);
            builder.Property(c => c.DateOfRelease).HasColumnType("Date");
            builder.Property(c => c.Picture).HasMaxLength(50);
            builder.HasIndex(c => c.Picture).IsUnique();
            builder.Property(c => c.Genre).HasMaxLength(50);
            builder.Property(c => c.YoutubeLink).HasMaxLength(100);
            builder.Property(c => c.VimeoLink).HasMaxLength(100);
            builder.Property(c => c.DateCreated).IsRequired().HasColumnType("Date");
            //builder.Property(c => c.CreatedBy).IsRequired();
        }
    }
}
