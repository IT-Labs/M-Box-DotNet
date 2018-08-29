using ItLabs.MBox.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Data.DbMapping
{
    public class ApplicationUserMapping : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).HasMaxLength(50).IsRequired();
            builder.Property(c => c.Picture).HasMaxLength(50);
            builder.HasIndex(c => c.Picture).IsUnique();
            builder.Property(c => c.IsActivated).IsRequired().HasDefaultValue(false);
            builder.Property(c => c.DateCreated).IsRequired().HasColumnType("Date");
            //builder.Property(c => c.CreatedBy).IsRequired();
        }
    }
}
