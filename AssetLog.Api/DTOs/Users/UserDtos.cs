
using AssetLog.Api.Models.Enums;

namespace AssetLog.Api.DTOs
{

    public class UserDto
    {
        public int Id { get; set; }
        public String Name { get; set; } = string.Empty;
        public String Email { get; set; } = string.Empty;
        public RoleEnum Role { get; set; }
        public DepartmentEnum Department { get; set; }
        public bool IsActive { get; set; }
        public DateTime EndDate { get; set; }
        public TypeOfContractEnum TypeOfContract { get; set; }
    }

}