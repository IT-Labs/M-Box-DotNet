using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Data.DbMapping;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Claims;

namespace ItLabs.MBox.Data
{
    public class MBoxDbContext : IdentityDbContext<ApplicationUser, ApplicationUserRole, int> 
    {

        public MBoxDbContext(DbContextOptions<MBoxDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            

            builder.ApplyConfiguration(new ApplicationUserMapping());
            builder.ApplyConfiguration(new ArtistMapping());
            builder.ApplyConfiguration(new ConfigurationMapping());
            builder.ApplyConfiguration(new EmailTemplateUserMapping());
            builder.ApplyConfiguration(new FollowMapping());
            builder.ApplyConfiguration(new RecordLabelArtistsMapping());
            builder.ApplyConfiguration(new RecordLabelMapping());
            builder.ApplyConfiguration(new SongMapping());

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            
        }

        public override int SaveChanges()
        {
            this.AuditEntities();

            return base.SaveChanges();
        }

        private void AuditEntities()
        {

            foreach (var auditableEntity in ChangeTracker.Entries<IAuditable>())
            {
                if (auditableEntity.State == EntityState.Added || auditableEntity.State == EntityState.Modified)
                {
                    auditableEntity.Entity.DateModified = DateTime.Now;

                    if (auditableEntity.State == EntityState.Added)
                    {
                        auditableEntity.Entity.DateCreated = DateTime.Now;
                    }
                }
            }
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }

        public DbSet<Song> Songs { get; set; }

        public DbSet<Configuration> Configurations { get; set; }
    
        public DbSet<EmailTemplate> EmailTemplates { get; set; }

        public DbSet<Follow> Follows { get; set; }

        public DbSet<RecordLabel> RecordLabels { get; set; }

        public DbSet<RecordLabelArtists> RecordLabelArtists { get; set; }

        public DbSet<Artist> Artists { get; set; }

        
    }
}
