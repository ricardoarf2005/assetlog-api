using AssetLog.Api.DTOs;
using AssetLog.Api.Models;
using AssetLog.Api.Models.Enums;
using AssetLog.Api.Repositories.OrderRepository;

namespace AssetLog.Api.Services.OrderService
{
    public class OrderService(IOrderRepository orderRepository) : IOrderService
    {
        public async Task<List<OrderSummaryDto>> GetUserOrdersAsync(int userId, CancellationToken cancellationToken = default)
        {
            var orders = await orderRepository.GetUserOrdersAsync(userId, cancellationToken);
            return orders.Select(MapToSummaryDto).ToList();
        }

        public async Task<List<OrderSummaryDto>> GetApproverOrdersAsync(int approverId, CancellationToken cancellationToken = default)
        {
            var orders = await orderRepository.GetApproverOrdersAsync(approverId, cancellationToken);
            return orders.Select(MapToSummaryDto).ToList();
        }

        public async Task<List<OrderSummaryDto>> GetOrdersByStatusAsync(StatusEnum status, CancellationToken cancellationToken = default)
        {
            var orders = await orderRepository.GetOrdersByStatusAsync(status, cancellationToken);
            return orders.Select(MapToSummaryDto).ToList();
        }

        public async Task<OrderDetailDto?> GetOrderDetailsAsync(int orderId, int? userId = null, CancellationToken cancellationToken = default)
        {
            var order = await orderRepository.GetOrderByIdWithDetailsAsync(orderId, cancellationToken);
            if (order is null)
            {
                return null;
            }

            if (userId.HasValue && order.UserId != userId.Value)
            {
                return null;
            }

            return MapToDetailDto(order);
        }

        public async Task<OrderDetailDto> CreateOrderAsync(CreateOrderRequestDto request, CancellationToken cancellationToken = default)
        {
            var user = await orderRepository.GetUserByIdAsync(request.UserId, cancellationToken)
                ?? throw new KeyNotFoundException($"User with ID {request.UserId} was not found.");

            if (request.Items is null || request.Items.Count == 0)
            {
                throw new ArgumentException("Order must contain at least one item.");
            }

            var equipmentIds = request.Items.Select(i => i.EquipmentId).Distinct().ToList();
            var equipments = await orderRepository.GetEquipmentsByIdsAsync(equipmentIds, cancellationToken);

            if (equipments.Count != equipmentIds.Count)
            {
                var foundIds = equipments.Select(e => e.Id).ToHashSet();
                var missingIds = equipmentIds.Where(id => !foundIds.Contains(id));
                throw new KeyNotFoundException($"Equipment(s) not found: {string.Join(", ", missingIds)}.");
            }

            var inactiveEquipments = equipments.Where(e => !e.IsActive).ToList();
            if (inactiveEquipments.Count > 0)
            {
                throw new InvalidOperationException($"Cannot order inactive equipment(s): {string.Join(", ", inactiveEquipments.Select(e => e.Name))}.");
            }

            var now = DateTime.UtcNow;
            var order = new Order
            {
                UserId = user.Id,
                ApproverId = null,
                CreationDate = now,
                Status = StatusEnum.OPENED,
                ExpectedDeliveryDate = request.ExpectedDeliveryDate ?? now.AddDays(7),
                OrderItems = request.Items
                    .GroupBy(i => i.EquipmentId)
                    .Select(g => new OrderItem
                    {
                        EquipmentId = g.Key,
                        Quantity = g.Sum(x => x.Quantity)
                    })
                    .ToList()
            };

            await orderRepository.AddOrderAsync(order, cancellationToken);
            await orderRepository.SaveChangesAsync(cancellationToken);

            var createdOrder = await orderRepository.GetOrderByIdWithDetailsAsync(order.Id, cancellationToken);
            return MapToDetailDto(createdOrder!);
        }

