using AssetLog.Api.Models;
using AssetLog.Api.Models.Enums;

namespace AssetLog.Api.Repositories.OrderRepository
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetUserOrdersAsync(int userId, CancellationToken cancellationToken = default);
        Task<List<Order>> GetApproverOrdersAsync(int approverId, CancellationToken cancellationToken = default);
        Task<List<Order>> GetOrdersByStatusAsync(StatusEnum status, CancellationToken cancellationToken = default);
        Task<Order?> GetOrderByIdWithDetailsAsync(int orderId, CancellationToken cancellationToken = default);
        Task<User?> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default);
        Task<List<Equipment>> GetEquipmentsByIdsAsync(IEnumerable<int> equipmentIds, CancellationToken cancellationToken = default);
        Task AddOrderAsync(Order order, CancellationToken cancellationToken = default);
        Task AddTrackingAsync(Tracking tracking, CancellationToken cancellationToken = default);
        Task AddStockNotificationAsync(StockNotification stockNotification, CancellationToken cancellationToken = default);
        Task<List<TrackingNotification>> GetUserNotificationsAsync(int userId, bool? isRead, CancellationToken cancellationToken = default);
        Task<TrackingNotification?> GetNotificationByIdAsync(int notificationId, CancellationToken cancellationToken = default);
        Task<List<TrackingNotification>> GetUnreadNotificationsByUserAsync(int userId, List<int>? notificationIds, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
