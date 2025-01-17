using Microsoft.EntityFrameworkCore;
using ApiBet.Models;

namespace ApiBet.Data
{
    public class BettingContext : DbContext
    {
        public BettingContext(DbContextOptions<BettingContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<GroupInvitation> GroupInvitations { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<BlacklistedToken> BlacklistedTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Gör `Email` unikt
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Andra relationer kan behållas
            modelBuilder.Entity<UserGroup>()
                .HasKey(ug => new { ug.UserId, ug.GroupId });
        }
    }
}
