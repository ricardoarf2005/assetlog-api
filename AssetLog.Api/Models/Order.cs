using AssetLog.Api.Models.Enums;

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
    }
}
