using Microsoft.EntityFrameworkCore;
using BasicGameService.Models;

namespace BasicGameService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Device> Devices { get; set; } = null!;
        public DbSet<Game> Games { get; set; } = null!;
        public DbSet<Session> Sessions { get; set; } = null!;
        // Add Players etc. when needed

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional: configure relationships
            modelBuilder.Entity<Device>()
                .HasMany(d => d.InstalledGames)
                .WithMany(); // simple many-to-many, adjust if you want a join table entity

            modelBuilder.Entity<Session>()
                .HasOne(s => s.Device)
                .WithMany() // if Device should have list of sessions, change model
                .HasForeignKey(s => s.DeviceId);

            modelBuilder.Entity<Session>()
                .HasOne(s => s.Game)
                .WithMany()
                .HasForeignKey(s => s.GameId)
                .IsRequired(false);
        }
    }
}
