using Microsoft.EntityFrameworkCore;
using WebAPIWithAuth.Models;

namespace WebAPIWithAuth.Data;

public class HeroContext(DbContextOptions<HeroContext> options) : DbContext(options)
{
    public DbSet<Hero> Hero { get; set; }
    public DbSet<User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hero>()
            .HasKey(h => h.Id);

        modelBuilder.Entity<Hero>().HasData(
            new Hero
            {
                Id = 1,
                FirstName = "System"
            });

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                FirstName = "System",
                LastName = "",
                UserName = "System",
                Password = "System"
            });
    }
}
