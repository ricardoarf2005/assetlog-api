using AssetLog.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetLog.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Equipment> Equipments { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<StockNotification> StockNotifications { get; set; }
    public DbSet<Tracking> Trackings { get; set; }
    public DbSet<TrackingNotification> TrackingNotifications { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Approver)
            .WithMany()
            .HasForeignKey(o => o.ApproverId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Tracking>()
            .HasOne(t => t.TrackingNotification)
            .WithOne(tn => tn.Tracking)
            .HasForeignKey<TrackingNotification>(tn => tn.TrackingId);
    }
}
