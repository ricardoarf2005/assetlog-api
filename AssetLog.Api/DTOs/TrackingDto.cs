using System.ComponentModel.DataAnnotations;
using AssetLog.Api.Models.Enums;

namespace AssetLog.Api.DTOs.Tracking;

public class TrackingDto
{
    public int Id { get; set; }
    public int ApproverId { get; set; }
    public string ApproverName { get; set; } = string.Empty;
    public int OrderId { get; set; }
    public DateTime Date { get; set; }
    public StatusEnum Status { get; set; }
    public string? Comments { get; set; }
    public TrackingNotificationDto? Notification { get; set; }
}

 public class TrackingNotificationDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int TrackingId { get; set; }
    public string? UserName { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public bool IsRead { get; set; }
}