namespace AssetLog.Api.Models
{
    public class TrackingNotification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TrackingId { get; set; }
        public String Message { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}
