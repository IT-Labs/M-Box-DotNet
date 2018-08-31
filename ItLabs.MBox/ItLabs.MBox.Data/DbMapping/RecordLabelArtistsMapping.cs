using ItLabs.MBox.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItLabs.MBox.Data.DbMapping
{
    public class RecordLabelArtistsMapping : IEntityTypeConfiguration<RecordLabelArtists>
    {
        public void Configure(EntityTypeBuilder<RecordLabelArtists> builder)
        {
            builder.ToTable("RecordLabelArtists");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.DateCreated).IsRequired().HasColumnType("Date"); ;
            //builder.Property(c => c.CreatedBy).IsRequired();
        }
    }
}
