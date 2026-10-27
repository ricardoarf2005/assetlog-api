using AssetLog.Api.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetLog.Api.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ApproverId { get; set; }
        public DateTime CreationDate { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }

        public User User { get; set; } = null!;
        [ForeignKey("ApproverId")]
        public User Approver { get; set; } = null!;

        public List<OrderItem> OrderItems { get; set; } = new();
        public List<Tracking> Trackings { get; set; } = new();
    }
}
