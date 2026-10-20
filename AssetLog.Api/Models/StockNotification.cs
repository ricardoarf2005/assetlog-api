namespace AssetLog.Api.Models
{
    public class StockNotification
    {
        public int Id { get; set; }
        public int ApproverId { get; set; }
        public int EquipmentId { get; set; }
        public String? Message { get; set; }
    }
}
