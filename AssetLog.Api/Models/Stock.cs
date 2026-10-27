namespace AssetLog.Api.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public int Quantity { get; set; }
        public bool IsRead { get; set; }

        public Equipment Equipment { get; set; } = null!;
    }
}