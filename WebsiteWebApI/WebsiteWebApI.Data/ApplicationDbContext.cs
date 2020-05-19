using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebsiteWebApI.DataModels;
using WebsiteWebApI.DataModels.BaseModels;
using WebsiteWebApI.DataModels.Identity;

namespace WebsiteWebApI.Data
{
    public class ApplicationDbContext : IdentityDbContext<SystemUser,SystemRole, Guid>
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<WebsiteEntity> Websites { get; set; }

        public DbSet<WebsiteCategory> WebsiteCategories { get; set; }

        public virtual DbSet<Url> Urls { get; set; }

        public DbSet<File> Files { get; set; }

        public DbSet<FileProvider> FileProviders { get; set; }

        public virtual DbSet<FileBlob> FileBlobs { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<FileBlob>()
                .HasIndex(x => new { x.FileId });

        }

        public virtual void SetState(object entity, EntityState state)
        {
            Entry(entity).State = state;
        }

        public override int SaveChanges()
        {
            this.UpdateSoftDeleteStatuses();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            this.UpdateSoftDeleteStatuses();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateSoftDeleteStatuses()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if(!(entry.Entity is BaseDbModel))
                {
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues[nameof(BaseDbModel.IsDeleted)] = false;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues[nameof(BaseDbModel.IsDeleted)] = true;
                        break;
                }
            }
        }
    }
}
