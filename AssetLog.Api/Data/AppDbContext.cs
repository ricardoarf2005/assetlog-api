using AssetLog.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetLog.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    DbSet<Equipment> Equipments { get; set; }
    DbSet<Order> Orders { get; set; }
    DbSet<OrderItem> OrderItems { get; set; }
    DbSet<Stock> Stocks { get; set; }
    DbSet<StockNotification> StockNotifications { get; set; }
    DbSet<Tracking> Trackings { get; set; }
    DbSet<TrackingNotification> TrackingNotifications { get; set; }
    DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany()
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Approver)
            .WithMany()
            .HasForeignKey(o => o.ApproverId)
            .OnDelete(DeleteBehavior.Restrict);

         modelBuilder.Entity<Tracking>()
            .HasOne(t => t.TrackingNotification)
            .WithOne(tn => tn.Tracking)
            .HasForeignKey<TrackingNotification>(tn => tn.TrackingId);
    }
}