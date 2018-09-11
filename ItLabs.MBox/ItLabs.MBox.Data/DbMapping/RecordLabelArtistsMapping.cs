using ItLabs.MBox.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItLabs.MBox.Data.DbMapping
{
    public class RecordLabelArtistsMapping : IEntityTypeConfiguration<RecordLabelArtist>
    {
        public void Configure(EntityTypeBuilder<RecordLabelArtist> builder)
        {
            builder.ToTable("RecordLabelArtists");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.DateCreated).IsRequired().HasColumnType("Date");
            builder.HasOne(c => c.Artist)
                .WithMany(b => b.RecordLabelArtists)
                .HasForeignKey(bc => bc.ArtistId);
            builder.HasOne(c => c.RecordLabel)
                .WithMany(b => b.RecordLabelArtists)
                .HasForeignKey(bc => bc.RecordLabelId);
            builder.Property(c => c.CreatedBy).IsRequired();
        }
    }
}
