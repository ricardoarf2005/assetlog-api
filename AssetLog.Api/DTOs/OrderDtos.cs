using System.ComponentModel.DataAnnotations;
using AssetLog.Api.Models.Enums;

namespace AssetLog.Api.DTOs
{
    public class CreateOrderItemRequestDto
    {
        [Required]
        public int EquipmentId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public int Quantity { get; set; }
    }

    public class CreateOrderRequestDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "An order must contain at least one item.")]
        public List<CreateOrderItemRequestDto> Items { get; set; } = new();

        public DateTime? ExpectedDeliveryDate { get; set; }
    }

    public class UpdateOrderStatusRequestDto
    {
        [Required]
        public int ApproverId { get; set; }

        [Required]
        public StatusEnum Status { get; set; }

        public string? Comments { get; set; }
    }

    public class UpdateExpectedDeliveryDateRequestDto
    {
        [Required]
        public int ApproverId { get; set; }

        [Required]
        public DateTime ExpectedDeliveryDate { get; set; }

        public string? Comments { get; set; }
    }

    public class OrderItemSummaryDto
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }

    public class OrderItemDetailDto
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public CategoryEnum Category { get; set; }
        public int Quantity { get; set; }
    }

    public class TrackingDto
    {
        public int Id { get; set; }
        public int ApproverId { get; set; }
        public string ApproverName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public StatusEnum Status { get; set; }
        public string? Comments { get; set; }
    }

    public class OrderSummaryDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int? ApproverId { get; set; }
        public string? ApproverName { get; set; }
        public DateTime CreationDate { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public List<OrderItemSummaryDto> Items { get; set; } = new();
    }

    public class OrderDetailDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int? ApproverId { get; set; }
        public string? ApproverName { get; set; }
        public DateTime CreationDate { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public List<OrderItemDetailDto> Items { get; set; } = new();
        public List<TrackingDto> Trackings { get; set; } = new();
    }

    public class TrackingNotificationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TrackingId { get; set; }
        public int OrderId { get; set; }
        public StatusEnum Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
    }
}
