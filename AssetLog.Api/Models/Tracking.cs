using AssetLog.Api.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetLog.Api.Models
{
    public class Tracking
    {
        public int Id { get; set; }
        public int ApproverId { get; set; }
        public int OrderId { get; set; }
        public DateTime Date { get; set; }
        public StatusEnum Status { get; set; }
        public String? Comments { get; set; }
        public int TrackingNotificationId { get; set; }


        [ForeignKey("ApproverId")]
        public User Approver { get; set; } = null!;
        public Order Order { get; set; } = null!;
        public TrackingNotification TrackingNotification { get; set; } = null!;
    }
}
