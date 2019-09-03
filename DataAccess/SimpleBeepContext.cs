using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimpleBeep.Models;

namespace SimpleBeep.DataAccess
{
    public class SimpleBeepContext : DbContext
    {
        public SimpleBeepContext(DbContextOptions<SimpleBeepContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Playlist>()
                .Property(pl => pl.Name).IsRequired();
            modelBuilder.Entity<Track>()
                .Property(t => t.Name).IsRequired();

            modelBuilder.Entity<Track>().HasOne(p => p.Playlist).WithMany(t => t.Tracks);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes()) {
                modelBuilder.Entity(entityType.Name).Property<DateTime>("Created");
                modelBuilder.Entity(entityType.Name).Property<DateTime?>("LastModified");
            }
        }

        public DbSet<Track> Tracks { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            ShadowPropertyUpdate();

            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges() {
            ShadowPropertyUpdate();

            return base.SaveChanges();
        }

        private void ShadowPropertyUpdate() {
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)) {
                if (entry.State == EntityState.Added)
                    entry.Property("Created").CurrentValue = DateTime.Now;
                else
                    entry.Property("LastModified").CurrentValue = DateTime.Now;
            }
        }
    }
}