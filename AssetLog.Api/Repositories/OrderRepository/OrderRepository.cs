using AssetLog.Api.Data;
using AssetLog.Api.Models;
using AssetLog.Api.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace AssetLog.Api.Repositories.OrderRepository
{
    public class OrderRepository(AppDbContext db) : IOrderRepository
    {
        public async Task<List<Order>> GetUserOrdersAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await db.Orders
                .Include(o => o.User)
                .Include(o => o.Approver)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Equipment)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreationDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Order>> GetApproverOrdersAsync(int approverId, CancellationToken cancellationToken = default)
        {
            return await db.Orders
                .Include(o => o.User)
                .Include(o => o.Approver)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Equipment)
                .Where(o => o.ApproverId == approverId)
                .OrderByDescending(o => o.CreationDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Order>> GetOrdersByStatusAsync(StatusEnum status, CancellationToken cancellationToken = default)
        {
            return await db.Orders
                .Include(o => o.User)
                .Include(o => o.Approver)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Equipment)
                .Where(o => o.Status == status)
                .OrderByDescending(o => o.CreationDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<Order?> GetOrderByIdWithDetailsAsync(int orderId, CancellationToken cancellationToken = default)
        {
            return await db.Orders
                .Include(o => o.User)
                .Include(o => o.Approver)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Equipment)
                .Include(o => o.Trackings)
                    .ThenInclude(t => t.Approver)
                .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
        }

        public async Task<User?> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await db.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }

        public async Task<List<Equipment>> GetEquipmentsByIdsAsync(IEnumerable<int> equipmentIds, CancellationToken cancellationToken = default)
        {
            var ids = equipmentIds.Distinct().ToList();
            return await db.Equipments
                .Where(e => ids.Contains(e.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task AddOrderAsync(Order order, CancellationToken cancellationToken = default)
        {
            await db.Orders.AddAsync(order, cancellationToken);
        }

        public async Task AddTrackingAsync(Tracking tracking, CancellationToken cancellationToken = default)
        {
            await db.Trackings.AddAsync(tracking, cancellationToken);
        }

        public async Task AddStockNotificationAsync(StockNotification stockNotification, CancellationToken cancellationToken = default)
        {
            await db.StockNotifications.AddAsync(stockNotification, cancellationToken);
        }

        public async Task<List<TrackingNotification>> GetUserNotificationsAsync(int userId, bool? isRead, CancellationToken cancellationToken = default)
        {
            var query = db.TrackingNotifications
                .Include(tn => tn.Tracking)
                .Where(tn => tn.UserId == userId);

            if (isRead.HasValue)
            {
                query = query.Where(tn => tn.IsRead == isRead.Value);
            }

            return await query
                .OrderByDescending(tn => tn.Date)
                .ToListAsync(cancellationToken);
        }

        public async Task<TrackingNotification?> GetNotificationByIdAsync(int notificationId, CancellationToken cancellationToken = default)
        {
            return await db.TrackingNotifications
                .Include(tn => tn.Tracking)
                .FirstOrDefaultAsync(tn => tn.Id == notificationId, cancellationToken);
        }

        public async Task<List<TrackingNotification>> GetUnreadNotificationsByUserAsync(int userId, List<int>? notificationIds, CancellationToken cancellationToken = default)
        {
            var query = db.TrackingNotifications
                .Include(tn => tn.Tracking)
                .Where(tn => tn.UserId == userId && !tn.IsRead);

            if (notificationIds is { Count: > 0 })
            {
                query = query.Where(tn => notificationIds.Contains(tn.Id));
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
