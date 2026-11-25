using Microsoft.EntityFrameworkCore;

namespace FailSafeWeatherMonitor.Data;

public class FailSafeWeatherMonitorDbContext : DbContext
{
    public DbSet<WeatherDataPoint> WeatherDataPoints { get; set; }

    public FailSafeWeatherMonitorDbContext(DbContextOptions<FailSafeWeatherMonitorDbContext> options) : base(options)
    {

    }

    public FailSafeWeatherMonitorDbContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FailSafeWeatherMonitor;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30");
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherDataPoint>()
            .HasIndex(w => new { w.StationName, w.TimeStamp })
            .IsUnique();
    }
}



