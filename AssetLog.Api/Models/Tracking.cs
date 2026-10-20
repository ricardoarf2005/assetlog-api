using AssetLog.Api.Models.Enums;

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
    }
}
