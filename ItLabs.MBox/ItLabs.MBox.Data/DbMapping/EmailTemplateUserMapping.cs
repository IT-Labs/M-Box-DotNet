using ItLabs.MBox.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Data.DbMapping
{
    class EmailTemplateUserMapping : IEntityTypeConfiguration<EmailTemplate>
    {
        public void Configure(EntityTypeBuilder<EmailTemplate> builder)
        {
            builder.ToTable("EmailTemplates");
            builder.HasKey(c => c.EmailTemplateId);
            builder.Property(c => c.Name).HasMaxLength(100);
            builder.HasAlternateKey(c => c.Name);
            builder.Property(c => c.Subject).HasMaxLength(50);
            builder.Property(c => c.DateCreated).IsRequired().HasColumnType("Date");
            //builder.Property(c => c.CreatedBy).IsRequired();
        }
    }
}