        public async Task<OrderDetailDto> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusRequestDto request, CancellationToken cancellationToken = default)
        {
            var order = await orderRepository.GetOrderByIdWithDetailsAsync(orderId, cancellationToken)
                ?? throw new KeyNotFoundException($"Order with ID {orderId} was not found.");

            var approver = await orderRepository.GetUserByIdAsync(request.ApproverId, cancellationToken)
                ?? throw new KeyNotFoundException($"Approver with ID {request.ApproverId} was not found.");

            var previousStatus = order.Status;
            var targetStatus = request.Status;
            order.ApproverId = approver.Id;
            order.Approver = approver;

            string notificationMessage;

            if (targetStatus == StatusEnum.APPROVED && previousStatus != StatusEnum.APPROVED)
            {
                var insufficientItems = order.OrderItems
                    .Where(oi => oi.Equipment.Stock < oi.Quantity)
                    .ToList();

                if (insufficientItems.Count > 0)
                {
                    targetStatus = StatusEnum.BLOCKED;
                    var itemNames = string.Join(", ", insufficientItems.Select(i => $"{i.Equipment.Name} (requested: {i.Quantity}, available: {i.Equipment.Stock})"));
                    notificationMessage = $"Order #{order.Id} was blocked due to insufficient stock for: {itemNames}.";

                    foreach (var item in insufficientItems)
                    {
                        await orderRepository.AddStockNotificationAsync(new StockNotification
                        {
                            ApproverId = approver.Id,
                            EquipmentId = item.EquipmentId,
                            IsRead = false,
                            Message = $"Insufficient stock for '{item.Equipment.Name}' on Order #{order.Id}. Requested: {item.Quantity}, Available: {item.Equipment.Stock}."
                        }, cancellationToken);
                    }
                }
                else
                {
                    foreach (var item in order.OrderItems)
                    {
                        item.Equipment.Stock -= item.Quantity;

                        if (item.Equipment.Stock <= item.Equipment.MinStock)
                        {
                            await orderRepository.AddStockNotificationAsync(new StockNotification
                            {
                                ApproverId = approver.Id,
                                EquipmentId = item.EquipmentId,
                                IsRead = false,
                                Message = $"Low stock alert for '{item.Equipment.Name}'. Current stock: {item.Equipment.Stock} (MinStock: {item.Equipment.MinStock})."
                            }, cancellationToken);
                        }
                    }

                    notificationMessage = $"Order #{order.Id} status updated to {targetStatus} by {approver.Name}.";
                }
            }
            else if (targetStatus == StatusEnum.RETURNED && previousStatus != StatusEnum.RETURNED)
            {
                foreach (var item in order.OrderItems)
                {
                    item.Equipment.Stock += item.Quantity;
                }

                notificationMessage = $"Order #{order.Id} items were returned and status updated to {targetStatus}.";
            }
            else
            {
                notificationMessage = $"Order #{order.Id} status updated to {targetStatus} by {approver.Name}.";
            }

            if (!string.IsNullOrWhiteSpace(request.Comments))
            {
                notificationMessage += $" Comments: {request.Comments}";
            }

            order.Status = targetStatus;

            var now = DateTime.UtcNow;
            var tracking = new Tracking
            {
                ApproverId = approver.Id,
                OrderId = order.Id,
                Date = now,
                Status = targetStatus,
                Comments = request.Comments,
                TrackingNotification = new TrackingNotification
                {
                    UserId = order.UserId,
                    Date = now,
                    IsRead = false,
                    Message = notificationMessage
                }
            };

            await orderRepository.AddTrackingAsync(tracking, cancellationToken);
            await orderRepository.SaveChangesAsync(cancellationToken);

            tracking.TrackingNotificationId = tracking.TrackingNotification.Id;
            await orderRepository.SaveChangesAsync(cancellationToken);

            var updatedOrder = await orderRepository.GetOrderByIdWithDetailsAsync(order.Id, cancellationToken);
            return MapToDetailDto(updatedOrder!);
        }

        public async Task<OrderDetailDto> UpdateExpectedDeliveryDateAsync(int orderId, UpdateExpectedDeliveryDateRequestDto request, CancellationToken cancellationToken = default)
        {
            var order = await orderRepository.GetOrderByIdWithDetailsAsync(orderId, cancellationToken)
                ?? throw new KeyNotFoundException($"Order with ID {orderId} was not found.");

            var approver = await orderRepository.GetUserByIdAsync(request.ApproverId, cancellationToken)
                ?? throw new KeyNotFoundException($"Approver with ID {request.ApproverId} was not found.");

            order.ExpectedDeliveryDate = request.ExpectedDeliveryDate;
            order.ApproverId ??= approver.Id;

            var now = DateTime.UtcNow;
            var message = $"Order #{order.Id} expected delivery date updated to {request.ExpectedDeliveryDate:yyyy-MM-dd} by {approver.Name}.";
            if (!string.IsNullOrWhiteSpace(request.Comments))
            {
                message += $" Comments: {request.Comments}";
            }

            var tracking = new Tracking
            {
                ApproverId = approver.Id,
                OrderId = order.Id,
                Date = now,
                Status = order.Status,
                Comments = request.Comments ?? $"Expected delivery date changed to {request.ExpectedDeliveryDate:yyyy-MM-dd}",
                TrackingNotification = new TrackingNotification
                {
                    UserId = order.UserId,
                    Date = now,
                    IsRead = false,
                    Message = message
                }
            };

            await orderRepository.AddTrackingAsync(tracking, cancellationToken);
            await orderRepository.SaveChangesAsync(cancellationToken);

            tracking.TrackingNotificationId = tracking.TrackingNotification.Id;
            await orderRepository.SaveChangesAsync(cancellationToken);

            var updatedOrder = await orderRepository.GetOrderByIdWithDetailsAsync(order.Id, cancellationToken);
            return MapToDetailDto(updatedOrder!);
        }

