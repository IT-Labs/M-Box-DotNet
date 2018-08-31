using ItLabs.MBox.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItLabs.MBox.Data.DbMapping
{
    public class ApplicationRoleMapping : IEntityTypeConfiguration<ApplicationRole>
        {
            public void Configure(EntityTypeBuilder<ApplicationRole> builder)
            {
                builder.ToTable("Roles");
                //builder.Ignore(c => c.Type);
            }
        }
}

