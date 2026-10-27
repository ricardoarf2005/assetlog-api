using System.ComponentModel.DataAnnotations.Schema;

namespace AssetLog.Api.Models
{
    public class StockNotification
    {
        public int Id { get; set; }
        public int ApproverId { get; set; }
        public int EquipmentId { get; set; }
        public String? Message { get; set; }

        [ForeignKey("ApproverId")]
        public User Approver { get; set; } = null!;
        public Equipment Equipment { get; set; } = null!;
    }
}
