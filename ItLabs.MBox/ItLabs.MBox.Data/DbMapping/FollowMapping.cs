using ItLabs.MBox.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItLabs.MBox.Data.DbMapping
{
    public class FollowMapping : IEntityTypeConfiguration<Follow>
    {
        public void Configure(EntityTypeBuilder<Follow> builder)
        {
            builder.ToTable("Follows");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.DateCreated).IsRequired().HasColumnType("Date");
            builder.HasOne(c => c.Artist)
                .WithMany(b => b.Follows)
                .HasForeignKey(bc => bc.ArtistId);
            builder.HasOne(c => c.Follower)
                .WithMany(b => b.Follows)
                .HasForeignKey(bc => bc.FollowerId);
            builder.Property(c => c.CreatedBy).IsRequired();
        }
    }
}
