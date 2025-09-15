using GameOver.Models;
using Microsoft.EntityFrameworkCore;

namespace GameOver.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Game> Games { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<GameDevice> GameDevices { get; set; }

    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasData(new Category[]
            {
                new Category{Id = 1, Name = "Sports"},
                new Category{Id = 2, Name = "Action"},
                new Category{Id = 3, Name = "Adventure"},
                new Category{Id = 4, Name = "Racing"},
                new Category{Id = 5, Name = "Puzzle"},
                new Category{Id = 6, Name = "Strategy"},
                new Category{Id = 7, Name = "Simulation"},
                new Category{Id = 8, Name = "Role-Playing"},
                new Category{Id = 9, Name = "Horror"},
                new Category{Id = 10, Name = "Multiplayer"},
                new Category{Id = 11, Name = "Educational"},
                new Category{Id = 12, Name = "Arcade"},
            });
        
        
        modelBuilder.Entity<Device>()
            .HasData(new Device[]
            {
                new Device{Id = 1, Name = "PlayStation", Icon = "bi bi-playstation"},
                new Device{Id = 2, Name = "Xbox", Icon = "bi bi-xbox"},
                new Device{Id = 3, Name = "Nintendo Switch", Icon = "bi bi-nintendo-switch"},
                new Device{Id = 4, Name = "PC", Icon = "bi bi-pc-display-horizontal"},
                new Device{Id = 5, Name = "Mobile", Icon = "bi bi-phone"},
                new Device{Id = 6, Name = "Tablet", Icon = "bi bi-tablet"},
                new Device{Id = 7, Name = "VR Headset", Icon = "bi bi-badge-vr"},
                new Device{Id = 8, Name = "Smart TV", Icon = "bi bi-tv"},
            });
        
        
        modelBuilder.Entity<GameDevice>()
            .HasKey(e => new { e.GameId, e.DeviceId });

        base.OnModelCreating(modelBuilder);
    }
}