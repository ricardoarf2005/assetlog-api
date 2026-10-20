namespace AssetLog.Api.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public String Name { get; set; } = string.Empty;
        public String Descritpion { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
        public int MinStock { get; set; }

        //TODO: add foreign keys and navigations
        //run migrations
    }
}
