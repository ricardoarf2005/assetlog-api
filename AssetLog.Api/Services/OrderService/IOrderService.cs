using AssetLog.Api.DTOs;
using AssetLog.Api.Models.Enums;

namespace AssetLog.Api.Services.OrderService
{
    public interface IOrderService
    {
        Task<List<OrderSummaryDto>> GetUserOrdersAsync(int userId, CancellationToken cancellationToken = default);
        Task<List<OrderSummaryDto>> GetApproverOrdersAsync(int approverId, CancellationToken cancellationToken = default);
        Task<List<OrderSummaryDto>> GetOrdersByStatusAsync(StatusEnum status, CancellationToken cancellationToken = default);
        Task<OrderDetailDto?> GetOrderDetailsAsync(int orderId, int? userId = null, CancellationToken cancellationToken = default);
        Task<OrderDetailDto> CreateOrderAsync(CreateOrderRequestDto request, CancellationToken cancellationToken = default);
        Task<OrderDetailDto> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusRequestDto request, CancellationToken cancellationToken = default);
        Task<OrderDetailDto> UpdateExpectedDeliveryDateAsync(int orderId, UpdateExpectedDeliveryDateRequestDto request, CancellationToken cancellationToken = default);
        Task<List<TrackingNotificationDto>> GetUserNotificationsAsync(int userId, bool? isRead = null, CancellationToken cancellationToken = default);
        Task<TrackingNotificationDto?> MarkNotificationAsReadAsync(int notificationId, CancellationToken cancellationToken = default);
        Task<int> MarkUserNotificationsAsReadAsync(int userId, List<int>? notificationIds = null, CancellationToken cancellationToken = default);
    }
}
