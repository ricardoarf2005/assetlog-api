namespace AssetLog.Api.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int EquipmentId { get; set; }
        public int Quantity { get; set; }

        public Order Order { get; set; } = null!;
        public Equipment Equipment { get; set; } = null!;
    }
}