        public async Task<List<TrackingNotificationDto>> GetUserNotificationsAsync(int userId, bool? isRead = null, CancellationToken cancellationToken = default)
        {
            var notifications = await orderRepository.GetUserNotificationsAsync(userId, isRead, cancellationToken);
            return notifications.Select(MapToNotificationDto).ToList();
        }

        public async Task<TrackingNotificationDto?> MarkNotificationAsReadAsync(int notificationId, CancellationToken cancellationToken = default)
        {
            var notification = await orderRepository.GetNotificationByIdAsync(notificationId, cancellationToken);
            if (notification is null)
            {
                return null;
            }

            if (!notification.IsRead)
            {
                notification.IsRead = true;
                await orderRepository.SaveChangesAsync(cancellationToken);
            }

            return MapToNotificationDto(notification);
        }

        public async Task<int> MarkUserNotificationsAsReadAsync(int userId, List<int>? notificationIds = null, CancellationToken cancellationToken = default)
        {
            var unreadNotifications = await orderRepository.GetUnreadNotificationsByUserAsync(userId, notificationIds, cancellationToken);
            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
            }

            if (unreadNotifications.Count > 0)
            {
                await orderRepository.SaveChangesAsync(cancellationToken);
            }

            return unreadNotifications.Count;
        }

        private static OrderSummaryDto MapToSummaryDto(Order order)
        {
            return new OrderSummaryDto
            {
                Id = order.Id,
                UserId = order.UserId,
                UserName = order.User?.Name ?? string.Empty,
                ApproverId = order.ApproverId,
                ApproverName = order.Approver?.Name,
                CreationDate = order.CreationDate,
                Status = order.Status,
                ExpectedDeliveryDate = order.ExpectedDeliveryDate,
                Items = order.OrderItems.Select(oi => new OrderItemSummaryDto
                {
                    Id = oi.Id,
                    EquipmentId = oi.EquipmentId,
                    EquipmentName = oi.Equipment?.Name ?? string.Empty,
                    Quantity = oi.Quantity
                }).ToList()
            };
        }

        private static OrderDetailDto MapToDetailDto(Order order)
        {
            return new OrderDetailDto
            {
                Id = order.Id,
                UserId = order.UserId,
                UserName = order.User?.Name ?? string.Empty,
                ApproverId = order.ApproverId,
                ApproverName = order.Approver?.Name,
                CreationDate = order.CreationDate,
                Status = order.Status,
                ExpectedDeliveryDate = order.ExpectedDeliveryDate,
                Items = order.OrderItems.Select(oi => new OrderItemDetailDto
                {
                    Id = oi.Id,
                    EquipmentId = oi.EquipmentId,
                    EquipmentName = oi.Equipment?.Name ?? string.Empty,
                    Description = oi.Equipment?.Descritpion ?? string.Empty,
                    Category = oi.Equipment?.Category ?? default,
                    Quantity = oi.Quantity
                }).ToList(),
                Trackings = order.Trackings
                    .OrderByDescending(t => t.Date)
                    .Select(t => new TrackingDto
                    {
                        Id = t.Id,
                        ApproverId = t.ApproverId,
                        ApproverName = t.Approver?.Name ?? string.Empty,
                        Date = t.Date,
                        Status = t.Status,
                        Comments = t.Comments
                    }).ToList()
            };
        }

        private static TrackingNotificationDto MapToNotificationDto(TrackingNotification notification)
        {
            return new TrackingNotificationDto
            {
                Id = notification.Id,
                UserId = notification.UserId,
                TrackingId = notification.TrackingId,
                OrderId = notification.Tracking?.OrderId ?? 0,
                Status = notification.Tracking?.Status ?? default,
                Message = notification.Message,
                Date = notification.Date,
                IsRead = notification.IsRead
            };
        }
    }
}
