using AssetLog.Api.Models.Enums;

namespace AssetLog.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public String Name { get; set; } = string.Empty;
        public String Email { get; set; } = string.Empty;
        public RoleEnum Role { get; set; } 
        //TODO: Hash the password
        public String PasswordHash { get; set; } = string.Empty;
        public DepartmentEnum Department { get; set; }
        public bool IsActive { get; set; }
        public DateTime EndDate { get; set; }
        public TypeOfContractEnum TypeOfContract { get; set; }
    }
}
