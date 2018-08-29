using ItLabs.MBox.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Data.DbMapping
{
    public class ArtistMapping : IEntityTypeConfiguration<Artist>
    {
        public void Configure(EntityTypeBuilder<Artist> builder)
        {
            builder.ToTable("Artists");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Bio).HasMaxLength(500);
            builder.Property(c => c.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Property(c => c.DateCreated).IsRequired().HasColumnType("Date");
            //builder.Property(c => c.CreatedBy).IsRequired();
        }
    }
}
