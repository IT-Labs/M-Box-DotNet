using ItLabs.MBox.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;


namespace ItLabs.MBox.Data.DbMapping
{
    public class FollowMapping : IEntityTypeConfiguration<Follow>
    {
        public void Configure(EntityTypeBuilder<Follow> builder)
        {
            builder.ToTable("Follows");
            builder.HasKey(c => c.FollowId);
            builder.Property(c => c.DateCreated).IsRequired().HasColumnType("Date"); ;
            //builder.Property(c => c.CreatedBy).IsRequired();
        }
    }
}
