using ItLabs.MBox.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItLabs.MBox.Data.DbMapping
{
    public class ConfigurationMapping : IEntityTypeConfiguration<Configuration>
    {
        public void Configure(EntityTypeBuilder<Configuration> builder)
        {
            builder.ToTable("Configurations");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Key).HasMaxLength(100);
            builder.HasAlternateKey(c => c.Key);
            builder.Property(c => c.DateCreated).IsRequired().HasColumnType("Date");
            //builder.Property(c => c.CreatedBy).IsRequired();
        }
    }
}
