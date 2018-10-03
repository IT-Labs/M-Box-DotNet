using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Data.DbMapping;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Identity;
using ItLabs.MBox.Contracts.Interfaces;

namespace ItLabs.MBox.Data
{
    public class MBoxDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int> 
    {

        public MBoxDbContext(DbContextOptions<MBoxDbContext> options) : base(options) { }

        internal ApplicationUser SingleOrDefault(Func<object, bool> p)
        {
            throw new NotImplementedException();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ApplicationUserMapping());
            builder.ApplyConfiguration(new ApplicationRoleMapping());
            builder.ApplyConfiguration(new ArtistMapping());
            builder.ApplyConfiguration(new RecordLabelMapping());
            builder.ApplyConfiguration(new ConfigurationMapping());
            builder.ApplyConfiguration(new EmailTemplateUserMapping());
            builder.ApplyConfiguration(new FollowMapping());
            builder.ApplyConfiguration(new RecordLabelArtistsMapping());
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

            foreach (var auditableEntity in ChangeTracker.Entries<IEntity>())
            {
                if(auditableEntity.Entity is Artist)
                {
                    if (((Artist)auditableEntity.Entity).User != null && auditableEntity.State != EntityState.Modified)
                        auditableEntity.Entity.Id = ((Artist)auditableEntity.Entity).User.Id;
                }

                if (auditableEntity.Entity is RecordLabel)
                {
                    if(((RecordLabel)auditableEntity.Entity).User != null && auditableEntity.State != EntityState.Modified)
                        auditableEntity.Entity.Id = ((RecordLabel)auditableEntity.Entity).User.Id;
                }


                if (auditableEntity.State == EntityState.Added || auditableEntity.State == EntityState.Modified)
                {
                    auditableEntity.Entity.DateModified = DateTime.UtcNow;

                    if (auditableEntity.State == EntityState.Added)
                    {
                        auditableEntity.Entity.DateCreated = DateTime.UtcNow;
                    }
                }

            }
            foreach (var auditableEntity in ChangeTracker.Entries<ApplicationUser>())
            {
                if (auditableEntity.State == EntityState.Added || auditableEntity.State == EntityState.Modified)
                {
                    auditableEntity.Entity.DateModified = DateTime.UtcNow;

                    if (auditableEntity.State == EntityState.Added)
                    {
                        auditableEntity.Entity.DateCreated = DateTime.UtcNow;
                    }
                }
            }
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<ApplicationRole> ApplicationRoles { get; set; }

        public DbSet<Artist> Artists { get; set; }

        public DbSet<Song> Songs { get; set; }

        public DbSet<Configuration> Configurations { get; set; }
    
        public DbSet<EmailTemplate> EmailTemplates { get; set; }

        public DbSet<Follow> Follows { get; set; }

        public DbSet<RecordLabel> RecordLabels { get; set; }

        public DbSet<RecordLabelArtist> RecordLabelArtists { get; set; } 
    }
}
