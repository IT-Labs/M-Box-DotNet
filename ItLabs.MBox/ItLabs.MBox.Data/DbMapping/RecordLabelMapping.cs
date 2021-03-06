﻿using ItLabs.MBox.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItLabs.MBox.Data.DbMapping
{
    public class RecordLabelMapping : IEntityTypeConfiguration<RecordLabel>
    {
        public void Configure(EntityTypeBuilder<RecordLabel> builder)
        {
            builder.ToTable("RecordLabels");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.AboutInfo);
            builder.Property(c => c.DateCreated).IsRequired().HasColumnType("Date");

            builder.HasMany(c => c.RecordLabelArtists).WithOne(c => c.RecordLabel);
            builder.Property(c => c.CreatedBy).IsRequired();
        }
    }
}
