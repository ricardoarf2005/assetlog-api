using AssetLog.Api.Models.Enums;

namespace AssetLog.Api.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public String Name { get; set; } = string.Empty;
        public String Description { get; set; } = string.Empty;
        public CategoryEnum Category { get; set; }
        public bool IsActive { get; set; }
        public int MinStock { get; set; }
        public int Stock { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new();
        public List<StockNotification> StockNotifications { get; set; } = new();
    }
}
