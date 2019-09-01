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

        public DbSet<Track> Tracks { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
    }
}