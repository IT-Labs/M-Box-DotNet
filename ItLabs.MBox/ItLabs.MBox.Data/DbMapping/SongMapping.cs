using ItLabs.MBox.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItLabs.MBox.Data.DbMapping
{
    public class SongMapping : IEntityTypeConfiguration<Song>
    {
        public void Configure(EntityTypeBuilder<Song> builder)
        {
            builder.ToTable("Songs");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).HasMaxLength(50);
            builder.Property(c => c.AlbumName).HasMaxLength(50);
            builder.Property(c => c.ReleaseDate).HasColumnType("Date");
            builder.Property(c => c.Picture).HasMaxLength(50);
            builder.HasIndex(c => c.Picture).IsUnique();
            builder.Property(c => c.Genre).HasMaxLength(50);
            builder.Property(c => c.YouTubeLink).HasMaxLength(100);
            builder.Property(c => c.VimeoLink).HasMaxLength(100);
            builder.Property(c => c.DateCreated).IsRequired().HasColumnType("Date");
            builder.HasOne(x => x.Artist).WithMany(x => x.Songs);
            //builder.Property(c => c.CreatedBy).IsRequired();
        }
    }
}
