using AssetLog.Api.DTOs;
using AssetLog.Api.Models.Enums;
using AssetLog.Api.Services.OrderService;
using Microsoft.AspNetCore.Mvc;

namespace AssetLog.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController(IOrderService orderService) : ControllerBase
    {
        // GET /api/order/user/{userId} -> orders do user
        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<List<OrderSummaryDto>>> GetUserOrders(
            [FromRoute] int userId,
            CancellationToken cancellationToken)
        {
            var orders = await orderService.GetUserOrdersAsync(userId, cancellationToken);
            return Ok(orders);
        }

        // GET /api/order/approver/{approverId} -> orders do approver
        [HttpGet("approver/{approverId:int}")]
        public async Task<ActionResult<List<OrderSummaryDto>>> GetApproverOrders(
            [FromRoute] int approverId,
            CancellationToken cancellationToken)
        {
            var orders = await orderService.GetApproverOrdersAsync(approverId, cancellationToken);
            return Ok(orders);
        }

        // GET /api/order/status/{status} -> orders com um certo estado
        [HttpGet("status/{status}")]
        public async Task<ActionResult<List<OrderSummaryDto>>> GetOrdersByStatus(
            [FromRoute] StatusEnum status,
            CancellationToken cancellationToken)
        {
            var orders = await orderService.GetOrdersByStatusAsync(status, cancellationToken);
            return Ok(orders);
        }

        // GET /api/order/{orderId} & GET /api/order/user/{userId}/{orderId} -> Detalhes da order
        [HttpGet("{orderId:int}")]
        [HttpGet("user/{userId:int}/{orderId:int}")]
        public async Task<ActionResult<OrderDetailDto>> GetOrderDetails(
            [FromRoute] int orderId,
            [FromRoute] int? userId,
            CancellationToken cancellationToken)
        {
            var order = await orderService.GetOrderDetailsAsync(orderId, userId, cancellationToken);
            if (order is null)
            {
                return NotFound(new { message = $"Order with ID {orderId} was not found." });
            }

            return Ok(order);
        }

        // POST /api/order -> adicionar order
        [HttpPost]
        public async Task<ActionResult<OrderDetailDto>> CreateOrder(
            [FromBody] CreateOrderRequestDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                var createdOrder = await orderService.CreateOrderAsync(request, cancellationToken);
                return CreatedAtAction(nameof(GetOrderDetails), new { orderId = createdOrder.Id }, createdOrder);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT /api/order/{orderId}/status -> atualizar estado
        [HttpPut("{orderId:int}/status")]
        public async Task<ActionResult<OrderDetailDto>> UpdateOrderStatus(
            [FromRoute] int orderId,
            [FromBody] UpdateOrderStatusRequestDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                var updatedOrder = await orderService.UpdateOrderStatusAsync(orderId, request, cancellationToken);
                return Ok(updatedOrder);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // PUT /api/order/{orderId}/expected-delivery-date -> alteração da data de entrega expectável
        [HttpPut("{orderId:int}/expected-delivery-date")]
        public async Task<ActionResult<OrderDetailDto>> UpdateExpectedDeliveryDate(
            [FromRoute] int orderId,
            [FromBody] UpdateExpectedDeliveryDateRequestDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                var updatedOrder = await orderService.UpdateExpectedDeliveryDateAsync(orderId, request, cancellationToken);
                return Ok(updatedOrder);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // GET /api/order/notification/user/{userId}?isRead=true|false -> mostrar todas (filtragem por lidas e não lidas)
        [HttpGet("notification/user/{userId:int}")]
        public async Task<ActionResult<List<TrackingNotificationDto>>> GetUserNotifications(
            [FromRoute] int userId,
            [FromQuery] bool? isRead,
            CancellationToken cancellationToken)
        {
            var notifications = await orderService.GetUserNotificationsAsync(userId, isRead, cancellationToken);
            return Ok(notifications);
        }

        // PUT /api/order/notification/{notificationId}/read -> atualizar uma notificação de não lida para lida
        [HttpPut("notification/{notificationId:int}/read")]
        public async Task<ActionResult<TrackingNotificationDto>> MarkNotificationAsRead(
            [FromRoute] int notificationId,
            CancellationToken cancellationToken)
        {
            var notification = await orderService.MarkNotificationAsReadAsync(notificationId, cancellationToken);
            if (notification is null)
            {
                return NotFound(new { message = $"Notification with ID {notificationId} was not found." });
            }

            return Ok(notification);
        }

        // PUT /api/order/notification/user/{userId}/read -> atualizar todas (ou lista de IDs) de não lidas para lidas
        [HttpPut("notification/user/{userId:int}/read")]
        public async Task<IActionResult> MarkUserNotificationsAsRead(
            [FromRoute] int userId,
            [FromBody] List<int>? notificationIds,
            CancellationToken cancellationToken)
        {
            var updatedCount = await orderService.MarkUserNotificationsAsReadAsync(userId, notificationIds, cancellationToken);
            return Ok(new { updatedCount });
        }
    }
}
